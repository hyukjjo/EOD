using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BarbedWire : MonoBehaviour
{
    [SerializeField] private float _hp = 0;
    [SerializeField] private float _originHp = 100;
    [SerializeField] private float _damagePerSec = 0f;

    [SerializeField] private GameObject _modelInHand;
    [SerializeField] private GameObject _modelBulid;
    [SerializeField] private GameObject _DummyCombineBettery;
    private Rigidbody _body;

    private XRGrabInteractable _interactable;

    private Ground ground;
    [SerializeField] LayerMask goundLayer;
    private bool _isActive = false;

    private List<Monster> _monsters = new List<Monster>();
    private float _tick = 1f;

    private bool _isCombine = false;

    [SerializeField] private AudioSource _audioSourceInside;
    private bool isInsidePlayer = false;

    private void Start()
    {
        _hp = _originHp;
        _body = GetComponent<Rigidbody>();
        _interactable = GetComponent<XRGrabInteractable>();
        //_interactable.selectEntered.AddListener(x => Grab());
        _interactable.selectExited.AddListener(x => Release());
        _interactable.onActivate.AddListener(x => Active());

        _modelBulid.SetActive(false);
        _DummyCombineBettery.SetActive(false);

        ground = FindObjectOfType<Ground>();
    }

    private void Update()
    {
        if (_interactable.isSelected && _isActive)
        {
            Transform hand = _interactable.selectingInteractor.GetComponentInParent<ActionBasedController>().transform;

            _modelInHand.gameObject.SetActive(false);
            _modelBulid.gameObject.SetActive(true);

            _modelBulid.transform.position = hand.position + hand.forward * 3f;
            _modelBulid.transform.localRotation = Quaternion.Euler(0f, hand.rotation.y, 0f);

            ground.SetInteraction(GroundState.InteractionFail);

            if (Physics.Raycast(hand.position, hand.forward, out RaycastHit hit, 3f, goundLayer))
            {
                if (hit.transform == ground.transform)
                {
                    ground.SetInteraction(GroundState.Interaction);

                    _modelInHand.gameObject.SetActive(false);
                    _modelBulid.gameObject.SetActive(true);

                    Collider col = _modelBulid.GetComponent<Collider>();
                    Vector3 _groundPoint = hit.point + hit.transform.up * (col.bounds.size.y * 0.5f);
                    _modelBulid.transform.position = _groundPoint;
                    _modelBulid.transform.localRotation = Quaternion.Euler(0f, hand.rotation.y, 0f);
                }
            }
        }

        if (_monsters.Count > 0)
        {
            foreach (var monster in _monsters)
            {
                if (monster.isDying)
                {
                    _monsters.Remove(monster);
                    break;
                }
            }

            _tick -= Time.deltaTime;

            if (_tick < 0)
            {
                _hp -= _monsters.Count * 2;
                _tick = 1f;

                if (_DummyCombineBettery.activeSelf)
                {
                    foreach (var monster in _monsters)
                    {
                        if(!monster.isDying)
                            monster.HitByPlayer(_damagePerSec, ShotType.BODYSHOT);
                    }
                }
            }
        }
        else if (_monsters.Count == 0)
        {
            if (_audioSourceInside.isPlaying)
                _audioSourceInside.Stop();
        }

        if (_hp < 0)
        {
            foreach (var monster in _monsters)
            {
                monster.SetSlow(false);
            }

            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_isActive)
            return;

        if (other.CompareTag("Player"))
        {
            isInsidePlayer = true;
            if (!_audioSourceInside.isPlaying)
                _audioSourceInside.Play();
        }

        //몬스터 체크
        Monster monster = other.GetComponentInParent<Monster>();

        if (monster && !_monsters.Contains(monster))
        {
            if (monster.name.Contains("Boss"))
                return;

            _monsters.Add(monster);
            if(!_audioSourceInside.isPlaying)
                _audioSourceInside.Play();
        }

        //조합용 아이템 체크
        if (!_isCombine)
        {
            Bettery bettery = other.GetComponent<Bettery>();

            if (bettery)
            {
                InventoryItem betteryinventory = other.GetComponent<InventoryItem>();

                if (bettery.IsGrab && !betteryinventory.IsInventoryItem)
                {
                    _DummyCombineBettery.SetActive(true);
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!_isActive)
            return;

        //몬스터 체크
        Monster monster = other.GetComponentInParent<Monster>();

        if (monster)
        {
            if (monster.isDying)
            {
                _monsters.Remove(monster);

                if(_monsters.Count == 0 || !isInsidePlayer)
                {
                    if (_audioSourceInside.isPlaying)
                        _audioSourceInside.Stop();
                }
            }
            else if(!monster.name.Contains("Boss"))
                monster.SetSlow(true);
        }

        //조합용 아이템 체크
        if (!_isCombine)
        {
            Bettery bettery = other.GetComponent<Bettery>();

            if (bettery)
            {
                InventoryItem betteryinventory = other.GetComponent<InventoryItem>();

                if (!bettery.IsGrab && !betteryinventory.IsInventoryItem)
                {
                    _isCombine = true;
                    _DummyCombineBettery.SetActive(true);
                    Destroy(bettery.gameObject);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!_isActive)
            return;

        if (other.CompareTag("Player"))
        {
            isInsidePlayer = false;
            if (_monsters.Count == 0 || !isInsidePlayer)
            {
                if (_audioSourceInside.isPlaying)
                    _audioSourceInside.Stop();
            }
        }

        //몬스터 체크
        Monster monster = other.GetComponentInParent<Monster>();

        if (monster)
        {
            monster.SetSlow(false);

            if (_monsters.Contains(monster))
            {
                _monsters.Remove(monster);

                if (_monsters.Count == 0 || !isInsidePlayer)
                {
                    if (_audioSourceInside.isPlaying)
                        _audioSourceInside.Stop();
                }
            }
        }

        //조합용 아이템 체크
        if (!_isCombine)
        {
            Bettery bettery = other.GetComponent<Bettery>();

            if (bettery)
            {
                InventoryItem betteryinventory = other.GetComponent<InventoryItem>();

                if (bettery.IsGrab && !betteryinventory.IsInventoryItem)
                    _DummyCombineBettery.SetActive(false);
            }
        }
    }

    private void Release()
    {
        if (_isActive)
        {
            _interactable.enabled = false;
            GetComponent<Rigidbody>().isKinematic = false;
            if(transform.parent != null)
                transform.parent = null;

            if (ground.GetState() == GroundState.Interaction)
                StartCoroutine(ReleaseWire(0.1f));
            else if(ground.GetState() == GroundState.InteractionFail)
            {
                _modelBulid.GetComponent<Collider>().isTrigger = false;
                StartCoroutine(ReleaseWire(2f));
            }

            ground.SetInteraction(GroundState.Default);
        }
    }

    private void Active()
    {
        if (GetComponent<InventoryItem>())
        {
            _isActive = true;
            _modelBulid.GetComponent<Collider>().isTrigger = true;            
            Destroy(GetComponent<InventoryItem>());
        }
    }

    private IEnumerator ReleaseWire(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        _modelBulid.GetComponent<Collider>().isTrigger = true;
        _body.constraints = RigidbodyConstraints.FreezeAll;
        yield return null;
    }

    public bool isActive()
    {
        return _isActive;
    }
}