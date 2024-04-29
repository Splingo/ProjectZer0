using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CityStats : ScriptableObject
{
    public enum StatType
    {
        HealthPoints,
        Gold,
        Wood,
        EnemiesKilled,
        MetaTrophies
    }

    private int HealthPoints;
    private int Gold;
    private int Wood;
    private int EnemiesKilled;
    private int MetaTrophies;

    public static CityStats CreateInstance(int healthPoints, int gold, int wood, int enemiesKilled, int metaTrophies)
    {
        var stats = CreateInstance<CityStats>();
        stats.Init(healthPoints, gold, wood, enemiesKilled, metaTrophies);
        return stats;
    }

    public void Init(int healthPoints, int gold, int wood, int enemiesKilled, int metaTrophies)
    {
        HealthPoints = healthPoints;
        Gold = gold;
        Wood = wood;
        EnemiesKilled = enemiesKilled;
        MetaTrophies = metaTrophies;
    }
    public int GetStat(StatType statType)
    {
        switch (statType)
        {
            case StatType.HealthPoints:
                return HealthPoints;
            case StatType.Gold:
                return Gold;
            case StatType.Wood:
                return Wood;
            case StatType.EnemiesKilled:
                return EnemiesKilled;
            case StatType.MetaTrophies:
                return MetaTrophies;
            default:
                Debug.LogError($"Unknown stat type: {statType}");
                return -1;
        }
    }

    public void UpdateStatValue(StatType statType, int value)
    {
        switch (statType)
        {
            case StatType.HealthPoints:
                HealthPoints += value;
                break;
            case StatType.Gold:
                Gold += value;
                break;
            case StatType.Wood:
                Wood += value;
                break;
            case StatType.EnemiesKilled:
                EnemiesKilled += value;
                break;
            case StatType.MetaTrophies:
                MetaTrophies += value;
                break;
            default:
                Debug.LogError($"Unknown stat type: {statType}");
                break;
        }
    }
}