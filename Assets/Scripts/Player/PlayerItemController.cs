using UnityEngine;

public class PlayerItemController : MonoBehaviour
{
    [SerializeField]
    private UnityEngine.XR.Interaction.Toolkit.XRInteractionManager _interactionManager;
    [SerializeField]
    private UnityEngine.XR.Interaction.Toolkit.XRDirectInteractor _leftInteractor;
    [SerializeField]
    private UnityEngine.XR.Interaction.Toolkit.XRDirectInteractor _rightInteractor;
    [SerializeField]
    private UnityEngine.XR.Interaction.Toolkit.XRDirectInteractor[] _xRDirectInteractors;
    [SerializeField]
    private Transform _leftCollider;
    [SerializeField]
    private Transform _rightCollider;
    [SerializeField]
    private GameObject _inventory;

    // Start is called before the first frame update
    void Start()
    {
        _leftCollider.SetParent(null);
        _rightCollider.SetParent(null);
    }

    // Update is called once per frame
    void Update()
    {
        FollowColliderPosition();
    }

    private void FollowColliderPosition()
    {
        _leftCollider.position = _leftInteractor.transform.position;
        _rightCollider.position = _rightInteractor.transform.position;
    }

    private void OnTriggerEnter(Collider coll)
    {
        if (_inventory.activeSelf)
            return;

        foreach (var _directInteractor in _xRDirectInteractors)
        {
            UnityEngine.XR.Interaction.Toolkit.XRBaseInteractable item = _directInteractor.selectTarget;

            if (item == null)
                continue;

            if (!item.GetComponent<PurchaseItem>())
                continue;

            var wire = item.GetComponent<BarbedWire>();
            if (wire)
            {
                if (wire.isActive())
                    continue;
            }
            item.GetComponent<PurchaseItem>().Buy();
            //Destroy(item.gameObject);
        }
    }
}