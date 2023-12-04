using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public string enemyLayerName; // Set this in the Inspector for each spawner

    public void SpawnEnemy()
    {
        GameObject enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);

        // Convert layer name to layer index
        int layerIndex = LayerMask.NameToLayer(enemyLayerName);

        // Set the layer of the spawned enemy
        enemy.layer = layerIndex;
    }
}