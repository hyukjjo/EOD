public class PurchaseItem : Purchase
{
    private Inventory inventory;

    protected override void Start()
    {
        base.Start();

        inventory = FindObjectOfType<Inventory>(true);
    }

    public override void Buy()
    {
        base.Buy();

        inventory.ItemInSlot(this);
    }
}
