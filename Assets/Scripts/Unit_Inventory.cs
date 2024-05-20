using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
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

    public void AddUnit(){
    
        for (int i = 0; i < unitInInventoryCount.Length; i++)
        {
            unitInInventoryCount[i] ++;
        }
    }
}
