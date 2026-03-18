using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ShopBasket : MonoBehaviour
{
    private List<Purchase> _purchases = new List<Purchase>();
    private ShopButton _shop;
    private int _totalPrice = 0;

    private AudioSource _audioSource;
    [SerializeField] private AudioClip _clipAddProduct;
    [SerializeField] private AudioClip _clipBuy;

    void Start()
    {
        _shop = FindObjectOfType<ShopButton>();
        _audioSource = GetComponent<AudioSource>();
        _shop.OutSideEvent += () => AllBuy();
    }

    private void OnDestroy()
    {
        _shop.OutSideEvent -= () => AllBuy();
    }

    private void OnTriggerEnter(Collider other)
    {
        Purchase item = other.GetComponentInParent<Purchase>();

        if (item && !_purchases.Contains(item))
        {
            _purchases.Add(item);
            _totalPrice += item.Item.Price;
            _shop.UpdateText(item.Item.Info, _totalPrice.ToString());
            _audioSource.PlayOneShot(_clipAddProduct);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Purchase item = other.GetComponentInParent<Purchase>();

        if (item && _purchases.Contains(item))
        {
            _purchases.Remove(item);
            _totalPrice -= item.Item.Price;
            _shop.UpdateText(item.Item.Info, _totalPrice.ToString());
        }
    }

    private void AllBuy()
    {
        if (_totalPrice == 0)
            return;

        if (_totalPrice > GameManager.Instance.GetPlayer().PlayerGold)
        {
            Purchase purchase = _purchases[_purchases.Count - 1];
            _totalPrice -= purchase.Item.Price;
            _purchases.Remove(purchase);
            Destroy(purchase.gameObject);

            AllBuy();
            return;
            //FailedBuy();
        }
        
        SuccessfulBuy();

        GameManager.Instance.GetPlayer().PlayerGold -= _totalPrice;
        _shop.UpdateText();
        _totalPrice = 0;

        foreach (Purchase purchase in _purchases)
        {
            if (purchase != null)
                purchase.GetComponent<XRGrabInteractable>().retainTransformParent = false;
        }

        _purchases.Clear();
    }

    private void SuccessfulBuy()
    {
        foreach (Purchase purchase in _purchases)
        {
            if (purchase != null)
                purchase.Buy();
        }
        _audioSource.PlayOneShot(_clipBuy);
    }

    private void FailedBuy()
    {
        foreach (Purchase purchase in _purchases)
        {
            Destroy(purchase.gameObject);
        }
    }

    public List<XRGrabInteractable> GetPurchasesItem()
    {
        List<XRGrabInteractable> list = new List<XRGrabInteractable>();

        foreach (Purchase purchase in _purchases)
        {
            XRGrabInteractable tmp = null;

            if (purchase)
                tmp = purchase.GetComponent<XRGrabInteractable>();

            if(tmp)
                list.Add(tmp);
        }

        return list;
    }
}
