using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CityStatistics", menuName = "ScriptableObjects/CityStatistics", order = 1)]
public class CityStatistics : ScriptableObject
{
    public enum StatType
    {
        HealthPoints,
        Gold,
        EnemiesKilled,
        MetaTrophies,
        // Add other stat types as needed
    }

    private Dictionary<StatType, int> stats = new Dictionary<StatType, int>();

    public void Init(int healthPoints, int gold, int enemiesKilled, int metaTrophies)
    {
        stats[StatType.HealthPoints] = healthPoints;
        stats[StatType.Gold] = gold; // Initialize gold
        stats[StatType.EnemiesKilled] = enemiesKilled;
        stats[StatType.MetaTrophies] = metaTrophies;
        // Initialize other stats as needed
    }

    public int GetStat(StatType type)
    {
        if (stats.TryGetValue(type, out int value))
        {
            return value;
        }
        return 0;
    }

    public void UpdateStatValue(StatType type, int changeValue)
    {
        if (stats.ContainsKey(type))
        {
            stats[type] += changeValue;
        }
    }
}
