using UnityEngine;
using TMPro;

public class Set_Unit_Price : MonoBehaviour
{
    // Reference to the Unit_Shop_Script to get the randomNumber
    public Unit_Shop_Script unitShopScript;

    // Array of unit prefabs to check for BaseUnit_Script
    public GameObject[] unitPrefabs;
    public TextMeshProUGUI costText;

    // Variable to store the previous random number
    private int previousRandomNumber = -1;

    void Start()
    {
        // Find the Unit_Shop_Script in the scene if not assigned
        if (unitShopScript == null)
        {
            unitShopScript = FindObjectOfType<Unit_Shop_Script>();
        }

        // Find the TextMeshProUGUI component on this GameObject if not assigned
        if (costText == null)
        {
            costText = GetComponent<TextMeshProUGUI>();
        }

        UpdateUnitPrices();
    }

    void Update()
    {
        // Check if the random number has changed
        if (unitShopScript.randomNumber != previousRandomNumber)
        {
            UpdateUnitPrices();
            previousRandomNumber = unitShopScript.randomNumber;
        }
    }

    void UpdateUnitPrices()
    {
        // Get the random number from Unit_Shop_Script
        int randomNumber = unitShopScript.randomNumber;

        // Check if the random number is within valid range
        if (randomNumber >= 0 && randomNumber < unitPrefabs.Length)
        {
            GameObject prefab = unitPrefabs[randomNumber];

            // Check if prefab is assigned
            if (prefab != null)
            {
                // Get the BaseUnit_Script component from the prefab
                BaseUnit_Script baseUnitScript = prefab.GetComponent<BaseUnit_Script>();

                // If BaseUnit_Script found, update the price display
                if (baseUnitScript != null)
                {
                    costText.text = $"Unit Cost: {baseUnitScript.unitCost}";
                }
                else
                {
                    costText.text = $"Unit Cost: ?";
                }
            }
            else
            {
                costText.text = $"Unit Cost: ?";
                Debug.LogWarning($"Unit prefab at index {randomNumber} is null.");
            }
        }
        else
        {
            costText.text = $"Unit Cost: ?";
            Debug.LogWarning($"Random number {randomNumber} is out of range for unitPrefabs array.");
        }
    }
}
