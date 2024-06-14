using UnityEngine;

public class ApplyBuildingBuffs : MonoBehaviour
{
    public building_Inventory buildingInventory; // Reference to the building_Inventory script
    public Unit_Inventory unitInventory; // Reference to the Unit_Inventory script

    void Update()
    {
        ApplyBuildingBonuses();
    }

    private void ApplyBuildingBonuses()
    {
        for (int i = 0; i < buildingInventory.buildingOnFieldCount.Length; i++)
        {
            int currentBuildingCount = buildingInventory.buildingOnFieldCount[i]; // Aktueller Gebäudezähler

            // Überprüfe, ob das Gebäude vorhanden ist
            if (currentBuildingCount > 0)
            {
                string buildingType = GetBuildingTypeFromIndex(i);
                ApplyBuildingBonus(buildingType, currentBuildingCount);
            }
        }
    }

    private void ApplyBuildingBonus(string buildingType, int buildingCount)
    {
        Building_Shop_Script buildingShop = FindObjectOfType<Building_Shop_Script>(); // Assuming there's a Building_Shop_Script
        Unit_Shop_Script unitShop = FindObjectOfType<Unit_Shop_Script>(); // Assuming there's a Unit_Shop_Script

        // Apply specific bonus for "Rathaus" outside the loop
        if (buildingType == "Rathaus")
        {
            Debug.Log("Stadt erhält mehr Einheiten durch das Rathaus.");
            // Decrease reroll cost in Building Shop by -1
            if (buildingShop != null)
            {
                if (buildingShop.rerollCost > 0)
                    buildingShop.rerollCost--;
            }
            // Decrease reroll cost in Unit Shop by -1
            if (unitShop != null)
            {
                if (unitShop.rerollCost > 0)
                    unitShop.rerollCost--;
            }
        }
        else    if (buildingType == "Taverne")
    {
        // Adjust unit cost in Tavern for each unit in unitInventory
        foreach (var unitPrefab in unitInventory.unitPrefabs)
        {
            BaseUnit_Script unitScript = unitPrefab.GetComponent<BaseUnit_Script>();
            if (unitScript != null)
            {
                // Decrease unit cost by 1, but ensure it doesn't go below 1
                unitScript.unitCost = Mathf.Max(1, unitScript.unitCost - 1);
            }
        }

        // Refresh the Unit Shop display after adjusting costs
        if (unitShop != null)
        {
            unitShop.setRandomUnit(false); // Update the display in Unit Shop
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
