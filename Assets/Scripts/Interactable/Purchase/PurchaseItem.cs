using UnityEngine;

public class PurchaseItem : Purchase
{
    [SerializeField]
    private Inventory _inventory;

    protected override void Start()
    {
        base.Start();
        
        if(_inventory == null)
            _inventory = FindObjectOfType<Inventory>(true);
    }

    public override void Buy()
    {
        base.Buy();

        _inventory.ItemInSlot(this);
    }
}
