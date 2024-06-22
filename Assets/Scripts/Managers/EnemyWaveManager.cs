using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyWaveManager : MonoBehaviour
{
    private int wave = 1;
    static private int enemiesPerWaveMultiplicationFactor = 2;

    private int enemiesCleared = 0;
    private int totalEnemiesThisWave = 0;
    private int enemiesPerSpawner;


    // Start is called before the first frame update
    void Start()
    {
        InitializeEventListeners();
    }

    public void StartNewWave()
    {
        var button = FindObjectOfType<StartWaveButton>();
        button.setButtonText("Fighting Wave " + wave);
        button.startWaveButton.enabled = false;

        var enemySpawners = FindObjectsByType<EnemySpawner>(FindObjectsSortMode.None);

        CalculateEnemiesCount(enemySpawners.Length);

        enemiesCleared = 0;

        foreach (EnemySpawner spawner in enemySpawners)
            spawner.StartEnemySpawn(enemiesPerSpawner);

        wave++;
    }

    private void CalculateEnemiesCount(int enemySpawnerCount)
    {
        enemiesPerSpawner = wave * enemiesPerWaveMultiplicationFactor;
        totalEnemiesThisWave = enemySpawnerCount * enemiesPerSpawner;
    }

    private void InitializeEventListeners()
    {
        EventManager.EnemyKilledEvent.AddListener(HandleEnemyCleared);
        EventManager.EnemyDespawnedEvent.AddListener(HandleEnemyCleared);
    }

    private void EndWave()
    {
        var button = FindObjectOfType<StartWaveButton>();
        button.setButtonText("Start Wave" + wave);
        button.startWaveButton.enabled =true;
    }

    private void HandleEnemyCleared()
    {
        enemiesCleared++;

        if (enemiesCleared == totalEnemiesThisWave)
            EndWave();
    }

}
