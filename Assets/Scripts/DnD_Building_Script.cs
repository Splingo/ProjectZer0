using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using UnityEngine.UI;

public class DragAndDropBuilding : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 initialPosition;
    private Vector3 startPosition;
    private Vector3 previousPosition;
    private Canvas canvas;
    public GridManager gridManager;
    public Vector3 gridOffset; // Offset basierend auf der Grid-Position
    public Vector2Int gridRange; // Bereich des Rasters, in dem das Objekt platziert werden kann
    private Vector3Int initialCellPosition;
    private Building_Class draggedBuilding;
    private GameObject copyObject;
    private bool freshSpawn = true;
    public int buildingIndex;
    public building_Inventory building_Inventory;

    private void Awake()
    {
        initialPosition = transform.position; // Speichere die ursprüngliche Position bei Start/Awake
        startPosition = initialPosition;
        draggedBuilding = GetComponent<Building_Class>();
    }

    void Start()
    {
        canvas = FindObjectOfType<Canvas>(); // Finde die Canvas im Spiel
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
        initialCellPosition = gridManager.gridTilemap.WorldToCell(transform.position);
        // Erhalten Sie die belegten Zellen für das aktuelle Element
        if (draggedBuilding != null)
        {
            List<Vector3Int> occupiedCells = draggedBuilding.GetOccupiedCells(initialCellPosition);
            draggedBuilding.previousOccupiedCells = occupiedCells;
            if (occupiedCells != null && occupiedCells.Count > 0)
            {
                gridManager.ReleaseCells(occupiedCells);
            }
        }

        previousPosition = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
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

            if (draggedBuilding != null)
            {
                List<Vector3Int> buildingOccupiedCells = draggedBuilding.ReturnOccupiedCells();
                draggedBuilding.hoverungOccupiedCells = draggedBuilding.GetOccupiedCells(cellPosition);
            }
        }
    }

  public void OnEndDrag(PointerEventData eventData)
    {
        Vector3 dropPosition = transform.position;
        Vector3Int cellPosition = gridManager.gridTilemap.WorldToCell(dropPosition);

        if (IsWithinAllowedRange(dropPosition))
        {
            if (draggedBuilding != null)
            {
                List<Vector3Int> newOccupiedCells = draggedBuilding.GetOccupiedCells(cellPosition);

                if (!gridManager.AreCellsOccupied(newOccupiedCells))
                {
                    // Release the previous occupied cells
                    gridManager.ReleaseCells(draggedBuilding.previousOccupiedCells);

                    // Occupy the new cells
                    gridManager.OccupyCells(newOccupiedCells);

                    // Snap to cell center
                    Vector3 cellCenter = gridManager.gridTilemap.GetCellCenterWorld(cellPosition);
                    transform.position = cellCenter;

                    if (freshSpawn)
                    {
                        GameObject rerollButtonObject = GameObject.Find("Building_Reroll_Button");
                        
                        building_Inventory buildingInventory = FindObjectOfType<building_Inventory>();
                        if (buildingInventory != null)
                        {
                            buildingInventory.AddbuildingToField(draggedBuilding.buildingID);
                        }
                        else
                        {
                            Debug.Log("building_Inventory nicht gefunden!");
                        }

                        
                    }
                    
                        gameObject.name = gameObject.name.Replace("_Shop", "");
                    

                    freshSpawn = false;
                    return; // Exit early if placement is successful
                }
                else
                {
                    // Re-occupy previous cells if placement was not successful
                    SnapBackToPreviousPosition();
                }
            }
        }
        else
        {
            // Re-occupy previous cells if placement was not successful
            SnapBackToPreviousPosition();
                     
        }
    }
    private void SnapBackToPreviousPosition()
    {
        if (draggedBuilding != null && draggedBuilding.previousOccupiedCells.Count > 0)
        {
            // Snap back to the first previous occupied cell's center position
            Vector3Int previousCellPosition = draggedBuilding.previousOccupiedCells[0];
            Vector3 previousCellCenter = gridManager.gridTilemap.GetCellCenterWorld(previousCellPosition);
            transform.position = previousCellCenter;

            // Re-occupy previous cells
            gridManager.OccupyCells(draggedBuilding.previousOccupiedCells);
        }
    }




    private bool IsWithinAllowedRange(Vector3 position)
    {
        Vector3Int cellPosition = gridManager.gridTilemap.WorldToCell(position);

        // Stelle sicher, dass die Position relativ zum Grid-Offset ist
        Vector3 relativeCellPosition = cellPosition - (Vector3)gridOffset;

        // Überprüfe, ob die Zellenposition innerhalb des erlaubten Bereichs liegt
        return relativeCellPosition.x >= 0 && relativeCellPosition.x < gridRange.x &&
               relativeCellPosition.y >= 0 && relativeCellPosition.y < gridRange.y;
    }
}
