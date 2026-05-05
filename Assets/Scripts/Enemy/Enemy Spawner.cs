using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;


public enum SpawnMode
{
    Timed,
    Wave
}

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private EnemyAI enemyPrefab;
    [SerializeField] private float spawnInterval;
    [SerializeField] private int maxEnemiesNumber;
    [SerializeField] private Player player;

    private List<EnemyAI> spawnedEnemies = new List<EnemyAI>();
    private float timeSinceLastSpawn;
    private int enemiesKilled;


    private SpawnMode spawnMode;

    [Header("Wave Mode")]
    [SerializeField] private Wave[] waves;
    [SerializeField] private float timeBetweenWaves = 10f;

    private int currentWaveIndex = 0;
    private bool isSpawningWave = false;

    private int enemiesSpawnedInWave;
    private int enemiesKilledInWave;


    void Start()
    {
        timeSinceLastSpawn = spawnInterval;
        enemiesKilled = 0;
        //Stats.Instance.SetTotal(maxEnemiesNumber);



        Subscribe();
    }


    private void Update()
    {
        if (GameManager.Instance.IsPaused() || GameManager.Instance.IsGameOver())
            return;

        if (spawnMode == SpawnMode.Timed)
        {
            HandleTimedSpawn();
        }
        else if (spawnMode == SpawnMode.Wave)
        {
            if (!isSpawningWave)
            {
                StartCoroutine(HandleWaveSpawn());
            }
        }

        Stats.Instance.SetEnemyText(maxEnemiesNumber, enemiesKilled);
    }

    //Handles Enemy Spawn in Timed Mode
    void HandleTimedSpawn()
    {
        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn > spawnInterval)
        {
            timeSinceLastSpawn = 0f;

            if (spawnedEnemies.Count < maxEnemiesNumber)
            {
                SpawnNewEnemies();
            }
        }
    }

    public void SetMode(SpawnMode mode)
    {
        spawnMode = mode;
    }


    IEnumerator HandleWaveSpawn()
    {
        isSpawningWave = true;

        enemiesSpawnedInWave = 0;
        enemiesKilledInWave = 0;

        if (currentWaveIndex >= waves.Length)
        {
            yield break;
        }
            //currentWaveIndex = 0; // loop waves (optional)

        Wave wave = waves[currentWaveIndex];

        for (int i = 0; i < wave.enemyCount; i++)
        {
            if (GameManager.Instance.IsGameOver())
                break;

            if (spawnedEnemies.Count < maxEnemiesNumber)
            {
                SpawnNewEnemies();
            }

            yield return new WaitForSeconds(wave.spawnDelay);
        }

        currentWaveIndex++;

        yield return new WaitForSeconds(timeBetweenWaves);

        isSpawningWave = false;
    }

    

    //Listen to events
    void Subscribe()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager is NULL in Spawner!");
            return;
        }

        GameManager.Instance.onRestartGame -= Resetgame; // avoid duplicates
        GameManager.Instance.onRestartGame += Resetgame;

        
    }

    public void OnEnable()
    {       

        Invoke(nameof(Subscribe), 0.1f);
    }    

    public void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.onRestartGame -= Resetgame;
        }
    }

    private void Resetgame()
    {        

        foreach (EnemyAI enemy in spawnedEnemies)
        {
            if(enemy != null)
            {
                Destroy(enemy.gameObject);
            }            
        }


        spawnedEnemies.Clear();
        enemiesKilled = 0;

        
        timeSinceLastSpawn = spawnInterval;

        currentWaveIndex = 0;
        isSpawningWave = false;

        StopAllCoroutines();

        
        player.ResetHealth();
    }

    private void SpawnNewEnemies()
    {
        EnemyAI enemy = Instantiate(enemyPrefab, transform.position, transform.rotation);
        enemiesSpawnedInWave++;
        int spawnPointIndex = spawnedEnemies.Count % spawnPoints.Length;        
        enemy.Init(player, GetRandomFreeCover());
        enemy.onEnemyDead += IsGameOver;
        spawnedEnemies.Add(enemy);
    }

    private void IsGameOver(EnemyAI enemy)
    {
        enemy.onEnemyDead -= IsGameOver;
        enemiesKilled++;
        enemiesKilledInWave++;
        spawnedEnemies.Remove(enemy);

        Stats.Instance.CalCulateScore(enemiesKilled);
        if (spawnMode == SpawnMode.Timed)
        {

            if (enemiesKilled == maxEnemiesNumber)
            {
                GameManager.Instance.GameOver(1); //Win
                enemy.StopAllActions();
            }
                
        }
        else if (spawnMode == SpawnMode.Wave)
        {
            CheckWaveCompletion(enemy);
        }
    }

    private void CheckWaveCompletion(EnemyAI enemy)
    {
        if (enemiesKilledInWave >= waves[currentWaveIndex - 1].enemyCount)
        {
            // last wave completed?
            if (currentWaveIndex >= waves.Length)
            {
                GameManager.Instance.GameOver(1); //Win
                enemy.StopAllActions();
            }
        }
    }

    public Transform GetRandomFreeCover()
    {
        List<Transform> available = new List<Transform>();

        foreach (var point in spawnPoints)
        {
            bool occupied = false;

            foreach (var enemy in spawnedEnemies)
            {
                if (enemy == null) continue;

                if (Vector3.Distance(enemy.transform.position, point.position) < 1.5f)
                {
                    occupied = true;
                    break;
                }
            }

            if (!occupied)
                available.Add(point);
        }

        if (available.Count == 0)
            return spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];

        return available[UnityEngine.Random.Range(0, available.Count)];
    }

    public bool IsLastEnemy()
    {
        bool isLastEnemy = spawnedEnemies.Count < maxEnemiesNumber ? false : true;
        return isLastEnemy;
    }

    public void ResetSpawnerDirect()
    {
        Resetgame();
    }

}


