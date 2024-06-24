 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Create_Building_OnDrag_Script : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
  
    public GameObject buildingPrefab;
    private Canvas canvas;
    public GridManager cityGrid;
    private Vector3 gridOffset = new Vector3(2.1244f, -3.1312f, 0f);
    private Vector2Int gridRange = new Vector2Int(10, 10);
    private Vector3 spawnBasePosition = Vector3.zero;

    private GameObject newBuilding;
    public building_Inventory building_Inventory;
    public int buildingIndex;


    void Start()
    {
        // Find the Canvas named "Battle_Setup_Canvas"
        canvas = GameObject.Find("City_Canvas").GetComponent<Canvas>();
        building_Inventory buildingInventory = FindObjectOfType<building_Inventory>();

        if (canvas == null)
        {
            Debug.LogError("Canvas 'City_Canvas' not found!");
            return;
        }

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (building_Inventory != null && building_Inventory.buildingInInventoryCount[buildingIndex] >= 0)
        {
            if (newBuilding != null)
            {
                var test = newBuilding.GetComponent<DragAndDropBuilding>();
                if (test != null)
                {
                    test.OnEndDrag(eventData);
                }
                if(building_Inventory.buildingInInventoryCount[buildingIndex] == 0){
                    this.enabled = false;
                }
                else{
                    this.enabled = true; 
                }
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {

        if (building_Inventory != null && building_Inventory.buildingInInventoryCount[buildingIndex] >= 0)
        {
            if (newBuilding != null)
            {
                var test = newBuilding.GetComponent<DragAndDropBuilding>();
                if (test != null)
                {
                    test.OnDrag(eventData);
                }
            }
        }
    }
    
    public void OnBeginDrag(PointerEventData eventData)
{

    if (building_Inventory != null && building_Inventory.buildingInInventoryCount[buildingIndex] >= 1)
    {
        // Konvertiere die Mausposition in Weltkoordinaten
        RectTransformUtility.ScreenPointToWorldPointInRectangle(canvas.transform as RectTransform, eventData.position, canvas.worldCamera, out Vector3 worldPos);

        // Erstellen Sie die newBuilding an der konvertierten Mausposition
        newBuilding = Instantiate(buildingPrefab, worldPos, Quaternion.identity);

        // Setzen Sie den Eltern des neuen Einheitsobjekts auf die Canvas
        newBuilding.transform.SetParent(canvas.transform, false);

        Building_Class buildingScript = newBuilding.GetComponent<Building_Class>();

        // Set the gridManager variable of the Basebuilding_Script to the Battle_Setup grid
        buildingScript.gridManager = cityGrid;

        var dragAndDropScript = newBuilding.GetComponent<DragAndDropBuilding>();
        if (dragAndDropScript == null)
        {
            dragAndDropScript = newBuilding.AddComponent<DragAndDropBuilding>();
        }
        dragAndDropScript.gridManager = cityGrid;
        dragAndDropScript.gridOffset = gridOffset;
        dragAndDropScript.gridRange = gridRange;
        dragAndDropScript.buildingIndex = buildingScript.buildingID;
         int prefabbuildingIndex = dragAndDropScript.buildingIndex;

        // Entfernen der Einheit aus dem Inventar unter Verwendung des abgerufenen buildingIndex
        building_Inventory.RemovebuildingFromInventory(prefabbuildingIndex);
        // Weiterleiten des BeginDrag-Ereignisses an das DragAndDropBuilding-Komponente der neuen Einheit
        var test = newBuilding.GetComponent<DragAndDropBuilding>();
        test.OnBeginDrag(eventData);
    }
}


}

