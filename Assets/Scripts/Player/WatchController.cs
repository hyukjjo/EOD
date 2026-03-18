using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using MikeNspired.UnityXRHandPoser;

public class WatchController : MonoBehaviour
{
    [SerializeField]
    private List<WatchScreen> _watchScreenList = new List<WatchScreen>();
    [SerializeField]
    private WatchScreen _currentScreen;
    [SerializeField]
    private InputActionReference _screenSlideInput;
    [SerializeField]
    private InputActionReference _screenTouchInput;

    private int _slideNumber = 0;

    // Start is called before the first frame update
    void Start()
    {
        _watchScreenList[_slideNumber].gameObject.SetActive(true);
        _currentScreen = _watchScreenList[_slideNumber];
        _screenSlideInput.GetInputAction().performed += WatchController_performed;
        _screenTouchInput.GetInputAction().performed += WatchController_performed1;
    }

    private void OnDestroy()
    {
        _screenSlideInput.GetInputAction().performed -= WatchController_performed;
        _screenTouchInput.GetInputAction().performed -= WatchController_performed1;
    }

    private void WatchController_performed1(InputAction.CallbackContext obj)
    {
        TouchScreen();
    }

    private void WatchController_performed(InputAction.CallbackContext obj)
    {
        SlideScreen();
    }

    private void SlideScreen()
    {
        for (int i = 0; i < _watchScreenList.Count; i++)
        {
            _watchScreenList[i].gameObject.SetActive(false);
        }
        _slideNumber++;
        if(_slideNumber > 2)
        {
            _slideNumber = 0;
        }
        _watchScreenList[_slideNumber].gameObject.SetActive(true);
        _currentScreen = _watchScreenList[_slideNumber];
    }

    private void TouchScreen()
    {
        _currentScreen.TouchScreen();
    }
}