using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityStats : ScriptableObject
{
    public int HealthPoints;
    public int Gold;
    public int Wood;
    public int Stone;
    public int MetaTrophies;

    public static CityStats CreateInstance(int healthPoints, int gold, int wood, int stone, int metaTrophies)
    {
        var stats = CreateInstance<CityStats>();
        stats.Init(healthPoints, gold, wood, stone, metaTrophies);
        return stats;
    }

    public void Init(int healthPoints, int gold, int wood, int stone, int metaTrophies)
    {
        HealthPoints = healthPoints;
        Gold = gold;
        Wood = wood;
        Stone = stone;
        MetaTrophies = metaTrophies;
    }
}
