using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CityManager : MonoBehaviour
{
    private CityStats cityStats;
    public CityStatsDisplay cityStatsDisplay;

    // Start is called before the first frame update
    void Start()
    {
        CityStats cityStats = ScriptableObject.CreateInstance<CityStats>();
        cityStats.Init(5, 4, 3, 0, 0);
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
        EventManager.EnemyKilledEvent.AddListener(HandleEnemyKilled);
        EventManager.EnemyDespawnedEvent.AddListener(HandleEnemyDespawned);
    }

    /// <summary>
    /// Here we do stuff when an enemy was killed by our units
    /// </summary>
    private void HandleEnemyKilled()
    {
        UpdateCityStat(CityStats.StatType.EnemiesKilled, 1);
        UpdateCityStat(CityStats.StatType.MetaTrophies, 1);
    }

    /// <summary>
    /// Here we do stuff when an enemy could not be killed and despawned
    /// </summary>
    private void HandleEnemyDespawned()
    {
        UpdateCityStat(CityStats.StatType.HealthPoints, -1);

        if (cityStats.GetStat(CityStats.StatType.HealthPoints) <= 0)
        {
            PlayerPrefs.SetInt("EnemiesKilled",cityStats.GetStat(CityStats.StatType.EnemiesKilled));
            SceneManager.LoadScene(2);
        }
    }
}
