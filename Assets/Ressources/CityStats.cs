using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityStats : MonoBehaviour
{
    int HealthPoints { get; set; }
    int Gold { get; set; }
    int Wood { get; set; }
    int Stone { get; set; }
    int MetaTrophies { get; set; }

    public CityStats(int healthPoints, int gold, int wood, int stone, int metaTrophies)
    {
        HealthPoints = healthPoints;
        Gold = gold;
        Wood = wood;
        Stone = stone;
        MetaTrophies = metaTrophies;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
