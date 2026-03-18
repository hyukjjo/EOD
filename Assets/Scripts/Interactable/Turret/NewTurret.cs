using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class NewTurret : MonsterTarget
{
    enum TurretState
    {
        Able = 0,
        Overheat
    }

    private XRGrabInteractable _interactable;
    private Rigidbody _rigidbody;
    private Collider[] _colliders;

    [SerializeField] private Transform centerPoint;
    [SerializeField] private float _viewArea;
    [Range(0, 360)]
    [SerializeField] private float _viewAngle;
    [SerializeField] private Transform m_HorizontalAxisTransform;
    [SerializeField] private Transform m_VerticalAxisTransform;
    [SerializeField] private float m_RotatingSpeed = 10;

    [SerializeField] private LayerMask _targetLayer;
    [SerializeField] private LayerMask _ignoreLayer;
    [SerializeField] private float _shotSecond = .5f;

    [SerializeField] private Animator m_BarrelAnimator;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private Rigidbody projectilePrefab;
    [SerializeField] private float _bulletSpeed = 150;
    [SerializeField] private AudioSource _fireAudio;

    private List<Monster> _inSideMonsters = new List<Monster>();
    private List<Transform> _shotTargets = new List<Transform>();
    private Transform _fireTarget = null;

    private const float _bulletSpreadAngle = 1;

    public bool _isActive = false;
    private BaseChecker _baseChecker;

    [SerializeField] private TurretState _state = TurretState.Able;
    [SerializeField] private float _ableShottingTime = 0f;
    [SerializeField] private float _overheatingTime = 0f;

    [SerializeField] private AudioSource _audioSourceSetup;
    [SerializeField] private AudioSource _audioSourceIdle;
    [SerializeField] private AudioClip _clipIdle;
    [SerializeField] private AudioClip _clipOverheat;
    [SerializeField] private AudioClip _clipDead;
    [SerializeField] private ParticleSystem[] _effects;

    [SerializeField] private GameObject _DummyCombineBettery;
    private bool _isCombine = false;

    private Coroutine cor = null;

    private void Start()
    {
        _interactable = GetComponent<XRGrabInteractable>();
        _rigidbody = GetComponent<Rigidbody>();
        _colliders = GetComponents<Collider>();
        _baseChecker = FindObjectOfType<BaseChecker>(true);
        _baseChecker.CheckMonster += () => { TargettingInsideMonsters(); };

        _interactable.selectEntered.AddListener(x => Grab());
        _interactable.selectExited.AddListener(x => Release());
        //_interactable.onActivate.AddListener(x => _isActive = true);
        //GameManager.Instance.RoundStart += () => { _interactable.enabled = false; };
        //GameManager.Instance.MaintenanceStart += () => { _interactable.enabled = true; };

        _effects = GetComponentsInChildren<ParticleSystem>();
        //cor = StartCoroutine(ShotTarget(2f));
        //_state = TurretState.Able;
        //transform.SetParent(null);
        //_audioSourceSetup.Play();
        _DummyCombineBettery.SetActive(false);

        _rigidbody.constraints = RigidbodyConstraints.FreezeAll;
    }

    private void OnDestroy()
    {
        if(_baseChecker)
            _baseChecker.CheckMonster -= () => { TargettingInsideMonsters(); };
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isCombine)
            return;

        if (!_isCombine)
        {
            Bettery bettery = collision.collider.GetComponent<Bettery>();

            if (bettery)
            {
                UseInventory betteryinventory = collision.collider.GetComponent<UseInventory>();

                if (bettery.IsGrab && !betteryinventory.IsInventoryItem)
                {
                    _DummyCombineBettery.SetActive(true);
                }
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (_isCombine)
            return;

        if (!_isCombine)
        {
            Bettery bettery = collision.collider.GetComponent<Bettery>();

            if (bettery)
            {
                UseInventory betteryinventory = collision.collider.GetComponent<UseInventory>();

                if (!bettery.IsGrab && !betteryinventory.IsInventoryItem)
                {
                    _isCombine = true;
                    _shotSecond *= 0.5f;
                    _DummyCombineBettery.SetActive(true);
                    Destroy(bettery.gameObject);
                }
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (_isCombine)
            return;

        if (!_isCombine)
        {
            Bettery bettery = collision.collider.GetComponent<Bettery>();

            if (bettery)
            {
                UseInventory betteryinventory = collision.collider.GetComponent<UseInventory>();

                if (bettery.IsGrab && !betteryinventory.IsInventoryItem)
                    _DummyCombineBettery.SetActive(false);
            }
        }
    }

    private void Grab()
    {
        _rigidbody.constraints = RigidbodyConstraints.None;

        foreach (var col in _colliders)
        {
            col.isTrigger = true;
        }

        _isActive = false;

        m_HorizontalAxisTransform.localRotation = Quaternion.identity;
        m_VerticalAxisTransform.localRotation = Quaternion.identity;

        if (cor != null)
            StopCoroutine(cor);

        _audioSourceSetup.Stop();
    }

    private void Release()
    {
        _isActive = true;

        foreach (var col in _colliders)
        {
            col.isTrigger = false;
        }

        _state = TurretState.Able;
        //_interactable.enabled = false;
        transform.SetParent(null);
        _audioSourceSetup.Play();
        cor = StartCoroutine(ShotTarget(2f));
    }

    private IEnumerator ShotTarget(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        _rigidbody.constraints = RigidbodyConstraints.FreezeAll;

        float shotTime = 0f;
        float overheatingTime = 0f;

        while (gameObject.activeSelf)
        {
            _fireTarget = null;

            switch (_state)
            {
                case TurretState.Able:

                    Collider[] TargetCollider = Physics.OverlapSphere(transform.position, _viewArea, _targetLayer);

                    if (TargetCollider.Length > 0)
                    {
                        _shotTargets.Clear();

                        foreach (var target in TargetCollider)
                        {
                            Monster mon = target.GetComponentInParent<Monster>();

                            Vector3 direction = mon.GetHitPosition() - transform.position;
                            if (Vector3.Dot(direction.normalized, transform.forward) > GetAngle(_viewAngle / 2).z)
                            {
                                Vector3 dirToTarget = (mon.GetHitPosition() - transform.position).normalized;

                                float dstToTarget = Vector3.Distance(transform.position, mon.GetHitPosition());

                                Debug.DrawRay(centerPoint.position, dirToTarget * 10f, Color.red, 2f);

                                if (Physics.Raycast(centerPoint.position, dirToTarget, out RaycastHit hit, dstToTarget, ~_ignoreLayer))
                                {
                                    Monster monster = hit.collider.GetComponentInParent<Monster>();

                                    if (monster)
                                    {
                                        if (!_shotTargets.Contains(target.transform))
                                            _shotTargets.Add(target.transform);
                                    }
                                }
                            }
                        }

                        foreach (var target in _shotTargets)
                        {
                            if (!_fireTarget)
                            {
                                if (!target.GetComponentInParent<Monster>().isDying)
                                    _fireTarget = target;
                                else
                                    continue;
                            }

                            float Distance = Vector3.Distance(centerPoint.position, target.position);

                            if (Distance < Vector3.Distance(centerPoint.position, _fireTarget.position))
                            {
                                _fireTarget = target;
                            }
                        }


                        if (_fireTarget)
                        {
                            // Rotate Turret
                            Vector3 directionToTarget = _fireTarget.GetComponentInParent<Monster>().GetHitPosition() - centerPoint.position;
                            Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
                            m_HorizontalAxisTransform.rotation = Quaternion.Lerp(m_HorizontalAxisTransform.rotation, Quaternion.Euler(0, lookRotation.eulerAngles.y, 0), Time.deltaTime * m_RotatingSpeed);
                            m_VerticalAxisTransform.localRotation = Quaternion.Lerp(m_VerticalAxisTransform.localRotation, Quaternion.Euler(lookRotation.eulerAngles.x, 0, 0), Time.deltaTime * m_RotatingSpeed);

                            Fire();

                            shotTime += _shotSecond;

                            if (shotTime > _ableShottingTime)
                            {
                                shotTime = 0f;
                                _state = TurretState.Overheat;
                            }

                            yield return new WaitForSeconds(_shotSecond);

                            if (_fireTarget)
                            {
                                if (_fireTarget.GetComponentInParent<Monster>(true).isDying)
                                {
                                    _shotTargets.Remove(_fireTarget);
                                }
                            }
                        }
                    }
                    if (_audioSourceIdle.clip != _clipIdle)
                    {
                        _audioSourceIdle.clip = _clipIdle;
                        _audioSourceIdle.loop = true;
                        _audioSourceIdle.Play();
                    }
                    yield return new WaitForEndOfFrame();
                    break;
                case TurretState.Overheat:
                    overheatingTime += Time.deltaTime;

                    if (overheatingTime > _overheatingTime)
                    {
                        overheatingTime = 0f;
                        _state = TurretState.Able;
                    }
                    if (_audioSourceIdle.clip != _clipOverheat)
                    {
                        _audioSourceIdle.clip = _clipOverheat;
                        _audioSourceIdle.loop = false;
                        _audioSourceIdle.Play();
                    }
                    yield return new WaitForEndOfFrame();
                    break;
                default:
                    break;
            }
        }
        yield return null;
    }

    private void Fire()
    {
        foreach(var item in _effects)
        {
            item.Play();
        }
        Vector3 shotDirection = Vector3.Slerp(_firePoint.forward, Random.insideUnitSphere, _bulletSpreadAngle / 180f);
        var bullet = Instantiate(projectilePrefab);

        var Colliders = GetComponentsInChildren<Collider>(true);
        var bulletCollider = bullet.GetComponentInChildren<Collider>();
        foreach (var c in Colliders) Physics.IgnoreCollision(c, bulletCollider);

        bullet.transform.SetPositionAndRotation(_firePoint.position, Quaternion.LookRotation(shotDirection));
        bullet.AddForce(bullet.transform.forward * _bulletSpeed, ForceMode.VelocityChange);

        m_BarrelAnimator.CrossFadeInFixedTime("Fire", 0.01f);

        if (_fireAudio)
            _fireAudio.PlayOneShot(_fireAudio.clip);
    }

    public Vector3 GetAngle(float AngleInDegree)
    {
        return new Vector3(Mathf.Sin(AngleInDegree * Mathf.Deg2Rad), 0, Mathf.Cos(AngleInDegree * Mathf.Deg2Rad));
    }

    public Vector3 DirFromAngle(float angleDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleDegrees += transform.eulerAngles.y;
        }

        return new Vector3(Mathf.Cos((-angleDegrees + 90) * Mathf.Deg2Rad), 0, Mathf.Sin((-angleDegrees + 90) * Mathf.Deg2Rad));
    }

    public override void Damage(float damage, Transform transform = null, System.Action action = null)
    {
        base.Damage(damage);

        if (hp <= 0)
        {
            _isActive = false;
            action?.Invoke();
            _audioSourceIdle.Stop();
            _audioSourceSetup.clip = _clipDead;
            _audioSourceSetup.Play();
            foreach (var monster in _inSideMonsters)
            {
                monster.CheckCloseTurret();
            }
            Invoke(nameof(Dead), _clipDead.length);
            return;
        }
    }

    private void TargettingInsideMonsters()
    {
        if (_isActive)
        {
            _inSideMonsters = StageMonsterHolder.Instance.GetInsideMonsters();

            foreach (var monster in _inSideMonsters)
            {
                monster.CheckCloseTurret();
            }
        }
    }

    private void Dead()
    {
        _baseChecker.CheckMonster -= () => { TargettingInsideMonsters(); };
        Destroy(gameObject);
    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.DrawWireArc(transform.position, Vector3.up, transform.forward, 360, _viewArea);

        Vector3 viewAngleA = DirFromAngle(-_viewAngle / 2, false);
        Vector3 viewAngleB = DirFromAngle(_viewAngle / 2, false);

        Handles.DrawLine(transform.position, transform.position + viewAngleA * _viewArea);
        Handles.DrawLine(transform.position, transform.position + viewAngleB * _viewArea);
    }
#endif
}
