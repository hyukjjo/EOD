using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class UseInventory : MonoBehaviour
{
    private XRGrabInteractable _interactable;
    private Inventory inventory = null;
    private Rigidbody rb = null;

    [SerializeField] private Vector3 _inventoryRotation = Vector3.zero;

    private Slot _currentSlot = null;
    public bool IsInventoryItem = false;

    private void Start()
    {
        _interactable = GetComponent<XRGrabInteractable>();
        inventory = FindObjectOfType<Inventory>(true);
        rb = GetComponentInChildren<Rigidbody>(true);
        _interactable.selectEntered.AddListener(x => Grab());
        _interactable.selectExited.AddListener(x => Release());
    }

    private void OnTriggerEnter(Collider other)
    {
        Slot slot = other.GetComponentInParent<Slot>();

        if(slot && _interactable)
        {
             if(_interactable.isSelected)
                _currentSlot = slot;
        }   
    }

    private void OnTriggerStay(Collider other)
    {
        Slot slot = other.GetComponentInParent<Slot>();

        if (slot && _interactable)
        {
            if (_interactable.isSelected)
                _currentSlot = slot;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Slot slot = other.GetComponentInParent<Slot>();

        if (slot && _interactable)
        {
            if (_interactable.isSelected)
                _currentSlot = null;
        }
    }

    private void Grab()
    {
        rb.isKinematic = true;
        if (!inventory.InventorySlots.activeSelf)
        {
            _currentSlot = null;
            return;
        }

        if (_currentSlot && IsInventoryItem)
        {
            IsInventoryItem = false;
            _currentSlot.RemoveItem(_interactable);
        }
    }

    private void Release()
    {
        if (!inventory.InventorySlots.activeSelf)
        {
            _currentSlot = null;
            rb.isKinematic = false;
            return;
        }

        if (_currentSlot)
        {
            IsInventoryItem = true;
            gameObject.transform.SetParent(_currentSlot.Holder);
            _currentSlot.InsertItem(_interactable, _inventoryRotation);
        }
        else
        {
            rb.isKinematic = false;
            gameObject.transform.SetParent(null);
        }
    }

    public Vector3 GetInventoryRotation()
    {
        return _inventoryRotation;
    }

    public void BuyInsertItem(Slot slot)
    {
        IsInventoryItem = true;
        _currentSlot = slot;
    }
}
