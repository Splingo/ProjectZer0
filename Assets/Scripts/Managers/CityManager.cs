using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CityManager : MonoBehaviour
{
    private CityStatistics cityStats;
    public CityStatsDisplay cityStatsDisplay;

    void Start()
    {
        CityStatistics cityStats = ScriptableObject.CreateInstance<CityStatistics>();
        cityStats.Init(5, 30, 5, 0, 0); // Initialize with health, gold, wood, enemiesKilled, and metaTrophies
        this.cityStats = cityStats;

        cityStatsDisplay.RefreshCityStatsUI(cityStats);

        PlayerPrefs.SetInt("EnemiesKilled", cityStats.GetStat(CityStatistics.StatType.EnemiesKilled));

        InitializeEventListeners();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UpdateCityStat(CityStatistics.StatType.HealthPoints, -1);
        }
    }

    public void UpdateCityStat(CityStatistics.StatType type, int changeValue)
    {
        cityStats.UpdateStatValue(type, changeValue);
        cityStatsDisplay.RefreshCityStatsUI(cityStats);
    }

    private void InitializeEventListeners()
    {
        EventManager.EnemyKilledEvent.AddListener(HandleEnemyKilled);
        EventManager.EnemyDespawnedEvent.AddListener(HandleEnemyDespawned);
    }

    private void HandleEnemyKilled()
    {
        UpdateCityStat(CityStatistics.StatType.EnemiesKilled, 1);
        UpdateCityStat(CityStatistics.StatType.Gold, 2);
        UpdateCityStat(CityStatistics.StatType.MetaTrophies, 1);
    }

    private void HandleEnemyDespawned()
    {
        UpdateCityStat(CityStatistics.StatType.HealthPoints, -1);

        if (cityStats.GetStat(CityStatistics.StatType.HealthPoints) <= 0)
        {
            PlayerPrefs.SetInt("EnemiesKilled", cityStats.GetStat(CityStatistics.StatType.EnemiesKilled));
            SceneManager.LoadScene(2);
        }
    }

    public bool CanAffordReroll(int goldCost)
    {
        return cityStats.GetStat(CityStatistics.StatType.Gold) >= goldCost;
    }

    public void DeductRerollCost(int goldCost)
    {
        UpdateCityStat(CityStatistics.StatType.Gold, -goldCost);
    }

    public void AddGold(int gold)
    {
        UpdateCityStat(CityStatistics.StatType.Gold, +gold);
    }
}
