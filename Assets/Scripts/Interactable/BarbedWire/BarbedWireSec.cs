using System.Net;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BarbedWireSec : MonoBehaviour
{
    [Header("SecondHand")]
    [SerializeField] private Transform main;
    [SerializeField] private XRBaseInteractable subXrGrabInteractable = null;

    private XRBaseInteractor xrGrabInteractor = null;
    private Vector3 originPos = Vector3.zero;
    private float originScale = 0f;
    private Vector3 handPos = Vector3.zero;
    private Vector3 LocalAxis = Vector3.right;
    private float AxisLength = .3f;
    private Vector3 grabbedOffset, endPoint, startPoint;
    private bool isGrab = false;

    void Start()
    {
        originPos = subXrGrabInteractable.transform.localPosition;
        originScale = main.localScale.x;
        subXrGrabInteractable.selectEntered.AddListener(OnGrabbed);
        subXrGrabInteractable.selectExited.AddListener(OnRelease);

        startPoint = subXrGrabInteractable.transform.localPosition;
        endPoint = subXrGrabInteractable.transform.localPosition + LocalAxis * AxisLength;
    }

    private void FixedUpdate()
    {
        if (isGrab)
        {
            Vector3 worldAxis = subXrGrabInteractable.transform.TransformDirection(LocalAxis);
            Vector3 distance = xrGrabInteractor.transform.position - subXrGrabInteractable.transform.position - grabbedOffset;
            float projected = Vector3.Dot(distance, worldAxis);
            float scale = Vector3.Distance(handPos, xrGrabInteractor.transform.position)/* * 0.003f*/;

            Vector3 targetPoint;

            if (projected > 0)
            {
                if (main.localScale.x > originScale * 15f)
                    return;

                main.localScale += new Vector3(scale, 0f, 0f);
                targetPoint = Vector3.MoveTowards(subXrGrabInteractable.transform.localPosition, endPoint, projected);
            }
            else
            {
                if (main.localScale.x < originScale)
                    return;

                main.localScale -= new Vector3(scale, 0f, 0f);
                targetPoint = Vector3.MoveTowards(subXrGrabInteractable.transform.localPosition, startPoint, -projected);
            }

            Vector3 move = targetPoint - transform.localPosition;
            subXrGrabInteractable.transform.localPosition += move;
        }
    }

    private void OnGrabbed(SelectEnterEventArgs arg0)
    {
        isGrab = true;
        xrGrabInteractor = subXrGrabInteractable.selectingInteractor;
        handPos = xrGrabInteractor.transform.position;
        grabbedOffset = xrGrabInteractor.transform.position - subXrGrabInteractable.transform.position;
    }

    private void OnRelease(SelectExitEventArgs arg0)
    {
        isGrab = false;
        xrGrabInteractor = null;
        subXrGrabInteractable.transform.localPosition = originPos;
    }
}
