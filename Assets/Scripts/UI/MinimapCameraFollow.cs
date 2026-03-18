using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCameraFollow : MonoBehaviour
{
    [SerializeField]
    private MinimapSettings _settings;
    [SerializeField]
    private float _cameraHeight;

    private void Awake()
    {
        _settings = GetComponentInParent<MinimapSettings>();
        _cameraHeight = transform.position.y;
    }

    private void FixedUpdate()
    {
        Vector3 targetPosition = _settings._targetToFollow.transform.position;
        transform.position = new Vector3(targetPosition.x, targetPosition.y + _cameraHeight, targetPosition.z);

        if (_settings._rotateWithTheTarget)
        {
            Quaternion targetRotation = _settings._targetToFollow.transform.rotation;

            transform.rotation = Quaternion.Euler(90, targetRotation.eulerAngles.y, 0);
        }

        if(Input.GetKeyDown(KeyCode.V))
        {
            if(GetComponent<Camera>().enabled)
                GetComponent<Camera>().enabled = false;
            else
                GetComponent<Camera>().enabled = true;
        }
    }
}