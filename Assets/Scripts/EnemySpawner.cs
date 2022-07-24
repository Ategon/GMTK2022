using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using TMPro;
using DataPipeline;
using Spellbound.Managers;

public class EnemySpawner : MonoBehaviour, IHandler<PlayerInteractionState>
{
    [SerializeField] private MinuteWaves[] minutes;
    private GameObject player;
    [SerializeField] private GameObject boss;

    bool bossSpawned;

    private void Start()
    {
        player = GameObject.Find("Player");
        player.GetComponent<PlayerInteractionPipline>().AddHandler(this);
    }

    public void Handle(in PlayerInteractionState data)
    {
        switch (data.GameState.roundPhase)
        {
            case RoundPhase.Wave:
                bossSpawned = false;
                SpawnEnemy((float) data.GameState.gameTimer, (float) data.deltaTime, data.GameState.wavesElapsed + 5 * data.GameState.roundsElapsed);
                break;
            case RoundPhase.Boss:
                if (!bossSpawned)
                {
                    bossSpawned = true;
                    SpawnBoss(data);
                }
                break;
            default:
                break;
        }
    }

    private void SpawnBoss(PlayerInteractionState data)
    {
        GameObject spawned;
        spawned = Instantiate(boss, new Vector3(UnityEngine.Random.Range(10f + player.transform.position.x, -10f + player.transform.position.x), 10, 10f + player.transform.position.z), Quaternion.identity);
        spawned.transform.SetParent(transform);
        StartCoroutine(ScreenShakeBoss(data.sharedData.VirtualCamera));
    }

    IEnumerator ScreenShakeBoss(Cinemachine.CinemachineVirtualCamera vc)
    {
        yield return new WaitForSeconds(1);
        vc.GetComponent<ScreenShakeController>().StartShake(1f, 50f);
    }

    private void SpawnEnemy(float gameTimer, float deltaTime, int wavesElapsed)
    {
        if (wavesElapsed >= minutes.Count()) return;

        foreach (EnemyInfo enemy in minutes[wavesElapsed].waves[UnityEngine.Random.Range(0, minutes[wavesElapsed].waves.Count())].enemies)
        {
            enemy.spawnTimer += deltaTime;
            if (enemy.spawnTimer >= enemy.spawnRate)
            {
                enemy.spawnTimer -= enemy.spawnRate;
                int randomSide = UnityEngine.Random.Range(0, 4);
                SpawnHelper(randomSide, enemy.gameObject, 1);
            }
        }
    }

    private void SpawnHelper(int side, GameObject spawnedThing, int enemyAmount)
    {
        for (int i = 0; i < enemyAmount; ++i)
        {
            GameObject spawned;
            switch (side)
            {
                case 0:
                    spawned = Instantiate(spawnedThing, new Vector3(20f + player.transform.position.x, 0, UnityEngine.Random.Range(20f + player.transform.position.z, -20f + player.transform.position.z)), Quaternion.identity);
                    spawned.transform.SetParent(transform);
                    break;
                case 1:
                    spawned = Instantiate(spawnedThing, new Vector3(-20f + player.transform.position.x, 0, UnityEngine.Random.Range(20f + player.transform.position.z, -20f + player.transform.position.z)), Quaternion.identity);
                    spawned.transform.SetParent(transform);
                    break;
                case 2:
                    spawned = Instantiate(spawnedThing, new Vector3(UnityEngine.Random.Range(20f + player.transform.position.x, -20f + player.transform.position.x), 0, 20f + player.transform.position.z), Quaternion.identity);
                    spawned.transform.SetParent(transform);
                    break;
                case 3:
                    spawned = Instantiate(spawnedThing, new Vector3(UnityEngine.Random.Range(20f + player.transform.position.x, -20f + player.transform.position.x), 0, -20f + player.transform.position.z), Quaternion.identity);
                    spawned.transform.SetParent(transform);
                    break;
                default:
                    break;
            }
        }
    }

    [System.Serializable]
    public class MinuteWaves
    {
        public WaveInfo[] waves;
    }

    [System.Serializable]
    public class WaveInfo
    {
        public EnemyInfo[] enemies;
    }

    [System.Serializable]
    public class EnemyInfo
    {
        public string name;
        public float spawnRate;
        public GameObject gameObject;
        public float spawnTimer;
    }


    /*// Public Properties
    public double GameTimer { get { return gameTimer; } private set {; } }

    // Set in editor
    [SerializeField] private MinuteWaves[] minutes;

    // References
    [SerializeField] private GameObject player;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject kingDice;

    // Private variables
    private double gameTimer;
    private double enemyTimer;
    private int kings;



    private void FixedUpdate()
    {
        gameTimer += Time.deltaTime;
        enemyTimer -= Time.deltaTime;

        if(enemyTimer <= 0)
        {
            enemyTimer = 0.1f;
            SpawnEnemy();
        }

        if(gameTimer >= 60 * 5 && kings == 0)
        {
            kings++;
            SpawnHelper(UnityEngine.Random.Range(0, 4), kingDice, 1);
        }

        if(gameTimer >= 60 * 10)
        {
            Time.timeScale = 0;
            winScreen.SetActive(true);
        }

        timerText.text = "Survive! " + TimerToString(SplitMinutes(60*15 - gameTimer));
    }

    private string TimerToString(double[] parts)
    {
        return $"{parts[0]:0}:{parts[1]:00.00}";
    }

    private double[] SplitMinutes(double time)
    {
        double minutes = 0;

        while (time >= 60)
        {
            time -= 60;
            minutes++;
        }

        return new double[] { minutes, time };
    }

    public void SpawnEnemy()
    {
        /*int waveNum = (int)Math.Floor(gameTimer / 60);
        int totalPriority = waves[waveNum].enemies.Sum(enemy => enemy.weight);
        int randomSide = UnityEngine.Random.Range(0, 4);
        int enemyType = UnityEngine.Random.Range(0, totalPriority);
        float enemySpawned = UnityEngine.Random.Range(0, enemySpawnChanceBase + enemySpawnChanceDecrease * (float) gameTimer);
        float hordeSpawned = UnityEngine.Random.Range(0, hordeChanceBase + hordeChanceDecrease * (float) gameTimer);*/

    /*if(enemySpawned < 1)
    {
        GameObject enemy;

        int i = 0;
        int j = 0;
        while (i < enemyType) {
            i += waves[waveNum].enemies[j].weight;
            ++j;
        }

        if (j != 0) j -= 1;

        enemy = minutes[waveNum].enemies[j].gameObject;

        int enemyAmount = hordeSpawned < 1 ? hordeSize : 1;
        SpawnHelper(randomSide, enemy, enemyAmount);
    }
}

private void SpawnHelper(int side, GameObject spawnedThing, int enemyAmount)
{
    for(int i = 0; i < enemyAmount; ++i)
    {
        GameObject spawned;
        switch (side)
        {
            case 0:
                spawned = Instantiate(spawnedThing, new Vector3(20f + player.transform.position.x, 0, UnityEngine.Random.Range(20f + player.transform.position.z, -20f + player.transform.position.z)), Quaternion.identity);
                spawned.transform.SetParent(transform);
                break;
            case 1:
                spawned = Instantiate(spawnedThing, new Vector3(-20f + player.transform.position.x, 0, UnityEngine.Random.Range(20f + player.transform.position.z, -20f + player.transform.position.z)), Quaternion.identity);
                spawned.transform.SetParent(transform);
                break;
            case 2:
                spawned = Instantiate(spawnedThing, new Vector3(UnityEngine.Random.Range(20f + player.transform.position.x, -20f + player.transform.position.x), 0, 20f + player.transform.position.z), Quaternion.identity);
                spawned.transform.SetParent(transform);
                break;
            case 3:
                spawned = Instantiate(spawnedThing, new Vector3(UnityEngine.Random.Range(20f + player.transform.position.x, -20f + player.transform.position.x), 0, -20f + player.transform.position.z), Quaternion.identity);
                spawned.transform.SetParent(transform);
                break;
            default:
                break;
        }
    }
}

[System.Serializable]
public class MinuteWaves
{
    public WaveInfo[] waves;
}

[System.Serializable]
public class WaveInfo
{
    public float spawnRate;
    public EnemyInfo[] enemies;
}

[System.Serializable]
public class EnemyInfo
{
    public string name;
    public int weight;
    public GameObject gameObject;
}*/
}
