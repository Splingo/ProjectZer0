using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Inventory : MonoBehaviour
{
    public int[] unitOnFieldCount;
    public int[] unitInInventoryCount;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the arrays with 12 elements, each set to 0
        unitOnFieldCount = new int[12];
        unitInInventoryCount = new int[12];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddUnit(int index)
    {
        // Increment unit count in inventory
        unitInInventoryCount[index]++;
        
        // Increment unit count on field
        unitOnFieldCount[index]++;
    }

    public void RemoveUnit(int index)
    {
        // Decrement unit count in inventory if it's greater than 0
        if (unitInInventoryCount[index] > 0)
        {
            unitInInventoryCount[index]--;
        }
        else
        {
            return;
        }
        
        // Decrement unit count on field if it's greater than 0
        if (unitOnFieldCount[index] > 0)
        {
            unitOnFieldCount[index]--;
        }
        else
        {
            return;
        }
    }
}
