using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class InteractableMagazineHolder : MonoBehaviour
{
    public XRGrabInteractable Prefab;
    public bool InfinityAmmo = false;
    public int StartCount = 0;
    private List<XRGrabInteractable> _inMagazines = new List<XRGrabInteractable>();
    private string _currentItem = string.Empty;
    private ShopButton _shop;

    private void Start()
    {
        _shop = FindObjectOfType<ShopButton>();

        if (Prefab)
        {
            if (InfinityAmmo)
            {
                CreateAmmo();
                gameObject.SetActive(false);
                return;
            }

            XRGrabInteractable item;
            for (int i = 0; i < StartCount; i++)
            {
                item = Instantiate(Prefab.gameObject).GetComponent<XRGrabInteractable>();
                item.name = Prefab.name;
                InsertItem(item);
                item.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            }
        }

        gameObject.SetActive(false);
    }

    private void Update()
    {
        //if (gameObject.activeSelf)
        //{
        //    foreach (var item in _inMagazines)
        //    {
        //        if(item.gameObject.activeSelf && !item.isSelected)
        //            item.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        //    }
        //}
    }

    public void InsertItem(XRGrabInteractable item)
    {
        if (string.IsNullOrEmpty(_currentItem) || _currentItem.Equals(item.name))
        {
            if (_inMagazines.Count > 0)
                _inMagazines[^1].gameObject.SetActive(false);

            item.GetComponentInChildren<Rigidbody>().isKinematic = true;
            item.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            _inMagazines.Add(item);
            item.transform.SetParent(transform);
            _currentItem = item.name;
            item.gameObject.SetActive(false);
            ShowHolderItem();
        }
    }

    public void RemoveItem(XRGrabInteractable item)
    {
        if (_inMagazines.Contains(item))
        {
            item.transform.SetParent(null);
            _inMagazines.Remove(item);
            if (_inMagazines.Count == 0)
                _currentItem = string.Empty;

            ShowHolderItem();
        }
    }

    public void ShowHolderItem()
    {
        if (_shop != null && _shop.GetOpenShop())
            return;

        if (_inMagazines.Count == 0 && InfinityAmmo)
            CreateAmmo();

        if (_inMagazines.Count > 0)
        {
            _inMagazines[^1].gameObject.SetActive(true);
            _inMagazines[^1].GetComponentInChildren<Rigidbody>().isKinematic = true;
            _inMagazines[^1].transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        }
    }

    public bool GetOpenShop()
    {
        return _shop != null && _shop.GetOpenShop();
    }


    private void CreateAmmo()
    {
        XRGrabInteractable item;
        item = Instantiate(Prefab.gameObject).GetComponent<XRGrabInteractable>();
        item.name = Prefab.name;
        InsertItem(item);
    }






}

//using MikeNspired.UnityXRHandPoser;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.XR.Interaction.Toolkit;

//public class InteractableMagazineHolder : MonoBehaviour
//{
//    [SerializeField] private XRDirectInteractor _leftGrip;
//    [SerializeField] private XRDirectInteractor _rightGrip;

//    private XRBaseInteractable _leftTarget = null;
//    private XRBaseInteractable _rightTarget = null;

//    private List<Magazine> _handGunMagazines = new List<Magazine>();

//    private bool _isInside = false;
//    private bool _isShow = false;
//    private MeshRenderer _meshRenderer;

//    void Start()
//    {
//        Init();
//    }

//    private void OnTriggerEnter(Collider other)
//    {
//        Magazine mag = other.GetComponentInParent<Magazine>();

//        if (mag)
//            _handGunMagazines.Add(mag);
//    }

//    private void OnTriggerExit(Collider other)
//    {
//        Magazine mag = other.GetComponentInParent<Magazine>();

//        if (mag && _handGunMagazines.Contains(mag))
//            _handGunMagazines.Remove(mag);
//    }

//    private void Init()
//    {
//        _meshRenderer = GetComponent<MeshRenderer>();
//        _meshRenderer.enabled = false;
//    }

//    public void ShowMagazine(bool isShow)
//    {
//        _isShow = isShow;
//        ShowSilhouette(true);

//        if (_handGunMagazines.Count > 0)
//        {
//            _handGunMagazines[0].GetComponent<Rigidbody>().isKinematic = true;
//            _handGunMagazines[0].gameObject.SetActive(isShow);            
//        }
//    }

//    private void ShowSilhouette(bool isShow)
//    {
//        _meshRenderer.enabled = isShow;
//    }

//    public List<Magazine> GetMagazineList() =>  _handGunMagazines;
//}