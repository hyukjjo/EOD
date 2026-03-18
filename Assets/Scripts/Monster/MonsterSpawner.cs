using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : Singleton<MonsterSpawner>
{
    [SerializeField]
    private List<Transform> _spawnPointList = new List<Transform>();
    [SerializeField]
    private Transform _door1SpawnPoint;
    [SerializeField]
    private Transform _door2SpawnPoint;
    [SerializeField]
    private Transform _door3SpawnPoint;
    private Coroutine _spawnMonsterCoroutine;

    [SerializeField]
    private string _normalMonsterName;
    [SerializeField]
    private string _bossMonsterName;
    [SerializeField]
    private float _spawnDelay = 0f;
    [SerializeField]
    private float _bossSpawnTime = 0f;
    [SerializeField]
    private int _bossSpawnCount = 0;
    [SerializeField]
    private float _randomSpawnOffset = 0f;
    [SerializeField]
    private StageMonsterHolder _stageMonsterHolder;
    private float _currentTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.RoundStart += InitSpawnMonster;
        GameManager.Instance.InfiniteModeStart += InitSpawnMonster;
        GameManager.Instance.MaintenanceStart += StopSpawnMonster;
        GameManager.Instance.RoundLevelUp += SpawnLevelUp;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitSpawnMonster()
    {
        SetSpawner();

        _spawnMonsterCoroutine = StartCoroutine(SpawnMonsterCoroutine());

        if(GameManager.Instance.IsFinalRound())
        {
            SpawnBossMonster();
        }
    }

    private void SpawnBossMonster()
    {
        StartCoroutine(_spawnBossCoroutine());
    }

    private IEnumerator _spawnBossCoroutine()
    {
        for (int i = 0; i < _bossSpawnCount; i++)
        {
            SpawnMonster(_bossMonsterName);
            yield return new WaitForSeconds(1f);
        }
    }

    public void SpawnMonster(string targetName)
    {
        var monster = ObjectPoolManager.Instance.Spawn(targetName).GetComponent<Monster>();
        int randomNumber = Random.Range(0, _spawnPointList.Count);
        float randomOffset = Random.Range(-_randomSpawnOffset, _randomSpawnOffset);
        monster.transform.position = _spawnPointList[randomNumber].transform.position + new Vector3(randomOffset, 0f, randomOffset);
        monster.transform.SetParent(_stageMonsterHolder.transform);
        _stageMonsterHolder.AddMonster(monster);
        monster.SetMoveTarget(GameManager.Instance.GetPlayer().transform);
    }

    private void StopSpawnMonster()
    {
        if(_spawnMonsterCoroutine != null)
        {
            StopCoroutine(_spawnMonsterCoroutine);
        }
    }

    private IEnumerator SpawnMonsterCoroutine()
    {
        while(true)
        {
            SpawnMonster(_normalMonsterName);
            yield return new WaitForSeconds(_spawnDelay);
        }
    }

    public void SetSpawner()
    {
        if (GameManager.Instance._door1Active && !_spawnPointList.Contains(_door1SpawnPoint) && _door1SpawnPoint != null)
            _spawnPointList.Add(_door1SpawnPoint);
        if (GameManager.Instance._door2Active && !_spawnPointList.Contains(_door2SpawnPoint) && _door2SpawnPoint != null)
            _spawnPointList.Add(_door2SpawnPoint);
        if (GameManager.Instance._door3Active && !_spawnPointList.Contains(_door3SpawnPoint) && _door3SpawnPoint != null)
            _spawnPointList.Add(_door3SpawnPoint);
    }

    private void SpawnLevelUp()
    {
        if (_spawnDelay > 1)
        {
            _spawnDelay--;
        }
    }

    public void SetSpawnTime(float spawnTime)
    {
        _spawnDelay = spawnTime;
    }
}
