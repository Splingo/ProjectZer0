using UnityEngine;

public class ApplyCombatBuffs : MonoBehaviour
{
    public building_Inventory buildingInventory; // Reference to the building_Inventory script
    public Unit_Inventory unitInventory; // Reference to the Unit_Inventory script

    public void StartBattle()
    {
        ApplyCombatBonuses();
    }

    private void ApplyCombatBonuses()
    {
        BaseUnit_Script[] units = FindObjectsOfType<BaseUnit_Script>();
        Building_Shop_Script buildingShop = FindObjectOfType<Building_Shop_Script>(); // Assuming there's a Building_Shop_Script
        Unit_Shop_Script unitShop = FindObjectOfType<Unit_Shop_Script>(); // Assuming there's a Unit_Shop_Script

        for (int i = 0; i < buildingInventory.buildingOnFieldCount.Length; i++)
        {
            int currentBuildingCount = buildingInventory.buildingOnFieldCount[i]; // Aktueller Gebäudezähler
            string buildingType = GetBuildingTypeFromIndex(i);

            // Überprüfe, ob das Gebäude auf dem Feld vorhanden ist
            if (currentBuildingCount > 0)
            {
                // Apply building bonuses based on building type and count
                BattleBoosts(buildingType, currentBuildingCount);
            }
        }
    }

    private void BattleBoosts(string buildingType, int buildingCount)
    {
        BaseUnit_Script[] units = FindObjectsOfType<BaseUnit_Script>();
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        Building_Shop_Script buildingShop = FindObjectOfType<Building_Shop_Script>(); // Assuming there's a Building_Shop_Script
        Unit_Shop_Script unitShop = FindObjectOfType<Unit_Shop_Script>(); // Assuming there's a Unit_Shop_Script

        for (int count = 0; count < buildingCount; count++)
        {
            foreach (BaseUnit_Script unit in units)
            {
                switch (buildingType)
                {
                    case "Schmiede":
                        Debug.Log("Einheiten erhalten Angriffsbonus durch die Schmiede.");
                        unit.UpdateAttackDamage(unit.attackDamage + 0.5f);
                        break;
                    case "Kirche":
                        Debug.Log("Einheiten erhalten HP durch die Kirche.");
                        unit.UpdateMaxHP(unit.maxHP + 10f);
                        break;
                    case "Tower":
                        Debug.Log("Einheiten erhalten Range durch den Turm.");
                        unit.UpdateAttackRange(unit.attackRange + 0.15f);
                        break;
                    case "Bewohner":
                        Debug.Log("Produktivität steigt um Prozente.");
                        // Increase all stats by 0.1 for residents
                        unit.UpdateAttackDamage(unit.attackDamage * 1.1f);
                        unit.UpdateMaxHP(unit.maxHP * 1.1f);
                        unit.UpdateAttackRange(unit.attackRange * 1.1f);
                        unit.UpdateAttackSpeed(unit.attackSpeed * 1.1f);
                        // Adjust any other stats as needed
                        break;
                    case "Shop":
                        Debug.Log("Man kann einkaufen.");
                        break;
                    default:
                        break;
                }
            }
            foreach(Enemy enemy in enemies){
                 switch (buildingType) {

                   case "Bank":
                        Debug.Log("Gegner droppen mehr gold.");
                        // Increase kill cost of enemies by +1
                        enemy.droppedGold++;
                        break;
                  default:
                        break;
                 }
            }
        }
    }

    // Example method to get building type based on index; replace with your actual implementation
    private string GetBuildingTypeFromIndex(int index)
    {
        // Replace with your logic to map index to building type
        // For example:
        switch (index)
        {
            case 0: return "Bank";
            case 1: return "Bewohner";
            case 2: return "Kirche";
            case 3: return "Rathaus";
            case 4: return "Schmiede";
            case 5: return "Shop";
            case 6: return "Taverne";
            case 7: return "Tower";
            // Add cases for other indices as needed
            default: return "Unknown"; // Handle default case if necessary
        }
    }
}
