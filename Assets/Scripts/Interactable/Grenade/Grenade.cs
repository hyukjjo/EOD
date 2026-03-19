using MikeNspired.UnityXRHandPoser;
using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Grenade : MonoBehaviour, IDamageable
{
    [SerializeField] private XRGrabInteractable interactable = null;
    [SerializeField] private GameObject Explosion = null;
    [SerializeField] private AudioSource activationSound = null;
    [SerializeField] private GameObject meshLightActivation = null;
    [SerializeField] private float detonationTime = 3;
    //Proto
    [SerializeField] protected bool startTimerAfterActivation = false;

    //Proto
    protected bool canActivate;
    private XRInteractionManager interactionManager;

    // Start is called before the first frame update
    void Start()
    {
        OnValidate();
        interactable = GetComponent<XRGrabInteractable>();
        interactable.onActivate.AddListener(TurnOnGrenade);
        interactable.onSelectExited.AddListener(Activate);
        if (meshLightActivation)
            meshLightActivation.SetActive(false);
    }

    private void OnValidate()
    {
        if (!interactable)
            interactable = GetComponent<XRGrabInteractable>();
        if (!interactionManager)
            interactionManager = FindObjectOfType<XRInteractionManager>();
    }
    //Proto
    protected virtual void TurnOnGrenade(XRBaseInteractor interactor)
    {
        canActivate = true;
        meshLightActivation.SetActive(true);
        activationSound.Play();
        Destroy(GetComponent<InventoryItem>());

        if (startTimerAfterActivation)
            Invoke(nameof(TriggerGrenade), detonationTime);
    }

    //Proto
    private void Activate(XRBaseInteractor interactor)
    {
        if (canActivate && !startTimerAfterActivation)
        {
            GetComponent<Rigidbody>().isKinematic = false;
            Destroy(GetComponent<InventoryItem>());
            Invoke(nameof(TriggerGrenade), detonationTime);
        }

        if (canActivate)
            GetComponent<Rigidbody>().isKinematic = false;
    }

    //Proto
    protected virtual void TriggerGrenade()
    {
        Explosion.SetActive(true);
        Explosion.transform.parent = null;
        Explosion.transform.localEulerAngles = Vector3.zero;

        if (interactable.selectingInteractor)
            interactionManager.SelectExit(interactable.selectingInteractor, interactable);

        StartCoroutine(MoveAndDisableCollider());
        //gameObject.SetActive(false);
        // Destroy(gameObject,1);
    }

    private IEnumerator MoveAndDisableCollider()
    {
        //objectToMove.GetComponent<CollidersSetToTrigger>()?.SetAllToTrigger();

        transform.position += Vector3.one * 9999;
        yield return new WaitForSeconds(Time.fixedDeltaTime * 2);
        //Lets physics respond to collider disappearing before disabling object physics update needs to run twice
        Destroy(gameObject);
    }

    public void TakeDamage(float damage, GameObject damager)
    {
        //Proto
        //TriggerGrenade();
    }
}
