using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityStats : ScriptableObject
{
    public readonly int healthPoints;
    public readonly int gold;
    public readonly int wood;
    public readonly int stone;
    public readonly int metaTrophies;

    public CityStats(int healthPoints, int gold, int wood, int stone, int metaTrophies)
    {
        this.healthPoints = healthPoints;
        this.gold = gold;
        this.wood = wood;
        this.stone = stone;
        this.metaTrophies = metaTrophies;
    }
}
