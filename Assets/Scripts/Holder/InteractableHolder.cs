using MikeNspired.UnityXRHandPoser;
using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class InteractableHolder : MonoBehaviour
{
    [SerializeField] private XRGrabInteractable _targetObject;
    [SerializeField] private InteractableMagazineHolder _magazineHolder;

    private Coroutine cor = null;

    private ShopButton _shop;

    void Start()
    {
        _shop = FindObjectOfType<ShopButton>();        

        _targetObject.transform.SetParent(transform, true);
        _targetObject.transform.localPosition = Vector3.zero;
        _targetObject.transform.localRotation = Quaternion.identity;

        _targetObject.selectEntered.AddListener(x =>
        {
            if (cor != null)
                StopCoroutine(cor);

            Magazine mag = _targetObject.GetComponentInChildren<Magazine>();

            if (_magazineHolder)
            {
                if (_shop != null && _shop.GetOpenShop())
                    return;

                _magazineHolder.gameObject.SetActive(true);
                _magazineHolder.ShowHolderItem();
            }
        });

        _targetObject.selectExited.AddListener(x =>
        {
            cor = StartCoroutine(ReturnInteractable(_targetObject));            

            if (_magazineHolder)
            {
                _magazineHolder.gameObject.SetActive(false);
            }
        });

        if (_shop != null)
        {
            _shop.InSideEvent += () => _targetObject.gameObject.SetActive(false);
            _shop.OutSideEvent += () => _targetObject.gameObject.SetActive(true);
        }
    }

    private void OnDestroy()
    {
        if (_shop)
        {
            _shop.InSideEvent -= () => _targetObject.gameObject.SetActive(false);
            _shop.OutSideEvent -= () => _targetObject.gameObject.SetActive(true);
        }
    }

    private IEnumerator ReturnInteractable(XRBaseInteractable target)
    {
        float time = 0f;
        float initTime = 1f;

        while (time <= initTime)
        {
            time += Time.deltaTime;

            target.transform.localPosition = Vector3.Lerp(target.transform.localPosition, Vector3.zero, time / initTime);
            target.transform.localRotation = Quaternion.Lerp(target.transform.localRotation, Quaternion.identity, time / initTime);

            yield return new WaitForEndOfFrame();
        }
        
        target.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

        yield return cor = null;
    }









































    //[SerializeField] private XRBaseInteractable _targetObject;
    //[SerializeField] private InteractableMagazineHolder _magazine;

    //[SerializeField] private XRDirectInteractor _leftGrip;
    //[SerializeField] private XRDirectInteractor _rightGrip;

    //private XRBaseInteractable _leftTarget = null;
    //private XRBaseInteractable _rightTarget = null;

    //private Coroutine cor = null;

    //void Start()
    //{
    //    _targetObject.transform.SetParent(transform, true);
    //    _targetObject.transform.localPosition = Vector3.zero;
    //    _targetObject.transform.localRotation = Quaternion.identity;

    //    _leftGrip.selectEntered.AddListener(x =>
    //    {
    //        if (_targetObject != _leftGrip.selectTarget)
    //            return;

    //        if (cor != null)
    //            StopCoroutine(cor);

    //        Magazine mag = _targetObject.GetComponentInChildren<Magazine>();

    //        if (mag)
    //        {
    //            if (mag.CurrentAmmo == 0)
    //            {
    //                if (_magazine)
    //                {
    //                    _magazine.gameObject.SetActive(true);
    //                    _magazine.ShowMagazine(true);
    //                }
    //            }
    //        }
    //        else
    //        {
    //            if (_magazine)
    //            {
    //                _magazine.gameObject.SetActive(true);
    //                _magazine.ShowMagazine(true);
    //            }
    //        }

    //        _leftTarget = _leftGrip.selectTarget;
    //    });

    //    _leftGrip.selectExited.AddListener(x =>
    //    {
    //        if (!_leftTarget)
    //            return;

    //        _leftTarget.transform.SetParent(transform, true);
    //        cor = StartCoroutine(ReturnInteractable(_leftTarget));
    //        if (_magazine)
    //        {
    //            _magazine.ShowMagazine(false);
    //            _magazine.gameObject.SetActive(false);                
    //        }
    //        _leftTarget = null;
    //    });

    //    _rightGrip.selectEntered.AddListener(x =>
    //    {
    //        if (_targetObject != _rightGrip.selectTarget)
    //            return;

    //        if (cor != null)
    //            StopCoroutine(cor);

    //        Magazine mag = _targetObject.GetComponentInChildren<Magazine>();

    //        if (mag)
    //        {
    //            if (mag.CurrentAmmo == 0)
    //            {
    //                if (_magazine)
    //                {
    //                    _magazine.gameObject.SetActive(true);
    //                    _magazine.ShowMagazine(true);
    //                }
    //            }
    //        }
    //        else
    //        {
    //            if (_magazine)
    //            {
    //                _magazine.gameObject.SetActive(true);
    //                _magazine.ShowMagazine(true);
    //            }
    //        }

    //        _rightTarget = _rightGrip.selectTarget;
    //    });

    //    _rightGrip.selectExited.AddListener(x =>
    //    {
    //        if (!_rightTarget)
    //            return;

    //        _rightTarget.transform.SetParent(transform, true);
    //        cor = StartCoroutine(ReturnInteractable(_rightTarget));
    //        if (_magazine)
    //        {
    //            _magazine.ShowMagazine(false);
    //            _magazine.gameObject.SetActive(false);
    //        }
    //        _rightTarget = null;
    //    });
    //}

    //private IEnumerator ReturnInteractable(XRBaseInteractable target)
    //{
    //    float time = 0f;
    //    float initTime = 1f;

    //    while (time <= initTime)
    //    {
    //        time += Time.deltaTime;

    //        target.transform.localPosition = Vector3.Lerp(target.transform.localPosition, Vector3.zero, time / initTime);
    //        target.transform.localRotation = Quaternion.Lerp(target.transform.localRotation, Quaternion.identity, time / initTime);

    //        yield return new WaitForEndOfFrame();
    //    }

    //    yield return cor = null;
    //}
}