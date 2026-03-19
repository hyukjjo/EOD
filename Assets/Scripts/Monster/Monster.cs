using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using MikeNspired.UnityXRHandPoser;
using System.Collections.Generic;
using System.Threading;

public enum MonsterType
{
    NORMAL = 0,
    BOSS
}

public enum ShotType
{
    BODYSHOT = 0,
    HEADSHOT
}

public class Monster : PoolObject, IDamageable
{
    public float Hp;
    public float Dam;
    public float Def;
    public float Spd;
    public int Exp;
    public float CriticalShotValue;
    public bool InsideTheBase = false;

    [SerializeField]
    private MonsterType _monsterType = MonsterType.NORMAL;
    [SerializeField]
    private List<Material> _materials = new List<Material>();
    [SerializeField]
    private SkinnedMeshRenderer _skinnedMeshRenderer;
    [SerializeField]
    private Transform MoveTarget;
    [SerializeField]
    private Transform AttackTarget;
    [SerializeField]
    private Transform HitTarget;
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private float _currentHp = 0f;
    [SerializeField]
    private Collider _headCollider;
    [SerializeField]
    private GameObject _minimapIcon;
    [SerializeField]
    private Transform _rayCastPos;
    [SerializeField]
    private GameObject _headShotEffect;

    private Collider[] _colliders;
    private bool ReadyToAttack = false;
    private bool IsDying = false;
    public bool isDying => IsDying;
    private NavMeshAgent _agent;
    private Rigidbody _rigidBody;

    private float _originSpd = 0;
    private bool _isSlow = false;
    private List<BloodEffect> _bloodEffects = new List<BloodEffect>();

    [Header("Sound")]
    [SerializeField]
    private AudioSource _audioSource3D;
    [SerializeField]
    private AudioSource _audioSource2D;
    [SerializeField]
    private AudioClip _idleSound;
    [SerializeField]
    private AudioClip _headShotKillSound;
    [SerializeField]
    private AudioClip _bodyShotKillSound;

    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _colliders = GetComponentsInChildren<Collider>();
        _rigidBody = GetComponent<Rigidbody>();
        GameManager.Instance.MaintenanceStart += ForcedDeath;
        InitMonster();
        _originSpd = Spd;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.D))
        //    HitByPlayer(2.0f, ShotType.BODYSHOT);

        if (IsDying)
        {
            return;
        }
        else
        {
            if (ReadyToAttack)
            {
                //AttackAnimation();
            }
            else if (!IsDying)
            {
                MoveToTarget();
            }
        }

        if (Physics.Raycast(_rayCastPos.position, _rayCastPos.forward, out RaycastHit hit, 2f))
        {
            if (hit.collider.CompareTag("BaseWall") || hit.collider.CompareTag("Player"))
            {
                AttackTarget = hit.transform;
                ReadyToAttack = true;
                AttackAnimation(new Vector3(hit.point.x, 0f, hit.point.z));

            }
        }
        else
        {
            AttackTarget = null;
            ReadyToAttack = false;
        }
    }

    private void InitMonster()
    {
        _animator.Play("Idle");
        _currentHp = Hp;

        if (!_monsterType.Equals(MonsterType.BOSS))
        {
            SetMonsterRandomMaterial();
        }
    }

    private void SetMonsterRandomMaterial()
    {
        _skinnedMeshRenderer.material = _materials[Random.Range(0, _materials.Count)];
    }

    public void HitByPlayer(float dam, ShotType shotType)
    {
        _currentHp -= dam;

        Debug.Log("Current HP : " + _currentHp);
        if (_currentHp <= 0)
        {
            if (_monsterType.Equals(MonsterType.BOSS))
            {
                GameManager.Instance.BossMonsterKill();
            }
            else
            {
                GameManager.Instance.MonsterKill();
            }

            foreach (var item in _bloodEffects)
            {
                ObjectPoolManager.Instance.Despawn(item);
            }

            _bloodEffects.Clear();

            switch (shotType)
            {
                case ShotType.BODYSHOT:
                    StartCoroutine(DeadByBodyShot());
                    break;
                case ShotType.HEADSHOT:
                    StartCoroutine(DeadByHeadShot());
                    break;
                default:
                    break;
            }
        }
    }

    private void ResetMonster()
    {
        transform.position = Vector3.zero;

        _currentHp = Hp;
        foreach (var collider in _colliders)
        {
            collider.enabled = true;
        }
        _headShotEffect.SetActive(false);
        _audioSource3D.clip = _idleSound;
        _audioSource3D.loop = true;
        IsDying = false;
        ReadyToAttack = false;
        _minimapIcon.SetActive(true);
        InsideTheBase = false;
        SetSlow(false);
        StageMonsterHolder.Instance.RemoveMonster(this);
        ObjectPoolManager.Instance.Despawn(GetComponent<PoolObject>());
    }

    public Transform GetMoveTarget()
    {
        return MoveTarget;
    }

    public void SetMoveTarget(Transform targetTransform)
    {
        MoveTarget = targetTransform;
    }

    private void MoveToTarget()
    {
        if (_isSlow)
            _animator.Play("Walk");
        else
            _animator.Play("Run");
        Vector3 dir = MoveTarget.position - transform.position;
        dir.y = 0f;
        Quaternion rot = Quaternion.LookRotation(dir.normalized);
        transform.rotation = rot;
        _agent.speed = Spd;
        _agent.isStopped = false;
        _agent.destination = MoveTarget.position;
    }

    private void AttackAnimation(Vector3 hitPos)
    {
        if (AttackTarget == null || AttackTarget.gameObject.activeSelf == false)
        {
            StopAttack();
            return;
        }
        _agent.isStopped = true;
        _agent.velocity = Vector3.zero;
        _rigidBody.velocity = Vector3.zero;
        transform.LookAt(hitPos);
        _animator.Play("Attack");
    }

    private void StopAttack()
    {
        ReadyToAttack = false;
    }

    public void CheckCloseTurret()
    {
        NewTurret[] tems = FindObjectsOfType<NewTurret>();

        List<NewTurret> turrets = new List<NewTurret>();

        for (int i = 0; i < tems.Length; i++)
        {
            if (tems[i]._isActive)
                turrets.Add(tems[i]);
        }

        if (turrets.Count > 0)
        {
            int num = 0;
            float dis = Vector3.Distance(turrets[0].transform.position, transform.position);

            for (int i = 1; i < turrets.Count; i++)
            {
                float tem = Vector3.Distance(turrets[i].transform.position, transform.position);

                if (dis > tem)
                    num = i;
            }

            SetMoveTarget(turrets[num].transform);
        }
        else if (turrets.Count == 0)
        {
            SetMoveTarget(GameManager.Instance.GetPlayer().transform);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            var dam = collision.gameObject.GetComponent<Damage>().Dam;
            var shotType = ShotType.BODYSHOT;

            if (collision.contacts[0].thisCollider.Equals(_headCollider))
            {
                dam *= CriticalShotValue;
                shotType = ShotType.HEADSHOT;
            }

            var bloodEffect = ObjectPoolManager.Instance.Spawn("Blood_Default").GetComponent<BloodEffect>();
            bloodEffect.transform.position = collision.transform.position;
            bloodEffect.transform.LookAt(GameManager.Instance.GetPlayer().transform);
            bloodEffect.transform.Rotate(90f, 0f, 0f);
            bloodEffect.transform.SetParent(transform);
            _bloodEffects.Add(bloodEffect);

            HitByPlayer(dam, shotType);
        }
    }



    private IEnumerator DeadByBodyShot()
    {
        IsDying = true;
        foreach (var collider in _colliders)
        {
            collider.enabled = false;
        }
        _audioSource3D.Stop();
        _audioSource2D.clip = _bodyShotKillSound;
        _audioSource2D.Play();
        _minimapIcon.SetActive(false);
        _agent.isStopped = true;
        ReadyToAttack = false;
        MoveTarget = null;
        AttackTarget = null;
        _animator.Play("FallingForward");
        yield return new WaitForSeconds(5.0f);
        ResetMonster();
    }

    private IEnumerator DeadByHeadShot()
    {
        IsDying = true;
        foreach (var collider in _colliders)
        {
            collider.enabled = false;
        }
        _headShotEffect.SetActive(true);
        _audioSource3D.Stop();
        _audioSource2D.clip = _headShotKillSound;
        _audioSource2D.Play();
        _minimapIcon.SetActive(false);
        _agent.isStopped = true;
        ReadyToAttack = false;
        MoveTarget = null;
        AttackTarget = null;
        _animator.Play("FallingBack");
        yield return new WaitForSeconds(5.0f);
        ResetMonster();
    }

    private void ForcedDeath()
    {
        IsDying = true;
        foreach (var collider in _colliders)
        {
            collider.enabled = false;
        }
        ReadyToAttack = false;
        MoveTarget = null;
        AttackTarget = null;
        ResetMonster();
    }

    public void DamageTarget()
    {
        if (AttackTarget)
        {
            MonsterTarget target = AttackTarget.GetComponent<MonsterTarget>();
            target.Damage(Dam, transform, StopAttack);
        }
    }

    public void TakeDamage(float damage, GameObject damager)
    {
        if (damager.CompareTag("Grenade"))
            HitByPlayer(damage, ShotType.BODYSHOT);
    }

    public void SetSlow(bool isSlow)
    {
        _isSlow = isSlow;

        if (isSlow)
            Spd = _originSpd * 0.05f;
        else
            Spd = _originSpd;
    }

    public Vector3 GetHitPosition()
    {
        Collider HitCol = HitTarget.GetComponent<Collider>();

        Bounds bounds = HitCol.bounds;

        return new Vector3(Random.Range(bounds.min.x, bounds.max.x), Random.Range(bounds.min.y, bounds.max.y), Random.Range(bounds.min.z, bounds.max.z));
    }
}