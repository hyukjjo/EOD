using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatchScreen_AirStrike : WatchScreen
{
    private Coroutine _coroutine;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    public override void Init()
    {
        base.Init();
    }

    public override void TouchScreen()
    {
        base.TouchScreen();

        if (_coroutine != null)
            return;

        _coroutine = StartCoroutine(_AirStrikeCoroutine());
        Debug.Log("Air Strike!");
    }

    private IEnumerator _AirStrikeCoroutine()
    {
        SoundManager.Instance.PlaySound("AirStrike");
        yield return new WaitForSeconds(5f);
        StageMonsterHolder.Instance.KilledBySupportFire();
        _coroutine = null;
    }
}