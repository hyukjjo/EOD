using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PurchaseMagazine : Purchase
{
    [SerializeField] private string _holderName = string.Empty;
    private InteractableMagazineHolder _holder;

    protected override void Start()
    {
        base.Start();

        var objects = FindObjectsOfType<InteractableMagazineHolder>(true);
        foreach (var obj in objects)
        {
            if (obj.name == _holderName)
                _holder = obj;
        }
    }

    public override void Buy()
    {
        base.Buy();

        _holder.InsertItem(GetComponent<XRGrabInteractable>());
    }
}
