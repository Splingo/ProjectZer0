using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public string enemyLayerName; // Set this in the Inspector for each spawner
    public TextMeshProUGUI startFightButtonText;
    public float spawnInterval = 6f;
    public bool waiting = false;
    


    // Spawner spawns Enemy every X seconds (spawnInterval)
    private IEnumerator SpawnEnemyWithInterval()
    {
        while (true)
        {
            if (waiting == false)
            {
                // Spawn enemy on spawner
                GameObject enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);

                // Convert layer name to layer index
                int layerIndex = LayerMask.NameToLayer(enemyLayerName);

                // Set the layer of the spawned enemy
                enemy.layer = layerIndex;

                waiting = true;
                yield return new WaitForSeconds(spawnInterval);
            }
            else
            {
                yield return new WaitForSeconds(spawnInterval);
                waiting = false;
            }
        }
    }

    // function called by Button on canvas
    public void SpawnEnemy()
    {
        Debug.Log(startFightButtonText.text);
        if (startFightButtonText.text == "Start Fight")
        {
            StartCoroutine(SpawnEnemyWithInterval());
        }
        else
        {
            StopAllCoroutines();
            waiting = false;
        }
        
    }
}