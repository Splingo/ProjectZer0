using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using TMPro;
[System.Serializable]

public class CityStatsDisplay : MonoBehaviour
{
    private CityStats cityStats;

    public TextMeshProUGUI Health;
    public TextMeshProUGUI Gold;
    public TextMeshProUGUI Wood;
    public TextMeshProUGUI Stone;
    public TextMeshProUGUI MetaTrophies;

    public CityStatsDisplay(CityStats stats)
    {
        cityStats = stats;
    }


    // Start is called before the first frame update
    void Start()
    {
        cityStats = new(5, 4, 3, 2, 1);
    }

    // Update is called once per frame
    void Update()
    {
        Health.text = cityStats.healthPoints.ToString();
        Gold.text = cityStats.gold.ToString();
        Wood.text = cityStats.wood.ToString();
        Stone.text = cityStats.stone.ToString();
        MetaTrophies.text = cityStats.metaTrophies.ToString();
    }
}
