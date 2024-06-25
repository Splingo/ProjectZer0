using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit_Shop_Script : MonoBehaviour
{
    [SerializeField]
    private GameObject targetObject;

    public Unit_Inventory unit_Inventory;
    public int randomNumber;
    public Sprite[] sprites;
    public int rerollCost = 0;

    public bool raiseCost = false;

    private CityManager cityManager;

    void Start()
    {
        cityManager = FindObjectOfType<CityManager>();
        if (cityManager == null)
        {
            Debug.LogError("CityManager not found in the scene!");
            return;
        }

        unit_Inventory = FindObjectOfType<Unit_Inventory>();
        setRandomUnit(raiseCost);
    }

    void Update()
    {
        // Optional: Add any relevant update logic if needed
    }

    public void setRandomUnit(bool setter)
    {
        if (cityManager == null)
        {
            Debug.LogError("CityManager is null!");
            return;
        }

        if (cityManager.CanAffordReroll(rerollCost))
        {
            List<int> enemyIndices = new List<int> { 0, 1, 3, 4, 5 };

            randomNumber = enemyIndices[Random.Range(0, enemyIndices.Count)];
            Sprite newSprite = sprites[randomNumber];
            targetObject.GetComponent<Image>().sprite = newSprite;
            if (setter)
            {
                cityManager.DeductRerollCost(rerollCost);
            }

            rerollCost++;
        }
    }

    public void getRandomUnit(int unitPrefix)
    {
        if (cityManager == null)
        {
            Debug.LogError("CityManager is null!");
            return;
        }

        int unitValue = GetUnitValue(randomNumber);

        if (cityManager.CanAffordReroll(unitValue))
        {
            cityManager.DeductRerollCost(unitValue);
            unit_Inventory.AddUnitToInventory(randomNumber);
            setRandomUnit(false);
        }
    }

    private int GetUnitValue(int index)
    {
        int[] unitValues = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
        if (index >= 0 && index < unitValues.Length)
        {
            return unitValues[index];
        }
        return 0;
    }
}
