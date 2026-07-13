using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class WaveSpawner : MonoBehaviour
{
    [Header("Wave Data")]
    public int waveDuration;
    public int currentWave;
    private int _waveValue;
    private float _waveTimer;
    private float _spawnInterval;
    private float _spawnTimer;

    [Header("Enemies Data")]
    [SerializeField] List<Enemy> enemies = new List<Enemy>();
    public List<GameObject> enemiesToSpawn = new List<GameObject>();
    public List<GameObject> spawnedEnemies = new List<GameObject>();

    [Header("Boss Data")]
    [SerializeField] GameObject[] bossesPrefab;
    private int _bossIndex = -1;

    [Header("Player Infromation")]
    [SerializeField] Transform playerPosition;
    [SerializeField] Vector2 minimumSpawRadius;
    [SerializeField] Vector2 maximunSpawRadius;
    void Start()
    {
        currentWave--;
        GenerateWave();
    }

    void FixedUpdate()
    {
        UpdateList();
        if (_spawnTimer <= 0)
        {
            
            if (enemiesToSpawn.Count > 0) //spawn an enemy
            {
                Vector3 spawnOffset = new Vector3(Random.Range(minimumSpawRadius.x, maximunSpawRadius.x), Random.Range(minimumSpawRadius.y, maximunSpawRadius.y), 0);
                Vector3 spawnPosition = playerPosition.position + spawnOffset;
                // Instantiate the enemy at the calculated position
                GameObject enemy = PoolManager.Instance.SpawnFromPool(enemiesToSpawn[0].name, enemiesToSpawn[0], spawnPosition, transform.rotation, GameObject.Find("EnemiesContainer").transform); // spawn first enemy in our list
                enemiesToSpawn.RemoveAt(0); // and remove it
                spawnedEnemies.Add(enemy);
                _spawnTimer = _spawnInterval;
            }
            else
            {
                _waveTimer = 0; // if no enemies remain, end wave
            }

        }
        else
        {
            _spawnTimer -= Time.fixedDeltaTime;
            _waveTimer -= Time.fixedDeltaTime;
        }

        if (_waveTimer <= 0 && spawnedEnemies.Count <= 0 && !IsBossAlive() && currentWave % 3 == 0)
        {
            GenerateBoss();
            FindAnyObjectByType<BossManager>().bossCurrentHp += (currentWave + currentWave);
            Debug.Log($"Boss Health {FindAnyObjectByType<BossManager>().bossCurrentHp} Round {currentWave}");
        }
        else if (_waveTimer <= 0 && spawnedEnemies.Count <= 0 && !IsBossAlive())
        {
            GenerateWave();
        }
    }

    private bool IsBossAlive()
    {
        return FindObjectsByType<BossManager>().Any(boss => !boss.isDead);
    }
    public void GenerateWave()
    {
        currentWave++;
        GameEventManager.Instance?.TriggerWaveChanged(currentWave);
        PlayerPrefs.SetInt("WaveDied", currentWave);
        _waveValue = currentWave * 10;
        GenerateEnemies();
        _spawnInterval = waveDuration / enemiesToSpawn.Count; // gives a fixed time between each enemies
        _waveTimer = waveDuration; 
    }

    public void GenerateEnemies()
    {
        List<GameObject> generatedEnemies = new List<GameObject>();
        while (_waveValue > 0 || generatedEnemies.Count < 50)
        {
            int randEnemyId = Random.Range(0, enemies.Count);
            int randEnemyCost = enemies[randEnemyId].cost;

            if (_waveValue - randEnemyCost >= 0)
            {
                generatedEnemies.Add(enemies[randEnemyId].enemyPrefab);
                _waveValue -= randEnemyCost;
            }
            else if (_waveValue <= 0)
            {
                break;
            }
        }
        enemiesToSpawn.Clear();
        enemiesToSpawn = generatedEnemies;
    }

    private void GenerateBoss()
    {
        if (spawnedEnemies.Count <= 0 && !IsBossAlive())
        {
            _bossIndex++;
            currentWave++;
            GameEventManager.Instance?.TriggerWaveChanged(currentWave);
            InstantiateBossByIndex(_bossIndex);
        }
    }

    private void InstantiateBossByIndex(int index)
    {
        Vector3 spawnOffset = new Vector3(Random.Range(minimumSpawRadius.x, maximunSpawRadius.x), Random.Range(minimumSpawRadius.y, maximunSpawRadius.y), 0);
        Vector3 spawnPosition = playerPosition.position + spawnOffset;
        if (spawnedEnemies.Count <= 0 && index % 2 == 0)
        {
            PoolManager.Instance.SpawnFromPool(bossesPrefab[0].name, bossesPrefab[0], spawnPosition, transform.rotation, GameObject.Find("EnemiesContainer").transform);
        }
        else
        {
            PoolManager.Instance.SpawnFromPool(bossesPrefab[1].name, bossesPrefab[1], spawnPosition, transform.rotation, GameObject.Find("EnemiesContainer").transform);
        }
    }
    public void UpdateList()
    {
        GameEventManager.Instance?.TriggerWaveEnemiesChanged(spawnedEnemies.Count);
        for (int i = enemiesToSpawn.Count - 1; i >= 0; i--)
        {
            if (enemiesToSpawn[i] == null)
            {
                enemiesToSpawn.RemoveAt(i);
            }
        }
    }
    void OnValidate()
    {
        enemiesToSpawn.RemoveAll(item => item == null);
    }
}

[System.Serializable]
public class Enemy
{
    public GameObject enemyPrefab;
    public int cost;
}

