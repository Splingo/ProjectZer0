using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityManager : MonoBehaviour
{
    public CityStats cityStats;
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            cityStats.HealthPoints += 1;
            cityStatsDisplay.RefreshCityStatsUI(cityStats);
        }

    }
}
