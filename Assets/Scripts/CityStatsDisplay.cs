using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class CityStatsDisplay : MonoBehaviour
{
    public TextMeshProUGUI Health;
    public TextMeshProUGUI Gold;
    public TextMeshProUGUI EnemiesKilled;
    public TextMeshProUGUI MetaTrophies;

    public void RefreshCityStatsUI(CityStatistics cityStats)
    {
        if (cityStats == null)
        {
            Debug.LogWarning("Player stats are null");
            return;
        }

        Health.text = cityStats.GetStat(CityStatistics.StatType.HealthPoints).ToString();
        Gold.text = cityStats.GetStat(CityStatistics.StatType.Gold).ToString();
        EnemiesKilled.text = cityStats.GetStat(CityStatistics.StatType.EnemiesKilled).ToString();
        MetaTrophies.text = cityStats.GetStat(CityStatistics.StatType.MetaTrophies).ToString();
    }
}
