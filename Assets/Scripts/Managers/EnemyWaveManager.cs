using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyWaveManager : MonoBehaviour
{
    private int wave = 1;
    static private int enemiesPerWaveMultiplicationFactor = 2;

    private int totalEnemiesThisWave = 0;
    private int enemiesPerSpawner;


    // Start is called before the first frame update
    void Start()
    {
        InitializeEventListeners();
    }

    public void StartNewWave()
    {
        var button = FindObjectOfType<StartFightButton>();
        button.ToggleText();

        var enemySpawners = FindObjectsByType<EnemySpawner>(FindObjectsSortMode.None);

        CalculateEnemiesCount(enemySpawners.Length);

        wave++;

        foreach (EnemySpawner spawner in enemySpawners)
            spawner.StartEnemySpawn(enemiesPerSpawner);
    }

    private void CalculateEnemiesCount(int enemySpawnerCount)
    {
        enemiesPerSpawner = wave * enemiesPerWaveMultiplicationFactor;
        totalEnemiesThisWave = enemySpawnerCount * enemiesPerSpawner;
    }

    private void InitializeEventListeners()
    {
        EventManager.EnemyKilledEvent.AddListener(HandleEnemyKilled);
        EventManager.EnemyDespawnedEvent.AddListener(HandleEnemyDespawned);
    }

    private void HandleEnemyKilled()
    {

    }

    private void HandleEnemyDespawned()
    {

    }
}
