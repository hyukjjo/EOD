using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StageUI_InfiniteMode : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _textKillCount;

    private void OnEnable()
    {
        SetKillCountText();
    }

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.MonsterKill += SetKillCountText;
    }

    private void SetKillCountText()
    {
        _textKillCount.text = "KILL : " + GameManager.Instance.GetCurrentKillCount() + " / ¡Ä";
    }
}
