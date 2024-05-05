using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Inventory : MonoBehaviour
{
    public unit_array;
    BaseUnit_Script baseUnitRank_1;
    friendly_ranged rangedRank_1;
    // Start is called before the first frame update
    void Start()
    {
        baseUnitRank_1;
        rangedRank_1;
        unit_array = [baseUnitRank_1,rangedRank_1,0,0,0,0,0,0,0,0,0,0] as unit_array;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
