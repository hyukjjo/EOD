using UnityEngine;

public class CameraMarking : MonoBehaviour
{
    [SerializeField] private Vector3 _offset = Vector3.zero;

    private void Update()
    {
        Vector3 dir = Camera.main.transform.forward;
        transform.forward = Vector3.ProjectOnPlane(dir, transform.up);
        transform.position = Camera.main.transform.position + _offset;
    }
}
