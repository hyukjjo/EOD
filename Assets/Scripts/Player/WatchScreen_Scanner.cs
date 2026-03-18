using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WatchScreen_Scanner : WatchScreen
{
    [SerializeField]
    private float _scanTime;
    [SerializeField]
    private Image _imageBattery;
    [SerializeField]
    private Image _imageScanButton;
    [SerializeField]
    private Sprite[] _batterySprites;
    [SerializeField]
    private int _batteryLife = 3;
    [SerializeField]
    private RawImage _renderTexture;

    private float _currentScanTime = 0f;
    private Coroutine _coroutine;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    public override void Init()
    {
        base.Init();

        GameManager.Instance.MaintenanceStart += ChargeBattery;
    }

    public override void TouchScreen()
    {
        base.TouchScreen();

        if (_coroutine != null)
            return;

        _batteryLife--;

        if (_batteryLife < 0)
        {
            _batteryLife = 0;
            return;
        }
        else
        {
            _imageBattery.sprite = _batterySprites[_batteryLife];
            _coroutine = StartCoroutine(_scanCoroutine());
        }
    }

    private IEnumerator _scanCoroutine()
    {
        _imageScanButton.gameObject.SetActive(false);
        _renderTexture.enabled = true;

        while (_currentScanTime < _scanTime)
        {
            _currentScanTime += Time.deltaTime;
            yield return null;
        }

        _imageScanButton.gameObject.SetActive(true);
        _renderTexture.enabled = false;

        _currentScanTime = 0f;
        _coroutine = null;
    }

    private void ChargeBattery()
    {
        _batteryLife = 3;
        _imageBattery.sprite = _batterySprites[3];
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            ChargeBattery();
        }
    }
}