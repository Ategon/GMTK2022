using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class EnemySpawner : MonoBehaviour
{
    public WaveInfo[] waves;
    [SerializeField] private float hordeChanceBase;
    [SerializeField] private float hordeChanceDecrease;
    [SerializeField] private float enemySpawnChanceBase;
    [SerializeField] private float enemySpawnChanceDecrease;
    [SerializeField] private int hordeSize;
    [SerializeField] private GameObject player;

    private double gameTimer;
    private double enemyTimer = 0;

    private void FixedUpdate()
    {
        gameTimer += Time.deltaTime;
        enemyTimer -= Time.deltaTime;

        if(enemyTimer <= 0)
        {
            enemyTimer = 0.1f;
            SpawnEnemy();
        }
    }

    public void SpawnEnemy()
    {
        int totalPriority = waves[(int) Math.Floor(gameTimer / 60)].enemies.Sum(enemy => enemy.weight);
        int randomSide = UnityEngine.Random.Range(0, 4);
        int enemyType = UnityEngine.Random.Range(0, totalPriority);
        float enemySpawned = UnityEngine.Random.Range(0, enemySpawnChanceBase + enemySpawnChanceDecrease * (float) gameTimer);
        float hordeSpawned = UnityEngine.Random.Range(0, hordeChanceBase + hordeChanceDecrease * (float) gameTimer);

        if(enemySpawned < 1)
        {
            GameObject enemy;

            enemy = waves[0].enemies[0].gameObject; // TODO Change to selecting from wave info instead of static.

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
                    spawned = Instantiate(spawnedThing, new Vector3(20f + player.transform.position.x, 1, UnityEngine.Random.Range(20f + player.transform.position.z, -20f + player.transform.position.z)), Quaternion.identity);
                    spawned.transform.SetParent(transform);
                    break;
                case 1:
                    spawned = Instantiate(spawnedThing, new Vector3(-20f + player.transform.position.x, 1, UnityEngine.Random.Range(20f + player.transform.position.z, -20f + player.transform.position.z)), Quaternion.identity);
                    spawned.transform.SetParent(transform);
                    break;
                case 2:
                    spawned = Instantiate(spawnedThing, new Vector3(UnityEngine.Random.Range(20f + player.transform.position.x, -20f + player.transform.position.x), 1, 20f + player.transform.position.z), Quaternion.identity);
                    spawned.transform.SetParent(transform);
                    break;
                case 3:
                    spawned = Instantiate(spawnedThing, new Vector3(UnityEngine.Random.Range(20f + player.transform.position.x, -20f + player.transform.position.x), 1, -20f + player.transform.position.z), Quaternion.identity);
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
