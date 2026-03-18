using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StageUI_Maintenance_GateStatus : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _textGateOpenedCount;
    [SerializeField]
    private TextMeshProUGUI _textGateClosedCount;

    // Start is called before the first frame update
    void Start()
    {
        SetText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetText()
    {
        int gateOpenedCount = 0;
        int gateClosedCount = 0;

        if (GameManager.Instance._door1Active)
            gateOpenedCount++;
        else
            gateClosedCount++;

        if (GameManager.Instance._door2Active)
            gateOpenedCount++;
        else
            gateClosedCount++;

        if (GameManager.Instance._door3Active)
            gateOpenedCount++;
        else
            gateClosedCount++;

        _textGateOpenedCount.text = gateOpenedCount.ToString();
        _textGateClosedCount.text = gateClosedCount.ToString();
    }
}
