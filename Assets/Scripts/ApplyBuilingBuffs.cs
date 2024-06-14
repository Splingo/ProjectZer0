using UnityEngine;

public class ApplyBuildingBuffs : MonoBehaviour
{
    public building_Inventory buildingInventory; // Reference to the building_Inventory script
    public Unit_Inventory unitInventory; // Reference to the Unit_Inventory script

    private int[] previousBuildingCounts; // Array to store previous building counts

    void Start()
    {
        // Initialize previousBuildingCounts with the initial counts from buildingInventory
        previousBuildingCounts = new int[buildingInventory.buildingOnFieldCount.Length];
        for (int i = 0; i < buildingInventory.buildingOnFieldCount.Length; i++)
        {
            previousBuildingCounts[i] = buildingInventory.buildingOnFieldCount[i];
        }
    }

    void Update()
    {
        // Check for changes in building counts and apply bonuses if necessary
        CheckBuildingCountsAndApplyBonuses();
    }

   private void CheckBuildingCountsAndApplyBonuses()
{
    // Ensure both arrays (buildingOnFieldCount and previousBuildingCounts) have the same length
    int length = Mathf.Min(buildingInventory.buildingOnFieldCount.Length, previousBuildingCounts.Length);

    for (int i = 0; i < length; i++)
    {
        int currentBuildingCount = buildingInventory.buildingOnFieldCount[i];
        int previousBuildingCount = previousBuildingCounts[i];

        // Check if the count has changed
        if (currentBuildingCount != previousBuildingCount)
        {
            string buildingType = GetBuildingTypeFromIndex(i);
            ApplyBuildingBonus(buildingType, currentBuildingCount);

            // Update previous count to current count
            previousBuildingCounts[i] = currentBuildingCount;
        }
    }
}


    private void ApplyBuildingBonus(string buildingType, int buildingCount)
    {
        Building_Shop_Script buildingShop = FindObjectOfType<Building_Shop_Script>(); // Assuming there's a Building_Shop_Script
        Unit_Shop_Script unitShop = FindObjectOfType<Unit_Shop_Script>(); // Assuming there's a Unit_Shop_Script

        // Implement your bonus logic here based on buildingType and buildingCount
        // Example:
        if (buildingType == "Rathaus")
        {
            Debug.Log("Stadt erhält mehr Einheiten durch das Rathaus.");
            // Apply bonus for Rathaus
            if (buildingShop != null)
            {
                if (buildingShop.rerollCost > 0)
                    buildingShop.rerollCost--;
            }
            if (unitShop != null)
            {
                if (unitShop.rerollCost > 0)
                    unitShop.rerollCost--;
            }
        }
        else if (buildingType == "Taverne")
        {
            // Apply bonus for Taverne
            foreach (var unitPrefab in unitInventory.unitPrefabs)
            {
                BaseUnit_Script unitScript = unitPrefab.GetComponent<BaseUnit_Script>();
                if (unitScript != null)
                {
                    unitScript.unitCost = Mathf.Max(1, unitScript.unitCost - 1);
                }
            }
            if (unitShop != null)
            {
                unitShop.setRandomUnit(false); // Update the display in Unit Shop
            }
        }
        // Add more conditions as needed for other building types
    }

    // Example method to get building type based on index; replace with your actual implementation
    private string GetBuildingTypeFromIndex(int index)
    {
        // Replace with your logic to map index to building type
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
            default: return "Unknown";
        }
    }
}
