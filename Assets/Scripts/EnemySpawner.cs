using System.Collections;
using TMPro;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public string enemyLayerName; // Set this in the Inspector for each spawner
    public TextMeshProUGUI startFightButtonText;
    public float spawnInterval = 12.0f;
    public bool waiting = false;


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


    public void SpawnEnemy()
    {
        // Spawn enemy on spawner
        GameObject enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);

        // Convert layer name to layer index
        int layerIndex = LayerMask.NameToLayer(enemyLayerName);

        // Set the layer of the spawned enemy
        enemy.layer = layerIndex;
    }


    // Function called by Button on canvas
    public void ToggleEnemySpawn()
    {
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