using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class DragAndDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 initialPosition;
    private Vector3 startPosition;
    private Vector3 previousPosition;
    private Canvas canvas;
    public GridManager gridManager;
    public Vector3 gridOffset; // Offset basierend auf der Grid-Position
    public Vector2Int gridRange; // Bereich des Rasters, in dem das Objekt platziert werden kann
    private Vector3Int initialCellPosition;
    private BaseUnit_Script baseUnit;
    private friendly_ranged rangedUnit;

    public Unit_Inventory unit_Inventory;

    private GameObject copyObject;

    public int unitTypeIndex;



    private void Awake()
    {
        initialPosition = transform.position; // Speichere die ursprüngliche Position bei Start/Awake
        startPosition = initialPosition;
        baseUnit = GetComponent<BaseUnit_Script>();
        rangedUnit = GetComponent<friendly_ranged>();

    }

    void Start()
    {
        canvas = FindObjectOfType<Canvas>(); // Finde die Canvas im Spiel
        unit_Inventory = FindObjectOfType<Unit_Inventory>();
        if (gridManager != null)
        {
            // Nehme die Position des Grids als Offset
            gridOffset = gridManager.transform.position;
            // Definiere den Bereich des Rasters, in dem das Objekt platziert werden kann
            gridRange = new Vector2Int(gridManager.rows, gridManager.columns);
        }
        else
        {
            Debug.LogError("GridManager nicht gefunden!");
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (unit_Inventory != null)
        {

            if (unit_Inventory.unitInInventoryCount[unitTypeIndex] >= 0 || unit_Inventory.unitOnFieldCount[unitTypeIndex] > 0)
            {
                
                initialCellPosition = gridManager.gridTilemap.WorldToCell(transform.position);
                // Erhalten Sie die belegten Zellen für das aktuelle Element

                if (baseUnit != null)
                {
                    baseUnit.previousOccupiedCells = baseUnit.GetOccupiedCells(initialCellPosition);
                }
                if (rangedUnit != null)
                {
                    rangedUnit.previousOccupiedCells = baseUnit.GetOccupiedCells(initialCellPosition);

                }

                previousPosition = transform.position;
            }

        }
    }





    public void OnDrag(PointerEventData eventData)
    {
        if (unit_Inventory != null)
        {

            if (unit_Inventory.unitInInventoryCount[unitTypeIndex] >= 0 || unit_Inventory.unitOnFieldCount[unitTypeIndex] > 0)
            {
                Vector3 mousePos = Input.mousePosition;
                mousePos.z = 10; // Entfernung der Canvas-Ebene
                Vector3 screenPos = Camera.main.ScreenToWorldPoint(mousePos);
                transform.position = screenPos; // Bewege das Originalobjekt

                Vector3 dropPosition = transform.position;

                if (IsWithinAllowedRange(dropPosition))
                {
                    Vector3Int cellPosition = gridManager.gridTilemap.WorldToCell(dropPosition);
                    Vector3 cellCenter = gridManager.gridTilemap.GetCellCenterWorld(cellPosition);
                    // Snappen an die Zellenposition
                    if (copyObject != null)
                    {
                        copyObject.transform.position = cellCenter;
                    }
                    else
                    {
                        transform.position = cellCenter; // Snappen an die Zellenposition für das Originalobjekt
                    }


                    if (baseUnit != null)
                    {
                        baseUnit.hoveringOccupiedCells = baseUnit.GetOccupiedCells(cellPosition);
                    }
                    if (rangedUnit != null)
                    {

                        rangedUnit.hoveringOccupiedCells = rangedUnit.GetOccupiedCells(cellPosition);
                    }
                }
            }
        }
    }



    public void OnEndDrag(PointerEventData eventData)
    {
        if (unit_Inventory != null)
        {

            if (unit_Inventory.unitInInventoryCount[unitTypeIndex] >= 0 || unit_Inventory.unitOnFieldCount[unitTypeIndex] > 0)
            {
                Vector3 dropPosition = transform.position;
                Vector3Int cellPosition = gridManager.gridTilemap.WorldToCell(dropPosition);
                if (IsWithinAllowedRange(dropPosition))
                {

                    if (rangedUnit != null)
                    {
                        List<Vector3Int> newOccupiedCells = rangedUnit.GetOccupiedCells(cellPosition);

                        if (!gridManager.AreCellsOccupied(newOccupiedCells))
                        {

                            // Release the previous occupied cells
                            gridManager.ReleaseCells(rangedUnit.previousOccupiedCells);

                            // Occupy the new cells
                            gridManager.OccupyCells(newOccupiedCells);

                            // Snap to cell center
                            Vector3 cellCenter = gridManager.gridTilemap.GetCellCenterWorld(cellPosition);
                            transform.position = cellCenter;
                            

                            if (rangedUnit.onField == false){

                                unit_Inventory.AddUnitToField(1);
                            }

                            rangedUnit.onField = true;


                            return; // Exit early if placement is successful
                        }
                    }
                    else if (baseUnit != null)
                    {
                        List<Vector3Int> newOccupiedCells = baseUnit.GetOccupiedCells(cellPosition);

                        if (!gridManager.AreCellsOccupied(newOccupiedCells))
                        {

                            // Release the previous occupied cells
                            gridManager.ReleaseCells(baseUnit.previousOccupiedCells);

                            // Occupy the new cells
                            gridManager.OccupyCells(newOccupiedCells);

                            // Snap to cell center
                            Vector3 cellCenter = gridManager.gridTilemap.GetCellCenterWorld(cellPosition);
                            transform.position = cellCenter;

                            if (baseUnit.onField == false){
                            //unit_Inventory.RemoveUnitFromInventory(0);
                                unit_Inventory.AddUnitToField(0);

                            }

                            baseUnit.onField = true;
                            return; // Exit early if placement is successful
                        }
                    }
                    if(unit_Inventory.unitInInventoryCount[unitTypeIndex] == 0){
                            
                    }
                }



                if (rangedUnit != null)
                {

                    List<Vector3Int> occupiedBattleCells = rangedUnit.GetOccupiedCells(initialCellPosition);
                    rangedUnit.previousOccupiedCells = occupiedBattleCells;

                    // Log the state of gridManager before releasing cells

                    if (occupiedBattleCells != null && occupiedBattleCells.Count > 0)
                    {

                        gridManager.ReleaseCells(rangedUnit.previousOccupiedCells);
                    }

                    // Log the state of gridManager after releasing cells
                    unit_Inventory.AddUnitToInventory(1);
                    unit_Inventory.RemoveUnitFromField(1);
                     GameObject unit1 = GameObject.Find("Unit_1");
                   CreateUnitOnDrag_Script createUnitOnDragScript = unit1.GetComponent<CreateUnitOnDrag_Script>();
                    createUnitOnDragScript.enabled = true;


                    Destroy(gameObject);
                }
                else if (baseUnit != null)
                {

                    List<Vector3Int> occupiedBattleCells = baseUnit.GetOccupiedCells(initialCellPosition);
                    baseUnit.previousOccupiedCells = occupiedBattleCells;
                    if (occupiedBattleCells != null && occupiedBattleCells.Count > 0)
                    {
                        gridManager.ReleaseCells(baseUnit.previousOccupiedCells);
                    }
                    unit_Inventory.AddUnitToInventory(0);
                    unit_Inventory.RemoveUnitFromField(0);
                     GameObject unit1 = GameObject.Find("Unit_0");
                   CreateUnitOnDrag_Script createUnitOnDragScript = unit1.GetComponent<CreateUnitOnDrag_Script>();
                    createUnitOnDragScript.enabled = true;

                    Destroy(gameObject);
                }

            }
        }
    }



    private bool IsWithinAllowedRange(Vector3 position)
    {
        Vector3 cellPosition = gridManager.gridTilemap.WorldToCell(position);

        // Stelle sicher, dass die Position relativ zum Grid-Offset ist
        cellPosition -= gridOffset;

        // Überprüfe, ob die Zellenposition innerhalb des erlaubten Bereichs liegt
        return cellPosition.x >= 0 && cellPosition.x < gridRange.x &&
               cellPosition.y >= 0 && cellPosition.y < gridRange.y;
    }

}


