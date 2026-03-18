using UnityEngine;
using TMPro;

public class WatchScreen_PlayerStatus : WatchScreen
{
    [SerializeField]
    private TextMeshProUGUI _textHp;
    [SerializeField]
    private TextMeshProUGUI _textBullet;
    [SerializeField]
    private MikeNspired.UnityXRHandPoser.ProjectileWeapon[] _projectileWeapons;

    private Player _player;
    private GameManager _gameManager;
    private int currBullet;
    private int maxBullet;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        _gameManager = GameManager.Instance;
        _player = _gameManager.GetPlayer();
    }

    public override void Init()
    {
        base.Init();
    }

    // Update is called once per frame
    void Update()
    {
        _textHp.text = (_player.GetPlayerHp()).ToString();

        if(_player.IsReadyToFire() && _player.CurrentGun.magazineAttach.Magazine != null)
        {
            var mag = _player.CurrentGun.magazineAttach.Magazine;
            _textBullet.text = mag.CurrentAmmo + " / " + mag.MaxAmmo;
        }
        else
        {
            _textBullet.text = "0 / 0";
        }
    }
}