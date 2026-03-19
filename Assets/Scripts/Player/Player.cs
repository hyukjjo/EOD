using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using MikeNspired.UnityXRHandPoser;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

public class Player : MonsterTarget
{
    public float Spd;
    public Transform MinimapIcon;
    public int PlayerGold;

    [Header("HIT EFFECT")]
    [SerializeField]
    private Image _hitEffect;
    [SerializeField]
    private Sprite _bloodImage;
    [SerializeField]
    private Sprite _bloodLeftImage;
    [SerializeField]
    private Sprite _bloodRightImage;
    [SerializeField]
    private Image _fadeEffect;
    [Header("PLAYER GUN GRAB STATE")]
    [SerializeField]
    private XRDirectInteractor _leftInteractor;
    [SerializeField]
    private XRDirectInteractor _rightInteractor;
    public ProjectileWeapon CurrentGun;
    [SerializeField]
    private bool _isGrabbingGun = false;
    [SerializeField]
    private bool _isMagazineLoaded = false;
    public int CurrentBullet;
    public int MaxBullet;

    private Coroutine _corHitEffect = null;
    private Color _hitColor = new Color(1f, 1f, 1f, 1f);
    private Color _clearColor = new Color(1f, 1f, 1f, 0f);

    public InputActionReference leftHandMoveAction;
    [SerializeField] private AudioSource _audioSourceHit;
    [SerializeField] private AudioSource _audioSourceFoot;
   // [SerializeField] private OVRPassthroughLayer _vrPassthroughLayer;

    private XRInteractionManager _interactionManager;
    private XRDirectInteractor[] _directInteractors;

    void Start()
    {
        if(_interactionManager == null)
            _interactionManager = FindObjectOfType<XRInteractionManager>();
        if(_directInteractors == null)
            _directInteractors = FindObjectsOfType<XRDirectInteractor>();

        _hitEffect.color = _clearColor;
        leftHandMoveAction.GetInputAction().performed += PlayerPerformed;
        leftHandMoveAction.GetInputAction().canceled += PlayerCanceled;

        GameManager.Instance.PlayerDead += PlayerDead;
        GameManager.Instance.MaintenanceStart += () =>
        {
            if (GameManager.Instance.IsInfiniteMode)
                return;

            switch (GameManager.Instance.GetRound())
            {
                case 1:
                    PlayerGold = 1000;
                    break;
                case 2:
                    PlayerGold = 2500;
                    break;
                case 3:
                    PlayerGold = 6500;
                    break;
                default:
                    break;
            }
        };
    }

    private void PlayerCanceled(InputAction.CallbackContext obj)
    {
        ActiveFootSound(false);
    }

    private void PlayerPerformed(InputAction.CallbackContext obj)
    {
        ActiveFootSound(true);
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.R))
        //    PlayerDead();

        //if(Input.GetKeyDown(KeyCode.D))
        //{
        //    if (_corHitEffect != null)
        //        StopCoroutine(_corHitEffect);

        //    _corHitEffect = StartCoroutine(HitEffect());
        //}

        //foreach (var interactor in _interactors)
        //{
        //    if (interactor.selectTarget == null)
        //    {
        //        _isGrabbingGun = false;
        //        _isMagazineLoaded = false;
        //        break;
        //    }

        //    if(interactor.selectTarget.GetComponent<ProjectileWeapon>())
        //    {
        //        _isGrabbingGun = true;

        //        if(interactor.selectTarget.GetComponent<ProjectileWeapon>().gunCocked)
        //        {
        //            CurrentGun = interactor.selectTarget.GetComponent<ProjectileWeapon>();
        //            _isMagazineLoaded = true;
        //        }
        //        else
        //        {
        //            _isMagazineLoaded = false;
        //        }
        //    }
        //    else
        //    {
        //        CurrentGun = null;
        //        _isGrabbingGun = false;
        //    }
        //}

        //foreach(var interactor in _interactors)
        //{
        //    foreach(var item in interactor.interactablesSelected)
        //    {
        //        if(item.transform.GetComponent<ProjectileWeapon>())
        //        {
        //            _isGrabbingGun = true;

        //            if(item.transform.GetComponent<ProjectileWeapon>().gunCocked)
        //            {
        //                CurrentGun = item.transform.GetComponent<ProjectileWeapon>();
        //                _isMagazineLoaded = true;
        //            }
        //            else
        //            {
        //                _isMagazineLoaded = false;
        //            }
        //        }
        //        else
        //        {
        //            _isGrabbingGun = false;
        //            CurrentGun = null;
        //        }
        //    }
        //}

        if (_leftInteractor.interactablesSelected.Count == 0 && _rightInteractor.interactablesSelected.Count == 0)
        {
            _isGrabbingGun = false;
            _isMagazineLoaded = false;
            CurrentGun = null;
        }
        else
        {
            foreach (var item in _leftInteractor.interactablesSelected)
            {
                if (item.transform.GetComponent<ProjectileWeapon>())
                {
                    _isGrabbingGun = true;

                    if (item.transform.GetComponent<ProjectileWeapon>().gunCocked)
                    {
                        CurrentGun = item.transform.GetComponent<ProjectileWeapon>();
                        _isMagazineLoaded = true;
                    }
                    else
                    {
                        _isMagazineLoaded = false;
                    }
                }
            }
            foreach (var item in _rightInteractor.interactablesSelected)
            {
                if (item.transform.GetComponent<ProjectileWeapon>())
                {
                    _isGrabbingGun = true;

                    if (item.transform.GetComponent<ProjectileWeapon>().gunCocked)
                    {
                        CurrentGun = item.transform.GetComponent<ProjectileWeapon>();
                        _isMagazineLoaded = true;
                    }
                    else
                    {
                        _isMagazineLoaded = false;
                    }
                }
            }
        }

        //if(_vrPassthroughLayer.textureOpacity <= 0)
        //{
        //    _vrPassthroughLayer.textureOpacity = 0;
        //    _vrPassthroughLayer.enabled = false;
        //}

        //if(Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    _vrPassthroughLayer.enabled = true;

        //    if (_vrPassthroughLayer.textureOpacity < 1)
        //    {
        //        _vrPassthroughLayer.textureOpacity += 0.1f;
        //    }
        //}
        //else if (Input.GetKeyDown(KeyCode.Alpha2))
        //{
        //    if (_vrPassthroughLayer.textureOpacity > 0)
        //    {
        //        _vrPassthroughLayer.textureOpacity -= 0.1f;
        //    }
        //}
    }

    private void OnDestroy()
    {
        leftHandMoveAction.GetInputAction().performed -= PlayerPerformed;
        leftHandMoveAction.GetInputAction().canceled -= PlayerCanceled;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (_corHitEffect != null)
                StopCoroutine(_corHitEffect);

            _corHitEffect = StartCoroutine(HitEffect());
        }
    }

    IEnumerator HitEffect()
    {
        if (!_audioSourceHit.isPlaying)
            _audioSourceHit.Play();

        float time = 0f;
        float initTime = 1f;

        _hitEffect.color = _hitColor;

        while (time <= initTime)
        {
            time += Time.deltaTime;

            _hitEffect.color = Color.Lerp(_hitColor, _clearColor, time / initTime);

            yield return new WaitForEndOfFrame();
        }

        _corHitEffect = null;

        yield return null;
    }

    private void SetHitEffectDirection(Transform target)
    {
        Vector3 direction = target.position - Camera.main.transform.position;
        Vector3 crossVec = Vector3.Cross(direction, Camera.main.transform.forward);

        float angle = Vector3.Angle(direction, Camera.main.transform.forward);
        float dot = Vector3.Dot(crossVec, Vector3.up);

        //Debug.Log(angle);

        if (angle < 60)
        {
            _hitEffect.sprite = _bloodImage;
        }
        else
        {
            if (dot > 0)
            {
                _hitEffect.sprite = _bloodLeftImage;
            }
            else
            {
                _hitEffect.sprite = _bloodRightImage;
            }
        }
    }

    public override void Damage(float damage, Transform transform = null, System.Action action = null)
    {
        base.Damage(damage);

        SetHitEffectDirection(transform);

        if (_corHitEffect != null)
            StopCoroutine(_corHitEffect);

        _corHitEffect = StartCoroutine(HitEffect());

        if (hp == 0)
        {
            GameManager.Instance.PlayerDead();
        }
    }

    public float GetPlayerHp()
    {
        return hp;
    }

    public bool IsReadyToFire()
    {
        return _isGrabbingGun && _isMagazineLoaded;
    }

    private void PlayerDead()
    {
        leftHandMoveAction.GetInputAction().performed -= x => ActiveFootSound(true);
        leftHandMoveAction.GetInputAction().canceled -= x => ActiveFootSound(false);
        GameManager.Instance.PlayerDead -= PlayerDead;
        StartCoroutine(PlayerDeadCoroutine());
    }

    private IEnumerator PlayerDeadCoroutine()
    {
        float time = 0f;
        float initTime = 3f;

        Color color = _fadeEffect.color;

        while (time <= initTime)
        {
            time += Time.deltaTime;

            color.a = Mathf.Lerp(0, 1, time / initTime);
            _fadeEffect.color = color;

            yield return new WaitForEndOfFrame();
        }

        Singleton<GameManager> a = FindObjectOfType<Singleton<GameManager>>();
        Singleton<SoundManager> b = FindObjectOfType<Singleton<SoundManager>>();
        Singleton<ObjectPoolManager> c = FindObjectOfType<Singleton<ObjectPoolManager>>();
        Singleton<StageMonsterHolder> d = FindObjectOfType<Singleton<StageMonsterHolder>>();
        Singleton<MonsterSpawner> e = FindObjectOfType<Singleton<MonsterSpawner>>();

        Destroy(a.gameObject);
        Destroy(b.gameObject);
        Destroy(c.gameObject);
        Destroy(d.gameObject);
        Destroy(e.gameObject);

        if (_directInteractors.Length > 0)
        {
            foreach (var _directInteractor in _directInteractors)
            {
                XRBaseInteractable tem = _directInteractor.selectTarget;

                if (tem != null)
                    _interactionManager.SelectExit(_directInteractor, _directInteractor.selectTarget);
            }
        }

        StartCoroutine(LoadAsyncScene());
    }

    public void SetMoveSpeed(float speed)
    {
        GetComponent<DynamicMoveProvider>().moveSpeed = speed;
    }

    private void ActiveFootSound(bool isActive)
    {
        if (isActive)
        {
            if (!_audioSourceFoot.isPlaying)
                _audioSourceFoot.Play();
        }
        else
            _audioSourceFoot.Stop();
    }

    private IEnumerator LoadAsyncScene()
    {
        var asyncOperation = SceneManager.LoadSceneAsync("Game", LoadSceneMode.Single);
        asyncOperation.allowSceneActivation = false;
        while (asyncOperation.progress < 0.9f)
        {
            yield return null;
        }
        asyncOperation.allowSceneActivation = true;
    }
}
