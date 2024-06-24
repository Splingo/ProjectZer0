using UnityEngine;
using UnityEngine.UI;

public class building_Inventory : MonoBehaviour
{
    public int[] buildingOnFieldCount;
    public int[] buildingInInventoryCount;
    public GameObject[] buildingGameObjects; // Changed to public for easy inspector assignment

    void Start()
    {
        // Initialize the arrays with 12 elements, each set to 0
        buildingOnFieldCount = new int[8];
        buildingInInventoryCount = new int[8];

        // Ensure buildingGameObjects is initialized if not assigned via the Inspector
        if (buildingGameObjects == null || buildingGameObjects.Length == 0)
        {
            buildingGameObjects = new GameObject[8]; // Array to store game objects
        }

        // Initial check and update on start
        CheckAndUpdateGameObjectSaturation();
    }

    // Method to add a building to the inventory
    public void AddbuildingToInventory(int index)
    {
        buildingInInventoryCount[index]++;
        CheckAndUpdateGameObjectSaturation();
    }

    public void AddbuildingToField(int index)
    {
        buildingOnFieldCount[index]++;
    }

    // Method to remove a building from the inventory
    public void RemovebuildingFromInventory(int index)
    {
        if (buildingInInventoryCount[index] > 0)
        {
            buildingInInventoryCount[index]--;
            CheckAndUpdateGameObjectSaturation();
        }
    }

    public void RemovebuildingFromField(int index)
    {
        buildingOnFieldCount[index]--;
    }

    // Method to check and update the saturation of GameObjects based on inventory count
    private void CheckAndUpdateGameObjectSaturation()
    {
        for (int i = 0; i < buildingGameObjects.Length; i++)
        {
            if (buildingGameObjects[i] != null)
            {
                Image imageComponent = buildingGameObjects[i].GetComponent<Image>();
                if (imageComponent != null)
                {
                    if (buildingInInventoryCount[i] == 0)
                    {
                          Color darkGray = new Color(0.3f, 0.3f, 0.3f); // Darker gray
                    imageComponent.color = darkGray;
                    }
                    else
                    {
                        // Reset saturation to 1 for original color
                        imageComponent.color = Color.white;
                    }
                }
                else
                {
                    Debug.LogWarning("No Image component found on " + buildingGameObjects[i].name);
                }
            }
            else
            {
                Debug.LogWarning("buildingGameObjects[" + i + "] is null.");
            }
        }
    }

    // Optional: Method to set the GameObject array, useful for setting up buildingGameObjects
    public void SetbuildingGameObject(int index, GameObject buildingGameObject)
    {
        if (index >= 0 && index < buildingGameObjects.Length)
        {
            buildingGameObjects[index] = buildingGameObject;
        }
        else
        {
            Debug.LogError("Index out of bounds when setting building game object.");
        }
    }
}
