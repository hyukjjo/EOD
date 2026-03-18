using UnityEngine;
using TMPro;

public class StageUI_Maintenance : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _textTimeLimit;
    [SerializeField]
    private StageUI_Maintenance_EntranceIcon _icon;
    private GameManager _gameManager;
    private int Sec = 0;

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        Sec = (int)(_gameManager.GetMaintenanceTime() - _gameManager.GetCurrentStageTime());
        //_textTimeLimit.text = "TIME LIMIT : " + Sec;
        _textTimeLimit.text = Sec.ToString();
    }

    private void OnEnable()
    {
        _icon.SetActiveIcons();
    }
}