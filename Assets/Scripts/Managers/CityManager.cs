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

    // We can call this function to update a specific CityStat value and refresh the UI
    public void UpdateCityStat(CityStats.StatType type, int changeValue)
    {
        cityStats.UpdateStatValue(type, changeValue);
        cityStatsDisplay.RefreshCityStatsUI(cityStats);
    }
}
