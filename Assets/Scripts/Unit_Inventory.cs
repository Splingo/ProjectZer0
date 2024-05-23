using UnityEngine;
using UnityEngine.UI;

public class Unit_Inventory : MonoBehaviour
{
    public int[] unitOnFieldCount;
    public int[] unitInInventoryCount;
    public GameObject[] unitGameObjects; // Changed to public for easy inspector assignment

    void Start()
    {
        // Initialize the arrays with 12 elements, each set to 0
        unitOnFieldCount = new int[12];
        unitInInventoryCount = new int[12];

        // Ensure unitGameObjects is initialized if not assigned via the Inspector
        if (unitGameObjects == null || unitGameObjects.Length == 0)
        {
            unitGameObjects = new GameObject[12]; // Array to store game objects
        }

        // Initial check and update on start
        CheckAndUpdateGameObjectSaturation();
    }

    // Method to add a unit to the inventory
    public void AddUnitToInventory(int index)
    {
        unitInInventoryCount[index]++;
        CheckAndUpdateGameObjectSaturation();
    }

    public void AddUnitToField(int index)
    {
        unitOnFieldCount[index]++;
    }

    // Method to remove a unit from the inventory
    public void RemoveUnitFromInventory(int index)
    {
        if (unitInInventoryCount[index] > 0)
        {
            unitInInventoryCount[index]--;
            CheckAndUpdateGameObjectSaturation();
        }
    }

    public void RemoveUnitFromField(int index)
    {
        unitOnFieldCount[index]--;
    }

    // Method to check and update the saturation of GameObjects based on inventory count
    private void CheckAndUpdateGameObjectSaturation()
    {
        for (int i = 0; i < unitGameObjects.Length; i++)
        {
            if (unitGameObjects[i] != null)
            {
                Image imageComponent = unitGameObjects[i].GetComponent<Image>();
                if (imageComponent != null)
                {
                    if (unitInInventoryCount[i] == 0)
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
                    Debug.LogWarning("No Image component found on " + unitGameObjects[i].name);
                }
            }
            else
            {
                Debug.LogWarning("unitGameObjects[" + i + "] is null.");
            }
        }
    }

    // Optional: Method to set the GameObject array, useful for setting up unitGameObjects
    public void SetUnitGameObject(int index, GameObject unitGameObject)
    {
        if (index >= 0 && index < unitGameObjects.Length)
        {
            unitGameObjects[index] = unitGameObject;
        }
        else
        {
            Debug.LogError("Index out of bounds when setting unit game object.");
        }
    }
}
