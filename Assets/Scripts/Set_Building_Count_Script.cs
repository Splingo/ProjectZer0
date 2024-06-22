using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using TMPro;

public class Set_Building_Count_Script : MonoBehaviour
{
    // Start is called before the first frame update
    public building_Inventory building_Inventory;
    public TextMeshProUGUI buildingCount;
    public int buildingID;


    void Start()
    {
        building_Inventory = FindObjectOfType<building_Inventory>();
        
    }

    // Update is called once per frame
    void Update()
    {
        buildingCount.text = building_Inventory.buildingInInventoryCount[buildingID].ToString();

    }
}
