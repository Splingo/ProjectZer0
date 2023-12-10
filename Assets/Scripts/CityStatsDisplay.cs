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
    public TextMeshProUGUI Stone;
    public TextMeshProUGUI MetaTrophies;

    public void RefreshCityStatsUI(CityStats cityStats)
    {
        if (cityStats == null)
        {
            Debug.LogWarning("Player stats are null");
            return;
        }

        Health.text = cityStats.HealthPoints.ToString();
        Gold.text = cityStats.Gold.ToString();
        Wood.text = cityStats.Wood.ToString();
        Stone.text = cityStats.Stone.ToString();
        MetaTrophies.text = cityStats.MetaTrophies.ToString();
    }
}
