using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateButton : MonoBehaviour
{
    [SerializeField]
    private GameObject _gate;
    [SerializeField]
    private GameObject _gateWoodBoard;
    [SerializeField]
    private StageUI_Maintenance_EntranceIcon _iconObject;
    [SerializeField]
    private StageUI_Maintenance_GateStatus _statusObject;
    [SerializeField]
    private GameObject _guideUI;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.StateChanged += SetGuideUI;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PushButton()
    {
        if (!GameManager.Instance._door3Active)
        {
            GameManager.Instance._door3Active = true;
            _iconObject.SetActiveIcons();
            _statusObject.SetText();
            //_gateWoodBoard.SetActive(true);
            MonsterSpawner.Instance.SetSpawner();
            _gate.GetComponent<Animator>().SetTrigger("Open");
            _gate.GetComponent<AudioSource>().Play();
            _guideUI.SetActive(false);
        }
    }

    public void ForceOpen()
    {
        GameManager.Instance._door3Active = true;
        _iconObject.SetActiveIcons();
        _statusObject.SetText();
        //_gateWoodBoard.SetActive(true);
        MonsterSpawner.Instance.SetSpawner();
        _gate.GetComponent<Animator>().SetTrigger("Open");
        _gate.GetComponent<AudioSource>().Play();
        _guideUI.SetActive(false);
    }

    public void SetGuideUI()
    {
        if(_guideUI != null && !GameManager.Instance._door3Active)
        {
            _guideUI.SetActive(true);
        }
        else
        {
            _guideUI.SetActive(false);
        }
    }
}
