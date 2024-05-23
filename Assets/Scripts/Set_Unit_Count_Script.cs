using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using TMPro;

public class Set_Unit_Count_Script : MonoBehaviour
{
    // Start is called before the first frame update
    public Unit_Inventory unit_Inventory;
    public TextMeshProUGUI unitCount;
    public int unitCountIndex;


    void Start()
    {
        unit_Inventory = FindObjectOfType<Unit_Inventory>();
        
    }

    // Update is called once per frame
    void Update()
    {
        unitCount.text = unit_Inventory.unitInInventoryCount[unitCountIndex].ToString();

    }
}
