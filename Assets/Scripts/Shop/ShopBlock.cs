using MikeNspired.UnityXRHandPoser;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ShopBlock : MonoBehaviour
{
    private List<XRGrabInteractable> _interactables = new List<XRGrabInteractable>();
    private ShopButton _shop;
    private ShopBasket _basket;
    private ObjectSpawner[] _objectSpawners;
    private BetterySpawner _betterySpawner;

    private List<XRGrabInteractable> _purchases = new List<XRGrabInteractable>();

    private void Start()
    {
        _shop = FindObjectOfType<ShopButton>();
        _basket = FindObjectOfType<ShopBasket>();
        _objectSpawners = GetComponentsInChildren<ObjectSpawner>();
        _betterySpawner = GetComponentInChildren<BetterySpawner>();
        _shop.InSideEvent += () =>
        {
            foreach (ObjectSpawner spawner in _objectSpawners)
            {
                spawner.hitDetect = false;
            }

            _betterySpawner.hitDetect = false;
        };
        _shop.OutSideEvent += () =>
        {
            GetComponentsInChildren(_interactables);
            _purchases = _basket.GetPurchasesItem();

            List<XRGrabInteractable> result = Enumerable.ToList(Enumerable.Except(_interactables, _purchases));

            foreach (var item in result)
            {
                Destroy(item.gameObject);
            }
            _interactables.Clear();
            _purchases.Clear();
            result.Clear();
        };
    }

    private void OnDestroy()
    {
        _shop.InSideEvent -= () =>
        {
            foreach (ObjectSpawner spawner in _objectSpawners)
            {
                spawner.hitDetect = false;
            }

            _betterySpawner.hitDetect = false;
        };
        _shop.OutSideEvent -= () =>
        {
            GetComponentsInChildren(_interactables);
            _purchases = _basket.GetPurchasesItem();

            List<XRGrabInteractable> result = Enumerable.ToList(Enumerable.Except(_interactables, _purchases));

            foreach (var item in result)
            {
                Destroy(item.gameObject);
            }
            _interactables.Clear();
            _purchases.Clear();
            result.Clear();
        };
    }
}
