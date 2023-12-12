using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityStats : ScriptableObject
{
    private int healthPoints;
    private int gold;
    private int wood;
    private int stone;
    private int metaTrophies;

    public static CityStats CreateInstance(int healthPoints, int gold, int wood, int stone, int metaTrophies)
    {
        var stats = ScriptableObject.CreateInstance<CityStats>();
        stats.Init(healthPoints, gold, wood, stone, metaTrophies);
        return stats;
    }

    public void Init(int healthPoints, int gold, int wood, int stone, int metaTrophies)
    {
        this.healthPoints = healthPoints;
        this.gold = gold;
        this.wood = wood;
        this.stone = stone;
        this.metaTrophies = metaTrophies;
    }

    public int HealthPoints() { return healthPoints; }

    public int Gold() { return gold; }

    public int Wood() { return wood; }

    public int Stone() { return stone; }

    public int MetaTrophies() { return metaTrophies; }
}
