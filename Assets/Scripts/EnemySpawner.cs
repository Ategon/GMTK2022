using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using TMPro;

public class EnemySpawner : MonoBehaviour
{
    public WaveInfo[] waves;
    [SerializeField] private float hordeChanceBase;
    [SerializeField] private float hordeChanceDecrease;
    [SerializeField] private float enemySpawnChanceBase;
    [SerializeField] private float enemySpawnChanceDecrease;
    [SerializeField] private int hordeSize;
    [SerializeField] private GameObject player;
    [SerializeField] private TextMeshProUGUI timerText;

    [SerializeField] private GameObject winScreen;

    [SerializeField] private GameObject kingDice;

    public double gameTimer;
    private double enemyTimer = 0;

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
        int waveNum = (int)Math.Floor(gameTimer / 60);
        int totalPriority = waves[waveNum].enemies.Sum(enemy => enemy.weight);
        int randomSide = UnityEngine.Random.Range(0, 4);
        int enemyType = UnityEngine.Random.Range(0, totalPriority);
        float enemySpawned = UnityEngine.Random.Range(0, enemySpawnChanceBase + enemySpawnChanceDecrease * (float) gameTimer);
        float hordeSpawned = UnityEngine.Random.Range(0, hordeChanceBase + hordeChanceDecrease * (float) gameTimer);

        if(enemySpawned < 1)
        {
            GameObject enemy;

            int i = 0;
            int j = 0;
            while (i < enemyType) {
                i += waves[waveNum].enemies[j].weight;
                ++j;
            }

            if (j != 0) j -= 1;

            enemy = waves[waveNum].enemies[j].gameObject;

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
                    spawned = Instantiate(spawnedThing, new Vector3(10f + player.transform.position.x, 0, UnityEngine.Random.Range(10f + player.transform.position.z, -10f + player.transform.position.z)), Quaternion.identity);
                    spawned.transform.SetParent(transform);
                    break;
                case 1:
                    spawned = Instantiate(spawnedThing, new Vector3(-10f + player.transform.position.x, 0, UnityEngine.Random.Range(10f + player.transform.position.z, -10f + player.transform.position.z)), Quaternion.identity);
                    spawned.transform.SetParent(transform);
                    break;
                case 2:
                    spawned = Instantiate(spawnedThing, new Vector3(UnityEngine.Random.Range(10f + player.transform.position.x, -10f + player.transform.position.x), 0, 10f + player.transform.position.z), Quaternion.identity);
                    spawned.transform.SetParent(transform);
                    break;
                case 3:
                    spawned = Instantiate(spawnedThing, new Vector3(UnityEngine.Random.Range(10f + player.transform.position.x, -10f + player.transform.position.x), 0, -10f + player.transform.position.z), Quaternion.identity);
                    spawned.transform.SetParent(transform);
                    break;
                default:
                    break;
            }
        }
    }

    [System.Serializable]
    public class WaveInfo
    {
        public string waveName;
        public EnemyInfo[] enemies;
    }

    [System.Serializable]
    public class EnemyInfo
    {
        public string name;
        public int weight;
        public GameObject gameObject;
    }
}
