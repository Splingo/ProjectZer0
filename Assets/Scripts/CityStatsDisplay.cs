using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using TMPro;
[System.Serializable]

public class CityStatsDisplay : MonoBehaviour
{
    public TextMeshProUGUI Health;
    public TextMeshProUGUI Gold;
    public TextMeshProUGUI Wood;
    public TextMeshProUGUI EnemiesKilled;
    public TextMeshProUGUI MetaTrophies;

    public void RefreshCityStatsUI(CityStats cityStats)
    {
        if (cityStats == null)
        {
            Debug.LogWarning("Player stats are null");
            return;
        }

        Health.text = cityStats.GetStat(CityStats.StatType.HealthPoints).ToString();
        Gold.text = cityStats.GetStat(CityStats.StatType.Gold).ToString();
        Wood.text = cityStats.GetStat(CityStats.StatType.Wood).ToString();
        EnemiesKilled.text = cityStats.GetStat(CityStats.StatType.EnemiesKilled).ToString();
        MetaTrophies.text = cityStats.GetStat(CityStats.StatType.MetaTrophies).ToString();
    }
}
