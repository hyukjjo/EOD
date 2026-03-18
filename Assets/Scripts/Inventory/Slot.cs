using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Slot : MonoBehaviour
{
    [SerializeField] public Transform Holder;

    private List<XRGrabInteractable> _inSlotItems = new List<XRGrabInteractable>();
    private TMP_Text _text;

    private int _itemCount = 0;
    public string _currentItem = string.Empty;

    private Vector3 rotation = Vector3.zero;

    private void Start()
    {
        InitText();
    }

    public void InsertItem(XRGrabInteractable item, Vector3 rotation)
    {
        InitText();

        if (string.IsNullOrEmpty(_currentItem) || _currentItem.Equals(item.name))
        {
            if (_inSlotItems.Count > 0)
                _inSlotItems[^1].gameObject.SetActive(false);

            item.GetComponentInChildren<Rigidbody>().isKinematic = true;
            item.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.Euler(rotation));
            _inSlotItems.Add(item);
            _currentItem = item.name;
            _itemCount = _inSlotItems.Count;
            Purchase purchase = item.GetComponent<Purchase>();
            _text.text = $"{purchase.Item.Name}\n{_itemCount}";
            this.rotation = rotation;
            item.gameObject.SetActive(false);
            ShowInventoryItem();
        }
    }

    public void RemoveItem(XRGrabInteractable item)
    {
        InitText();

        if (_inSlotItems.Contains(item))
        {
            _inSlotItems.Remove(item);
            if (_inSlotItems.Count == 0)
                _currentItem = string.Empty;
            _itemCount = _inSlotItems.Count;
            Purchase purchase = item.GetComponent<Purchase>();
            _text.text = $"{purchase.Item.Name}\n{_itemCount}";
            ShowInventoryItem();
        }
    }

    public void ShowInventoryItem()
    {
        if (_inSlotItems.Count > 0)
        {
            _inSlotItems[^1].gameObject.SetActive(true);
        }
        else
        {
            InitText();
            _text.text = string.Empty;
        }
    }

    private void InitText()
    {
        if (!_text)
            _text = GetComponentInChildren<TMP_Text>(true);
    }
}
