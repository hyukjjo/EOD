using UnityEngine;
using System;
using UnityEngine.Events;

public enum GameState
{
    WAIT = 0,
    ROUND,
    MAINTENANCE,
    INFINITEMODE,
    PAUSE,
}

public class GameManager : Singleton<GameManager>
{
    [Header("Global")]
    [SerializeField]
    private Player _player;
    [SerializeField]
    private GameState _firstState = GameState.MAINTENANCE;
    [SerializeField]
    private GameState _currentState;
    [SerializeField]
    private float _currentStageTime;
    [Space]
    [Header("Round")]
    [SerializeField]
    private int _finalRound;
    [SerializeField]
    private int _currentRound = 1;
    [SerializeField]
    private int _killCountForRound;
    [SerializeField]
    private int _currentKillCount;
    [SerializeField]
    private bool _isBossMonsterInField = false;

    [Space]
    [Header("Maintenance")]
    [SerializeField]
    private float _maintenanceTime;
    [SerializeField]
    private bool _isSkipped = false;
    public bool _door1Active = false;
    public bool _door2Active = false;
    public bool _door3Active = false;

    [Space]
    [Header("Infinite Mode")]
    public bool IsInfiniteMode = false;
    public int PlayerGold = 0;
    public float RespawnTime = 0f;

    [Space]
    [Header("StageSound")]
    [SerializeField]
    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip _maintenanceWarningSound;
    [SerializeField]
    private AudioClip _roundStartSound;

    #region GameManager Actions
    public Action GameStart         = () => { Debug.Log("Game Start!"); };
    public Action InfiniteModeStart = () => { Debug.Log("Infinite Mode Start!"); };
    public Action RoundStart        = () => { Debug.Log("Round Start!"); };
    public Action MaintenanceStart  = () => { Debug.Log("Maintenance Start!"); };
    public Action PauseGame         = () => { Debug.Log("Game has been paused!"); };
    public Action StateChanged      = () => { Debug.Log("Game State has been changed!"); };
    public Action RoundLevelUp      = () => { Debug.Log("Round Level Up!"); };
    public Action MonsterKill       = () => { };
    public Action BossMonsterSpawn  = () => { Debug.Log("Boss Monster is spawned in field!"); };
    public Action BossMonsterKill   = () => { Debug.Log("Boss Monster is dead by player!"); };
    public Action PlayerDead        = () => { Debug.Log("Player Dead!"); };
    public Action GameClear         = () => { Debug.Log("Game Clear!"); };
    public Action GameEnd           = () => { Debug.Log("Game End!"); };
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        InitGame();
    }



    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            if(_currentState.Equals(GameState.ROUND))
            {
                _currentKillCount = _killCountForRound;
            }
        }

        PlayGame();
    }

    void PlayGame()
    {
        _currentStageTime += Time.deltaTime;

        switch (_currentState)
        {
            case GameState.WAIT:
                break;
            case GameState.ROUND:
                if(_player.GetPlayerHp() > 0)
                {
                    if (_currentKillCount >= _killCountForRound && !_isBossMonsterInField)
                    {
                        if (_currentRound < _finalRound)
                        {
                            RoundLevelUp();
                            MaintenanceStart();
                        }
                        else
                        {
                            GameClear();
                        }
                    }
                }
                else
                {
                    PlayerDead();
                }
                break;
            case GameState.MAINTENANCE:
                if(_maintenanceTime - _currentStageTime <= 10f)
                {
                    if (!_audioSource.isPlaying)
                    {
                        _audioSource.clip = _maintenanceWarningSound;
                        _audioSource.Play();
                    }
                }
                if(_currentStageTime >= _maintenanceTime || _isSkipped)
                {
                    if (IsInfiniteMode)
                    {
                        InfiniteModeStart();
                    }
                    else
                    {
                        RoundStart();
                    }
                }
                break;
            case GameState.PAUSE:
                break;
            case GameState.INFINITEMODE:
                break;
            default:
                break;
        }
    }

    void InitGame()
    {
        GameStart += () =>
        {
            _player = GameObject.FindObjectOfType<Player>();
            _currentState = _firstState;

            switch (_currentState)
            {
                case GameState.WAIT:
                    break;
                case GameState.ROUND:
                    RoundStart();
                    break;
                case GameState.MAINTENANCE:
                    MaintenanceStart();
                    break;
                case GameState.PAUSE:
                    PauseGame();
                    break;
                default:
                    break;
            }
        };
        RoundStart += () =>
        {
            _currentState = GameState.ROUND;
            StateChanged();
            _audioSource.clip = _roundStartSound;
            _audioSource.Play();
        };
        MaintenanceStart += () =>
        {
            _currentState = GameState.MAINTENANCE;
            StateChanged();
            _audioSource.clip = _roundStartSound;
            _audioSource.Play();
            if(IsInfiniteMode)
                _player.PlayerGold = PlayerGold;
        };
        InfiniteModeStart += () =>
        {
            _currentState = GameState.INFINITEMODE;
            StateChanged();
            _audioSource.clip = _roundStartSound;
            _audioSource.Play();
            MonsterSpawner.Instance.SetSpawnTime(RespawnTime);
        };
        PauseGame += () =>
        {

        };
        StateChanged += () =>
        {
            _currentKillCount = 0;
            _currentStageTime = 0f;
            _isSkipped = false;
        };
        RoundLevelUp += () =>
        {
            _currentRound++;
            //_killCountForRound += 30;

            switch(_currentRound)
            {
                case 2:
                    _killCountForRound = 40;
                    break;
                case 3:
                    _killCountForRound = 90;
                    break;
                default:
                    _killCountForRound += 30;
                    break;
            }
        };
        MonsterKill += () =>
        {
            _currentKillCount++;
        };
        BossMonsterSpawn += () =>
        {
            _isBossMonsterInField = true;
        };
        BossMonsterKill += () =>
        {
            _isBossMonsterInField = false;
        };
        GameEnd += () =>
        {

        };
    }

    public GameState GetState()
    {
        return _currentState;
    }

    public int GetCurrentKillCount()
    {
        return _currentKillCount;
    }

    public int GetKillCountForRound()
    {
        return _killCountForRound;
    }

    public int GetRound()
    {
        return _currentRound;
    }

    public float GetCurrentStageTime()
    {
        return _currentStageTime;
    }

    public float GetMaintenanceTime()
    {
        return _maintenanceTime;
    }

    public void SkipMaintenence()
    {
        _currentStageTime = _maintenanceTime;
    }

    public Player GetPlayer()
    {
        if(_player == null)
            _player = GameObject.FindObjectOfType<Player>();

        return _player;
    }

    public bool IsFinalRound()
    {
        return _currentRound == _finalRound;
    }

    public bool IsBossMonsterInField()
    {
        return _isBossMonsterInField;
    }

    private void OnDestroy()
    {
        GameStart         = null;
        InfiniteModeStart = null;
        RoundStart        = null;
        MaintenanceStart  = null;
        PauseGame         = null;
        StateChanged      = null;
        RoundLevelUp      = null;
        MonsterKill       = null;
        BossMonsterSpawn  = null;
        BossMonsterKill   = null;
        PlayerDead        = null;
        GameClear         = null;
        GameEnd           = null;
    }
}