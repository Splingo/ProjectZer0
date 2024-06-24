using UnityEngine;
using TMPro;

public class Set_Building_Text : MonoBehaviour
{
    // Reference to the Building_Shop_Script to get the randomNumber
    public Building_Shop_Script shopScript;

    // Array of Building prefabs to check for BaseBuilding_Script
    public GameObject[] buildingPrefabs;
    public TextMeshProUGUI costText;
    private int previousRandomNumber = -1;

    void Start()
    {
        // Find the Building_Shop_Script in the scene if not assigned
        if (shopScript == null)
        {
            shopScript = FindObjectOfType<Building_Shop_Script>();
        }

        // Find the TextMeshProUGUI component on this GameObject if not assigned
        if (costText == null)
        {
            costText = GetComponent<TextMeshProUGUI>();
        }

        UpdateBuildingPrices();
    }

    void Update()
    {
        // Check if the random number has changed
        if (shopScript != null && shopScript.randomNumber != previousRandomNumber)
        {
            UpdateBuildingPrices();
            previousRandomNumber = shopScript.randomNumber;
        }
    }

    void UpdateBuildingPrices()
    {
        // Check if shopScript is assigned
        if (shopScript == null)
        {
            Debug.LogWarning("Building_Shop_Script not assigned.");
            return;
        }

        // Get the random number from Building_Shop_Script
        int randomNumber = shopScript.randomNumber;
    
        // Check if the random number is within valid range
        if (randomNumber >= 0 && randomNumber < buildingPrefabs.Length)
        {
            GameObject prefab = buildingPrefabs[randomNumber];

            // Check if prefab is assigned
            if (prefab != null)
            {
                // Get the Building_Class component from the prefab
                Building_Class buildingClass = prefab.GetComponent<Building_Class>();

                // If Building_Class found, update the price display
                if (buildingClass != null)
                {
                    costText.text = $"Building Cost: {buildingClass.value}";
                }
                else
                {
                    costText.text = $"Building Cost: ?";
                }
            }
            else
            {
                costText.text = $"Building Cost: ?";
                Debug.LogWarning($"Building prefab at index {randomNumber} is null.");
            }
        }
        else
        {
            costText.text = $"Building Cost: ?";
            Debug.LogWarning($"Random number {randomNumber} is out of range for BuildingPrefabs array.");
        }
    }
}
