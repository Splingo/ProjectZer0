using UnityEngine;

public class ApplyBuildingBuffs : MonoBehaviour
{
    public building_Inventory buildingInventory; // Reference to the building_Inventory script
    public Unit_Inventory unitInventory; // Reference to the Unit_Inventory script

    private int[] previousBuildingCounts; // Array to store previous building counts

    void Update()
    {
        CheckAndUpdateBuildingBuffs();
    }

    private void CheckAndUpdateBuildingBuffs()
    {
        // Initialize previousBuildingCounts if it's null or has wrong size
        if (previousBuildingCounts == null || previousBuildingCounts.Length != buildingInventory.buildingOnFieldCount.Length)
        {
            previousBuildingCounts = new int[buildingInventory.buildingOnFieldCount.Length];
        }

        for (int i = 0; i < buildingInventory.buildingOnFieldCount.Length; i++)
        {
            int currentBuildingCount = buildingInventory.buildingOnFieldCount[i];
            int previousBuildingCount = previousBuildingCounts[i];

            // Check if building count increased from 0 to 1 or from 1 to 2
            if ((previousBuildingCount == 0 && currentBuildingCount == 1) ||
                (previousBuildingCount == 1 && currentBuildingCount == 2))
            {
                string buildingType = GetBuildingTypeFromIndex(i);
                ApplyBuildingBonus(buildingType);

                // Debug-Log ausgeben, welcher Gebäudetyp betroffen ist
            }

            // Update previousBuildingCounts with current counts for next frame
            previousBuildingCounts[i] = currentBuildingCount;
        }
    }

    private void ApplyBuildingBonus(string buildingType)
    {
        BaseUnit_Script[] units = FindObjectsOfType<BaseUnit_Script>();
        Building_Shop_Script buildingShop = FindObjectOfType<Building_Shop_Script>(); // Assuming there's a Building_Shop_Script
        Unit_Shop_Script unitShop = FindObjectOfType<Unit_Shop_Script>(); // Assuming there's a Unit_Shop_Script

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
                    Debug.Log("Produktivität steigt um Prozente");
                    // Increase all stats by 0.1 for residents
                    unit.UpdateAttackDamage(unit.attackDamage * 1.1f);
                    unit.UpdateMaxHP(unit.maxHP * 1.1f);
                    unit.UpdateAttackRange(unit.attackRange * 1.1f);
                    unit.UpdateAttackSpeed(unit.attackSpeed * 1.1f);
                    // Adjust any other stats as needed
                    break;
                case "Bank":
                    Debug.Log("Stadt erhält mehr Gold durch die Bank.");
                    // Increase kill cost of enemies by +1
                    unit.unitCost++;
                    break;
                case "Taverne":
                    Debug.Log("Einheiten erhalten AttakSpeed durch die Taverne.");
                    // Decrease unit cost in Tavern by -1
                    unit.unitCost--;
                    break;
                case "Shop":
                    Debug.Log("Man kann einkaufen.");
                    break;
                default:
                    break;
            }
        }

        // Apply specific bonus for "Rathaus" outside the loop
        if (buildingType == "Rathaus")
        {
            Debug.Log("Stadt erhält mehr Einheiten durch das Rathaus.");
            // Decrease reroll cost in Building Shop by -1
            if (buildingShop != null)
            {
                buildingShop.rerollCost--;
            }
            // Decrease reroll cost in Unit Shop by -1
            if (unitShop != null)
            {
                unitShop.rerollCost--;
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
