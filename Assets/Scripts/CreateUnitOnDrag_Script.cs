using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CreateUnitOnDrag_Script : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject unitPrefab;
    private Canvas canvas;
    public GridManager battleGrid;
    private Vector3 gridOffset = new Vector3(2.1244f, -3.1312f, 0f);
    private Vector2Int gridRange = new Vector2Int(4, 6);
    private Vector3 spawnBasePosition = Vector3.zero;

    private GameObject newUnit;
    public Unit_Inventory unit_Inventory;
     public int unitTypeIndex;


    void Start()
    {
        // Find the Canvas named "Battle_Setup_Canvas"
        canvas = GameObject.Find("Battle_Setup_Canvas").GetComponent<Canvas>();
          unit_Inventory = FindObjectOfType<Unit_Inventory>();

        if (canvas == null)
        {
            Debug.LogError("Canvas 'Battle_Setup_Canvas' not found!");
            return;
        }

    }

public void OnEndDrag(PointerEventData eventData)
{
    if (newUnit != null)
    {
        var test = newUnit.GetComponent<DragAndDrop>();
        if (test != null)
        {
            test.OnEndDrag(eventData);
        }
    }
}

public void OnDrag(PointerEventData eventData)
{
    if (newUnit != null)
    {
        var test = newUnit.GetComponent<DragAndDrop>();
        if (test != null)
        {
            test.OnDrag(eventData);
        }
    }
}


    public void OnBeginDrag(PointerEventData eventData)
    {
           if (unit_Inventory != null && unit_Inventory.unitInInventoryCount[unitTypeIndex] > 0)
        {
        float currentXPosition = -11;
        // Create a new unit GameObject
        Vector3 position = new Vector3(currentXPosition, 2f, spawnBasePosition.z);

        newUnit = Instantiate(unitPrefab, position, Quaternion.identity);

        // Set the parent of the new unit to the Canvas
        newUnit.transform.SetParent(canvas.transform, false);

        BaseUnit_Script unitScript = newUnit.GetComponent<BaseUnit_Script>();

        // Find the GridManager and set it to the Battle_Setup grid
        GridManager gridManager = GameObject.FindObjectOfType<GridManager>();
        if (gridManager == null)
        {
            Debug.LogError("GridManager not found!");
            return;
        }
        // Set the gridManager variable of the BaseUnit_Script to the Battle_Setup grid
        unitScript.gridManager = battleGrid;
       
        var dragAndDropScript = newUnit.GetComponent<DragAndDrop>();
        if (dragAndDropScript == null)
        {
            dragAndDropScript = newUnit.AddComponent<DragAndDrop>();
        }
        dragAndDropScript.gridManager = battleGrid;
        dragAndDropScript.gridOffset = gridOffset;
        dragAndDropScript.gridRange = gridRange;
        dragAndDropScript.unitTypeIndex = unitScript.unitID;

     
        var test = newUnit.GetComponent<DragAndDrop>();
        test.OnDrag(eventData);
        }
        else{
            return;
        }
    }
}
