using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[Serializable]
public class ItemInfo
{
    public string Name = string.Empty;
    public int Price = 0;
    [TextArea]
    public string Info = string.Empty;

    public ItemInfo(string name, int price, string info)
    {
        Name = name;
        Price = price;
        Info = info;
    }
}

public class Purchase : MonoBehaviour
{
    public ItemInfo Item;
    private XRGrabInteractable _interactable;
    private ShopButton _shop;    
    private AudioSource _audioSource;

    protected virtual void Start()
    {
        _interactable = GetComponent<XRGrabInteractable>();
        _shop = FindObjectOfType<ShopButton>();
        _audioSource = GetComponent<AudioSource>();
        _interactable.selectEntered.AddListener(x => Grab());
        _interactable.selectExited.AddListener(x => Release());
    }

    public virtual void Buy()
    {
        if (_shop != null && !_shop.GetOpenShop())
            return;
    }

    private void Grab()
    {
        if (_shop != null && _shop.GetOpenShop())
        {
            _shop.UpdateText(Item.Info, Item.Price.ToString());
            _audioSource.Play();
        }
    }

    private void Release()
    {
        if (_shop != null && _shop.GetOpenShop())
            _shop.UpdateText();
    }
}