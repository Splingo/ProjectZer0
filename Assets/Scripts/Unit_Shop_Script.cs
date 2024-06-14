using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Unit_Shop_Script : MonoBehaviour
{

    [SerializeField]
    private GameObject targetObject;
    // Start is called before the first frame update
    public Unit_Inventory unit_Inventory;
    public int randomNumber;
    public Sprite[] sprites;
    public int rerollCost = 0;
    public void setRandomUnit()
    {
        randomNumber = Random.Range(0, 12);
        Sprite newSprite = sprites[randomNumber]; 
            targetObject.GetComponent<Image>().sprite = newSprite;
        rerollCost++;
       
    }

    public void getRandomUnit(int unitPrefix)
    {
        unit_Inventory.AddUnitToInventory(randomNumber);

    }
    void Start()
    {

        unit_Inventory = FindObjectOfType<Unit_Inventory>();
       setRandomUnit();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
