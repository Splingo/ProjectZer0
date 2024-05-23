using UnityEngine;

public class Unit_Inventory : MonoBehaviour
{
    public int[] unitOnFieldCount;
    public int[] unitInInventoryCount;
    private GameObject[] unitGameObjects;

    void Start()
    {
        // Initialize the arrays with 12 elements, each set to 0
        unitOnFieldCount = new int[12];
        unitInInventoryCount = new int[12];
        unitGameObjects = new GameObject[12]; // Array to store game objects
    }

    // Method to add a unit to the inventory
    public void AddUnitToInventory(int index)
    {
        unitInInventoryCount[index]++;
    }
    public void AddUnitToField(int index){
        unitOnFieldCount[index]++;
    }

    // Method to remove a unit from the inventory
    public void RemoveUnitFromInventory(int index)
    {
        if(unitInInventoryCount[index] >0){
        unitInInventoryCount[index]--;
        }
    }
    
    public void RemoveUnitFromField(int index)
    {
        unitOnFieldCount[index]--;
    }

    // Method to find the index of a unit in the inventory
    private int FindUnitIndex(GameObject unitGameObject)
    {
        for (int i = 0; i < unitGameObjects.Length; i++)
        {
            if (unitGameObjects[i] == unitGameObject)
            {
                return i; // Return the index if found
            }
        }
        return -1; // Return -1 if not found
    }
}
