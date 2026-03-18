using MikeNspired.UnityXRHandPoser;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class UseHolderMagazine : MonoBehaviour
{
    private XRGrabInteractable _interactable;
    private InteractableMagazineHolder _holder;

    [SerializeField] private string _holderName = string.Empty;
    [SerializeField] private string _targetGunName = string.Empty;

    private bool _isInHolder = false;

    private void Start()
    {
        _interactable = GetComponent<XRGrabInteractable>();
        var objects = FindObjectsOfType<InteractableMagazineHolder>(true);
        foreach (var obj in objects)
        {
            if (obj.name == _holderName)
                _holder = obj;
        }

        _interactable.selectEntered.AddListener(x => Grab());
        _interactable.selectExited.AddListener(x => Release());
    }

    private void OnTriggerEnter(Collider other)
    {
        InteractableMagazineHolder holder = other.GetComponent<InteractableMagazineHolder>();

        if (holder)
        {
            if (_interactable.isSelected)
                _isInHolder = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        InteractableMagazineHolder holder = other.GetComponent<InteractableMagazineHolder>();

        if (holder)
        {
            if (_interactable.isSelected)
                _isInHolder = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        InteractableMagazineHolder holder = other.GetComponent<InteractableMagazineHolder>();

        if (holder)
        {
            //if(_isInHolder && _interactable.isSelected)
            //    _holder.RemoveItem(_interactable);

            if (_interactable.isSelected)
                _isInHolder = false;
        }
    }

    private void Grab()
    {
        if (_holder.GetOpenShop())
            return;

        _holder.gameObject.SetActive(true);

        //if (_isInHolder)
        //{
            _holder.RemoveItem(_interactable);
        //}
    }

    private void Release()
    {
        if (_holder.GetOpenShop())
            return;

        if (_interactable.GetComponent<Magazine>().CurrentAmmo == 0)
            return;

        var hands = FindObjectsOfType<XRDirectInteractor>();

        bool activeGun = false;

        foreach (var hand in hands)
        {
            if (hand.selectTarget)
            {
                if (hand.selectTarget.name.Equals(_targetGunName))
                    activeGun = true;
            }
        }

        if(activeGun)
            _holder.gameObject.SetActive(true);
        else
            _holder.gameObject.SetActive(false);

        //if (_isInHolder)
        //{
        //    _holder.InsertItem(_interactable);
        //}
    }
}
