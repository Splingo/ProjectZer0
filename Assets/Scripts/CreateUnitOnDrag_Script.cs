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

    void Start()
    {
        // Find the Canvas named "Battle_Setup_Canvas"
        canvas = GameObject.Find("Battle_Setup_Canvas").GetComponent<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("Canvas 'Battle_Setup_Canvas' not found!");
            return;
        }
    }

    public void OnDrag(PointerEventData eventData){
        var test = newUnit.GetComponent<DragAndDrop>();
         test.OnDrag(eventData);
    }

    public void OnEndDrag(PointerEventData eventData){

        var test = newUnit.GetComponent<DragAndDrop>();
         test.OnEndDrag(eventData);
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        float currentXPosition = -11;
        // Create a new unit GameObject
        Vector3 position = new Vector3(currentXPosition, 2f, spawnBasePosition.z);

        newUnit = Instantiate(unitPrefab,position, Quaternion.identity);

        // Set the parent of the new unit to the Canvas
        newUnit.transform.SetParent(canvas.transform, false);

        // Set the position of the new unit
       // newUnit.transform.position = transform.position;
        //3,2,0 - 6,2,0
        //3,-3,0 - 6,-3,0
  

        // Return the new position with z set to 0

        // Get the BaseUnit_Script component from the new unit
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

         var test = newUnit.GetComponent<DragAndDrop>();
         test.OnDrag(eventData);

    }
}