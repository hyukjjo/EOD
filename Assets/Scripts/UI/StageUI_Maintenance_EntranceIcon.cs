using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageUI_Maintenance_EntranceIcon : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _door1Icons;
    [SerializeField]
    private GameObject[] _door2Icons;
    [SerializeField]
    private GameObject[] _door3Icons;

    public void SetActiveIcons()
    {
        if (GameManager.Instance._door1Active)
        {
            _door1Icons[0].SetActive(true);
            _door1Icons[1].SetActive(false);
        }
        else
        {
            _door1Icons[1].SetActive(true);
            _door1Icons[0].SetActive(false);
        }
        if (GameManager.Instance._door2Active)
        {
            _door2Icons[0].SetActive(true);
            _door2Icons[1].SetActive(false);
        }
        else
        {
            _door2Icons[1].SetActive(true);
            _door2Icons[0].SetActive(false);
        }
        if (GameManager.Instance._door3Active)
        {
            _door3Icons[0].SetActive(true);
            _door3Icons[1].SetActive(false);
        }
        else
        {
            _door3Icons[1].SetActive(true);
            _door3Icons[0].SetActive(false);
        }
    }
}
