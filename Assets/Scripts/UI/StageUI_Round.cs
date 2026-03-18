using UnityEngine;
using TMPro;

public class StageUI_Round : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _textRound;
    [SerializeField]
    private TextMeshProUGUI _textKillCount;

    private void OnEnable()
    {
        SetRoundText();
        SetKillCountText();
    }

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.MonsterKill += SetKillCountText;
    }

    private void SetRoundText()
    {
        _textRound.text = "ROUND " + GameManager.Instance.GetRound();
    }

    private void SetKillCountText()
    {
        _textKillCount.text = (GameManager.Instance.GetKillCountForRound() - GameManager.Instance.GetCurrentKillCount()).ToString();
    }
}
