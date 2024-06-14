using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Set_Reroll_Text : MonoBehaviour
{
    public Building_Shop_Script building_Shop_Script;

    // Reference to the TextMeshProUGUI component
    public TextMeshProUGUI rerollText;

    private int previousBuildingRerollCost;

    void Start()
    {
        // Find the scripts if not already assigned

        if (building_Shop_Script == null)
        {
            building_Shop_Script = FindObjectOfType<Building_Shop_Script>();
        }

        // Find the TextMeshProUGUI component on this GameObject if not assigned
        if (rerollText == null)
        {
            rerollText = GetComponent<TextMeshProUGUI>();
        }

        // Initialize previous values


        if (building_Shop_Script != null)
        {
            previousBuildingRerollCost = building_Shop_Script.rerollCost;
        }
    }

    void Update()
    {

        // Check if building_Shop_Script is not null and if rerollCost has changed
        if (building_Shop_Script != null && building_Shop_Script.rerollCost != previousBuildingRerollCost)
        {
            rerollText.text = $"Reroll-Cost: {building_Shop_Script.rerollCost}";
            previousBuildingRerollCost = building_Shop_Script.rerollCost;
        }
    }
}
