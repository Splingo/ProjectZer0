using System;
using System.Collections;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public string enemyLayerName; // Set this in the Inspector for each spawner
    public TextMeshProUGUI startFightButtonText;
    public float spawnInterval = 12.0f;
    public bool waiting = false;

    private int enemiesToSpawn = 0;
    private int enemiesSpawned = 0;

    // Spawner spawns Enemy every X seconds (spawnInterval)
    private IEnumerator SpawnEnemyWithInterval()
    {
        while (true)
        {
            if (!waiting)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(spawnInterval);
            }
            else
            {
                yield return new WaitForSeconds(spawnInterval);
                waiting = false;
            }
        }
    }


    private void SpawnEnemy()
    {
        System.Random rnd = new System.Random();
        GameObject enemyPrefab = enemyPrefabs[rnd.Next(0, enemyPrefabs.Length)];
        // Spawn enemy on spawner
        GameObject enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);


        // Convert layer name to layer index
        int layerIndex = LayerMask.NameToLayer(enemyLayerName);

        // Set the layer of the spawned enemy
        enemy.layer = layerIndex;

        enemiesSpawned++;
        if (enemiesSpawned == enemiesToSpawn)
        {
            StopAllCoroutines();
            waiting = false;
        }
    }




    // Function called by Button on canvas
    public void StartEnemySpawn(int enemiesToSpawn)
    {
        enemiesSpawned = 0;

        this.enemiesToSpawn = enemiesToSpawn;

        StartCoroutine(SpawnEnemyWithInterval());
    }
}