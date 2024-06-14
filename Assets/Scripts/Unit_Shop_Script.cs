using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit_Shop_Script : MonoBehaviour
{
    [SerializeField]
    private GameObject targetObject;

    public Unit_Inventory unit_Inventory;
    public int randomNumber;
    public Sprite[] sprites;
    public int rerollCost = 0;

    public bool raiseCost = false;

    private CityManager cityManager;

    void Start()
    {
        cityManager = FindObjectOfType<CityManager>();
        if (cityManager == null)
        {
            Debug.LogError("CityManager not found in the scene!");
            return;
        }

        unit_Inventory = FindObjectOfType<Unit_Inventory>();
        setRandomUnit(raiseCost);
    }

    void Update()
    {
        // Optional: Add any relevant update logic if needed
    }

    public void setRandomUnit(bool setter)
    {
        if (cityManager == null)
        {
            Debug.LogError("CityManager is null!");
            return;
        }

        // Check if city can afford the reroll cost
        if (cityManager.CanAffordReroll(rerollCost))
        {
            randomNumber = Random.Range(0, sprites.Length);
            Sprite newSprite = sprites[randomNumber];
            targetObject.GetComponent<Image>().sprite = newSprite;
            raiseCost = setter;

            // Deduct the reroll cost from city resources and increase reroll cost
            cityManager.DeductRerollCost(rerollCost);
            rerollCost++;

            raiseCost = false;
        }
        else
        {
            Debug.Log("Not enough resources (gold) to buy unit.");
            // Optionally reset targetObject sprite or perform other actions
        }
    }

    public void getRandomUnit(int unitPrefix)
    {
        if (cityManager == null)
        {
            Debug.LogError("CityManager is null!");
            return;
        }

        // Use the unit's value to deduct from city resources
        int unitValue = GetUnitValue(randomNumber);

        if (cityManager.CanAffordReroll(unitValue))
        {
            // Deduct the unit's value from the city's resources
            cityManager.DeductRerollCost(unitValue);

            // Add the unit to the inventory
            unit_Inventory.AddUnitToInventory(randomNumber);
        }
        else
        {
            Debug.Log("Not enough resources (gold) to buy unit.");
        }
    }

    private int GetUnitValue(int index)
    {
        // Here you can define how to get the value of a unit based on its index.
        // For example, if you have a predefined array or list of unit values:
        int[] unitValues = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
        if (index >= 0 && index < unitValues.Length)
        {
            return unitValues[index];
        }
        return 0;
    }
}
