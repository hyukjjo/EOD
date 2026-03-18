using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatchScreen : MonoBehaviour
{
    // Start is called before the first frame update
    public virtual void Start()
    {
        Init();
    }

    public virtual void Init()
    {

    }

    public virtual void TouchScreen()
    {

    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            TouchScreen();
        }
    }
}
