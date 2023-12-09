using System.Collections;
using TMPro;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public string enemyLayerName; // Set this in the Inspector for each spawner
    public TextMeshProUGUI startFightButtonText;
    // Review: write '6.0f' to clearly indicate usage of float. (open for discussion :D )
    public float spawnInterval = 6f;
    public bool waiting = false;


    // Spawner spawns Enemy every X seconds (spawnInterval)
    private IEnumerator SpawnEnemyWithInterval()
    {
        while (true)
        {
            // Review: Use '!waiting'? Sounds still understandable to me. 
            if (waiting == false)
            {
                // Review: Move enemy spawn to own function -> better reusability
                // Spawn enemy on spawner
                GameObject enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);

                // Convert layer name to layer index
                int layerIndex = LayerMask.NameToLayer(enemyLayerName);

                // Set the layer of the spawned enemy
                enemy.layer = layerIndex;

                waiting = true;
                // Review: Are we not waiting 2xspawnInterval time due to the next line?
                // Loop goes like this:
                // if -> spawn enemies -> waiting = true -> wait 6 secs -> next iteration -> else -> wait 6 secs -> waiting = false -> return to beginning?
                yield return new WaitForSeconds(spawnInterval);
            }
            else
            {
                yield return new WaitForSeconds(spawnInterval);
                waiting = false;
            }
        }
    }

    // Maybe find a better name for this? 'ToggleEnemySpawn' or something similar. With the current name I would expect this spawns enemies directly,
    // but it only toggles the spawn on/off.
    // Function called by Button on canvas
    public void SpawnEnemy()
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