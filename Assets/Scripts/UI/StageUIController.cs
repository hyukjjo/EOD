using UnityEngine;

public class StageUIController : MonoBehaviour
{
    [SerializeField]
    private GameObject _waitStatePanel;
    [SerializeField]
    private GameObject _roundStatePanel;
    [SerializeField]
    private GameObject _maintenanceStatePanel;
    [SerializeField]
    private GameObject _infiniteModePanel;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.StateChanged += SwitchPanel;
        //HideAllView();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchPanel()
    {
        HideAllView();
        var gameState = GameManager.Instance.GetState();

        switch (gameState)
        {
            case GameState.WAIT:
                _waitStatePanel.SetActive(true);
                break;
            case GameState.ROUND:
                _roundStatePanel.SetActive(true);
                break;
            case GameState.MAINTENANCE:
                _maintenanceStatePanel.SetActive(true);
                break;
            case GameState.INFINITEMODE:
                _infiniteModePanel.SetActive(true);
                break;
            case GameState.PAUSE:
                break;
            default:
                break;
        }
    }

    private void HideAllView()
    {
        _waitStatePanel.SetActive(false);
        _roundStatePanel.SetActive(false);
        _maintenanceStatePanel.SetActive(false);
        _infiniteModePanel.SetActive(false);
    }
}
