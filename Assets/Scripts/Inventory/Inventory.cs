using MikeNspired.UnityXRHandPoser;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class Inventory : MonoBehaviour
{
    [SerializeField] private InputActionReference _openMenuInputLeftHand, _openMenuInputRightHand;
    public GameObject InventorySlots;
    [SerializeField] private List<Slot> _slots = new List<Slot>();
    private ShopButton _shop;

    private AudioSource _audioSource;
    [SerializeField] private AudioClip _clipOn;
    [SerializeField] private AudioClip _clipOff;

    private void Start()
    {
        _shop = FindObjectOfType<ShopButton>();
        _audioSource = GetComponent<AudioSource>();
        GetComponentsInChildren(_slots);
        InventorySlots.SetActive(false);
        _openMenuInputRightHand.GetInputAction().performed += Inventory_performed;
    }

    private void Inventory_performed(InputAction.CallbackContext obj)
    {
        ShowInventory(!InventorySlots.activeSelf);
    }

    private void OnDestroy()
    {
        _openMenuInputRightHand.GetInputAction().performed -= Inventory_performed;
    }

    private void ShowInventory(bool isShow)
    {
        if (_shop != null && _shop.GetOpenShop())
        {
            InventorySlots.SetActive(false);
            return;
        }

        Vector3 dir = Camera.main.transform.forward;
        transform.forward = Vector3.ProjectOnPlane(dir, transform.up);
        transform.position = Camera.main.transform.position;
        InventorySlots.SetActive(isShow);
        foreach (var slot in _slots)
        {
            slot.ShowInventoryItem();
        }
        _audioSource.PlayOneShot(isShow ? _clipOn : _clipOff);
    }

    public void ItemInSlot(Purchase item)
    {
        Slot slot = AvailableSlot(item.name);

        if (slot)
        {
            item.transform.SetParent(slot.Holder);
            item.GetComponent<UseInventory>().BuyInsertItem(slot);
            slot.InsertItem(item.GetComponent<XRGrabInteractable>(), item.GetComponent<UseInventory>().GetInventoryRotation());
        }
    }

    public Slot AvailableSlot(string item)
    {
        foreach (var slot in _slots)
        {
            if(item == slot._currentItem)
                return slot;
        }

        foreach (var slot in _slots)
        {
            if (string.IsNullOrEmpty(slot._currentItem))
                return slot;
        }

        return null;
    }
}
