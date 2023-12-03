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
        // Replace these lines when CityStats are coming from external source.
        CityStats cityStats = ScriptableObject.CreateInstance<CityStats>();
        cityStats.Init(5, 4, 3, 2, 1);
        this.cityStats = cityStats;
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Health.text = cityStats.HealthPoints().ToString();
        Gold.text = cityStats.Gold().ToString();
        Wood.text = cityStats.Wood().ToString();
        Stone.text = cityStats.Stone().ToString();
        MetaTrophies.text = cityStats.MetaTrophies().ToString();
    }
}
