using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityManager : MonoBehaviour
{
    private CityStats cityStats;
    public CityStatsDisplay cityStatsDisplay;

    // Start is called before the first frame update
    void Start()
    {
        CityStats cityStats = ScriptableObject.CreateInstance<CityStats>();
        cityStats.Init(5, 4, 3, 2, 1);
        this.cityStats = cityStats;

        cityStatsDisplay.RefreshCityStatsUI(cityStats);

        InitializeEventListeners();
    }

    // Update is called once per frame
    void Update()
    {
        // This is only an example of an stat change. Later we will call UpdateCityStat() from outside to manipulate stats.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UpdateCityStat(CityStats.StatType.HealthPoints, -1);
        }
    }

    /// <summary>
    /// This function is used to modify a specific cityStat value and refresh the UI afterwards
    /// </summary>
    public void UpdateCityStat(CityStats.StatType type, int changeValue)
    {        
        cityStats.UpdateStatValue(type, changeValue);
        cityStatsDisplay.RefreshCityStatsUI(cityStats);
    }

    /// <summary>
    /// This function initializes event listeners
    /// </summary>
    private void InitializeEventListeners()
    {
        EventManager.EnemeyKilledEvent.AddListener(HandleEnemyKilled);
        EventManager.EnemyDespawnedEvent.AddListener(HandleEnemyDespawned);
    }

    /// <summary>
    /// Here we do stuff when an enemy was killed by our units
    /// </summary>
    private void HandleEnemyKilled()
    {
        UpdateCityStat(CityStats.StatType.MetaTrophies, 1);
    }

    /// <summary>
    /// Here we do stuff when an enemy could not be killed and despawned
    /// </summary>
    private void HandleEnemyDespawned()
    {
        UpdateCityStat(CityStats.StatType.HealthPoints, -1);
    }

}
