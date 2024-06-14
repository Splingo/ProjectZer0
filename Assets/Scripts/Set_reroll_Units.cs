using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Set_Reroll_Unit : MonoBehaviour
{
    // References to the scripts containing the data
    public Unit_Shop_Script unit_Shop_Script;


    public TextMeshProUGUI rerollText;

    // Variables to store previous values for comparison
    private int previousUnitRerollCost;


    void Start()
    {
        // Find the scripts if not already assigned
        if (unit_Shop_Script == null)
        {
            unit_Shop_Script = FindObjectOfType<Unit_Shop_Script>();
        }

      

        // Find the TextMeshProUGUI component on this GameObject if not assigned
        if (rerollText == null)
        {
            rerollText = GetComponent<TextMeshProUGUI>();
        }

        // Initialize previous values
        if (unit_Shop_Script != null)
        {
            previousUnitRerollCost = unit_Shop_Script.rerollCost;
        }

      
    }

    void Update()
    {
        // Check if unit_Shop_Script is not null and if rerollCost has changed
        if (unit_Shop_Script != null && unit_Shop_Script.rerollCost != previousUnitRerollCost)
        {
            rerollText.text = $"Reroll-Cost: {unit_Shop_Script.rerollCost}";
            previousUnitRerollCost = unit_Shop_Script.rerollCost;
        }

        
    }
}
