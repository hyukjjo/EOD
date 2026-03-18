using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Bettery : MonoBehaviour
{
    public bool IsGrab = false;
    private XRGrabInteractable _interactable;

    void Start()
    {
        _interactable = GetComponent<XRGrabInteractable>();
        _interactable.selectEntered.AddListener(x => Grab());
        _interactable.selectExited.AddListener(x => Release());
    }

    private void Grab()
    {
        IsGrab = true;
    }

    private void Release()
    {
        IsGrab = false;
    }
}
