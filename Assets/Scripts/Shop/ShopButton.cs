using System;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ShopButton : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _text;
    [SerializeField]
    private GameObject _shopArea;
    [SerializeField]
    private GameObject _fieldLights;

    public Action InSideEvent;
    public Action OutSideEvent;

    private GameManager _gameManager;
    private XRInteractionManager _interactionManager;
    private XRDirectInteractor[] _directInteractors;

    void Start()
    {
        _gameManager = GameManager.Instance;
        _gameManager.RoundStart += ForceExit;
        _gameManager.InfiniteModeStart += ForceExit;
        _interactionManager = FindObjectOfType<XRInteractionManager>();
        _directInteractors = FindObjectsOfType<XRDirectInteractor>();
    }

    private void OnDestroy()
    {
        InSideEvent = null;
        OutSideEvent = null;
    }

    public void PressShopButton()
    {
        if (!_gameManager.GetState().Equals(GameState.MAINTENANCE))
        {
            return;
        }

        _shopArea.SetActive(!_shopArea.activeSelf);
        _fieldLights.SetActive(!_fieldLights.activeSelf);

        if (_shopArea.activeSelf)
        {
            if (_directInteractors.Length > 0)
            {
                foreach (var _directInteractor in _directInteractors)
                {
                    XRBaseInteractable tem = _directInteractor.selectTarget;

                    if (tem != null)
                        _interactionManager.SelectExit(_directInteractor, _directInteractor.selectTarget);
                }
            }

            InSideEvent?.Invoke();
            UpdateText();
        }
        else
        {
            OutSideEvent?.Invoke();
            _text.text = "SHOP";
        }
    }

    public void UpdateText(string itemInfo = "", string price = "")
    {
        if (string.IsNullOrEmpty(itemInfo) && string.IsNullOrEmpty(price))
            _text.text = $"Gold = {_gameManager.GetPlayer().PlayerGold}";
        else if (string.IsNullOrEmpty(price))
            _text.text = $"Gold = {_gameManager.GetPlayer().PlayerGold}\n\n{itemInfo}";
        else
            _text.text = $"Gold = {_gameManager.GetPlayer().PlayerGold}\nTotalItemGold = {price}\n\n{itemInfo}";
    }

    public bool GetOpenShop()
    {
        return _shopArea.activeSelf;
    }

    private void ForceExit()
    {
        _shopArea.SetActive(false);
        _fieldLights.SetActive(true);
        OutSideEvent?.Invoke();
    }
}