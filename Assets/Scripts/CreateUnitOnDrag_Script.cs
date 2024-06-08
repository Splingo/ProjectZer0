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
        if (unit_Inventory != null && unit_Inventory.unitInInventoryCount[unitTypeIndex] >= 0)
        {
            if (newUnit != null)
            {
                var test = newUnit.GetComponent<DragAndDrop>();
                if (test != null)
                {
                    test.OnEndDrag(eventData);
                }
                if(unit_Inventory.unitInInventoryCount[unitTypeIndex] == 0){
                    this.enabled = false;
                }
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {

        if (unit_Inventory != null && unit_Inventory.unitInInventoryCount[unitTypeIndex] >= 0)
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
    }
    
    public void OnBeginDrag(PointerEventData eventData)
{

    if (unit_Inventory != null && unit_Inventory.unitInInventoryCount[unitTypeIndex] >= 1)
    {
        // Konvertiere die Mausposition in Weltkoordinaten
        RectTransformUtility.ScreenPointToWorldPointInRectangle(canvas.transform as RectTransform, eventData.position, canvas.worldCamera, out Vector3 worldPos);

        // Erstellen Sie die newUnit an der konvertierten Mausposition
        newUnit = Instantiate(unitPrefab, worldPos, Quaternion.identity);

        // Setzen Sie den Eltern des neuen Einheitsobjekts auf die Canvas
        newUnit.transform.SetParent(canvas.transform, false);

        BaseUnit_Script unitScript = newUnit.GetComponent<BaseUnit_Script>();

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
         int prefabUnitTypeIndex = dragAndDropScript.unitTypeIndex;

        // Entfernen der Einheit aus dem Inventar unter Verwendung des abgerufenen unitTypeIndex
        unit_Inventory.RemoveUnitFromInventory(prefabUnitTypeIndex);
        // Weiterleiten des BeginDrag-Ereignisses an das DragAndDrop-Komponente der neuen Einheit
        var test = newUnit.GetComponent<DragAndDrop>();
        test.OnBeginDrag(eventData);
    }
}


}
