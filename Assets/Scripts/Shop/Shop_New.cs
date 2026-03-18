using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop_New : MonoBehaviour
{
    [SerializeField]
    private GameObject _round1Prefab;
    [SerializeField]
    private GameObject _round2Prefab;
    [SerializeField]
    private GameObject _round3Prefab;
    [SerializeField]
    private GameObject _shopLight;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.MaintenanceStart += ShopOn;
        GameManager.Instance.RoundStart += ShopOff;

        _round1Prefab.SetActive(false);
        _round2Prefab.SetActive(false);
        _round3Prefab.SetActive(false);
    }

    private void ShopOn()
    {
        int round = GameManager.Instance.GetRound();

        switch(round)
        {
            case 1:
                _round1Prefab.SetActive(true);
                break;
            case 2:
                _round2Prefab.SetActive(true);
                break;
            case 3:
                _round3Prefab.SetActive(true);
                break;
            default:
                break;
        }
        _shopLight.SetActive(true);
    }

    private void ShopOff()
    {
        _round1Prefab.SetActive(false);
        _round2Prefab.SetActive(false);
        _round3Prefab.SetActive(false);
        _shopLight.SetActive(false);
    }
}