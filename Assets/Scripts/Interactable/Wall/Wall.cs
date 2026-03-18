using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Wall : MonoBehaviour
{
    public bool IsFill = false;

    private bool _isInside = false;
    private Material _origin;
    private Material _Silhouette;
    private MeshRenderer _renderer;
    private Collider _collider;

    private XRDirectInteractor _leftGrip;
    private XRDirectInteractor _rightGrip;

    private XRBaseInteractable _leftTarget = null;
    private XRBaseInteractable _rightTarget = null;

    private Action action;

    private void OnDestroy()
    {
        action = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Board"))
        {
            if (IsFill)
                return;

            _isInside = true;
            SetWall();
            _renderer.material = _origin;
            Debug.Log("Show Origin");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Board"))
        {
            if (IsFill)
                return;

            _isInside = false;
            SetWall();
            Debug.Log("Show Silhouette");
        }
    }

    public void Init(XRDirectInteractor left, XRDirectInteractor right, Material originMat, Material SilhouetteMat)
    {
        _renderer = GetComponent<MeshRenderer>();
        _collider = GetComponent<Collider>();

        _leftGrip = left;
        _rightGrip = right;

        _leftGrip.selectEntered.AddListener(x => GrabTarget(true, _leftGrip.selectTarget));
        _leftGrip.selectExited.AddListener(x => FillWall(true));
        _rightGrip.selectEntered.AddListener(x => GrabTarget(false, _rightGrip.selectTarget));
        _rightGrip.selectExited.AddListener(x => FillWall(false));

        _origin = new(originMat);
        _Silhouette = new(SilhouetteMat);
        SetWall();
        ShowWall(IsFill);
    }

    private void SetWall()
    {
        _collider.isTrigger = !IsFill;
        _renderer.material = IsFill ? _origin : _Silhouette;
    }

    public void ShowWall(bool isShow)
    {
        _renderer.enabled = false;
        _renderer.enabled = isShow;
    }

    public void SetHp(Action action)
    {
        this.action = action;
    }

    private void GrabTarget(bool left, XRBaseInteractable grabTarget)
    {
        if (left)
            _leftTarget = grabTarget;

        if (!left)
            _rightTarget = grabTarget;

        if (_leftTarget)
        {
            if (_leftTarget.CompareTag("Board"))
            {
                SetWall();
                ShowWall(true);
                _isInside = false;
            }
        }

        if (_rightTarget)
        {
            if (_rightTarget.CompareTag("Board"))
            {
                SetWall();
                ShowWall(true);
                _isInside = false;
            }
        }
    }

    private void FillWall(bool left)
    {
        if (IsFill)
            return;

        if (_isInside)
        {
            if (left && _leftTarget.CompareTag("Board"))
            {
                Destroy(_leftTarget.gameObject);
                _leftTarget = null;
            }
            else if (!left && _rightTarget.CompareTag("Board"))
            {
                Destroy(_rightTarget.gameObject);
                _rightTarget = null;
            }
            else
                return;

            IsFill = true;
            SetWall();
            ShowWall(true);
            action?.Invoke();
        }
        else
        {
            ShowWall(false);

            if (left)
            {
                _leftTarget = null;

                if (_rightTarget)
                {
                    if (_rightTarget.CompareTag("Board"))
                    {
                        SetWall();
                        ShowWall(true);
                    }
                }
            }
            else
            {
                _rightTarget = null;

                if (_leftTarget)
                {
                    if (_leftTarget.CompareTag("Board"))
                    {
                        SetWall();
                        ShowWall(true);
                    }
                }
            }
        }
    }

    public void SetFill(bool isFill)
    {
        this.IsFill = isFill;
        SetWall();
        ShowWall(isFill);
    }

    public void RemoveWall()
    {
        IsFill = false;
        SetWall();
    }
}
