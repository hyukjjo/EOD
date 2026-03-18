//using System.Collections;
//using System.Collections.Generic;
//using Unity.XR.CoreUtils;
//using UnityEngine;
//using UnityEngine.XR.Interaction.Toolkit;

//public class Board : MonoBehaviour
//{
//    private XRGrabInteractable _interactable;
//    private ActionBasedController _currentHand = null;

//    private void Start()
//    {
//        _interactable = GetComponent<XRGrabInteractable>();
//        _interactable.selectEntered.AddListener(x => Grab(_interactable.selectingInteractor));
//        _interactable.selectExited.AddListener(x => Release());
//    }

//    private void OnTriggerEnter(Collider other)
//    {
//        Wall wall = other.GetComponent<Wall>();

//        if (wall)
//        {
//            if (wall.IsFill)
//                return;

//            wall.ShowWall(true);
//        }
//    }

//    private void OnTriggerStay(Collider other)
//    {
//        Wall wall = other.GetComponent<Wall>();

//        if (wall)
//        {
//            if (wall.IsFill)
//                return;

//            wall.ShowWall(true);
//        }
//    }

//    private void OnTriggerExit(Collider other)
//    {
//        Wall wall = other.GetComponent<Wall>();

//        if (wall)
//        {
//            if (wall.IsFill)
//                return;

//            wall.set
//        }
//    }

//    private void Grab(XRBaseInteractor interactor)
//    {
//        _currentHand = interactor.GetComponentInParent<ActionBasedController>();
//    }

//    private void Release()
//    {
//        _currentHand = null;
//    }
//}
