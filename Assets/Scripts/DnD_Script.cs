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
    private Building_Class draggedBuilding;
private BaseUnit_Script baseUnit;
    private void Awake()
    {
        initialPosition = transform.position; // Speichere die ursprüngliche Position bei Start/Awake
        startPosition = initialPosition;
        draggedBuilding = GetComponent<Building_Class>(); 
        baseUnit = GetComponent<BaseUnit_Script>();
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
    // Speichere die Zellenposition, in der sich das Objekt zu Beginn befindet
    initialCellPosition = gridManager.gridTilemap.WorldToCell(transform.position);
    // Erhalte die belegten Zellen für das aktuelle Element
    if(draggedBuilding != null){
        List<Vector3Int> occupiedCells = draggedBuilding.GetOccupiedCells(initialCellPosition );
        draggedBuilding.previousOccupiedCells = occupiedCells;
        if (occupiedCells != null && occupiedCells.Count > 0)
        {
            gridManager.ReleaseCells(occupiedCells);
        }
    }
    if( baseUnit!= null){
        List<Vector3Int> occupiedBattleCells = baseUnit.GetOccupiedCells(initialCellPosition);
        baseUnit.previousOccupiedCells = occupiedBattleCells;
        if (occupiedBattleCells != null && occupiedBattleCells.Count > 0)
        {
            gridManager.ReleaseCells(occupiedBattleCells);
        }
    }
    // Entferne die Zellen, die durch das Element besetzt sind

    previousPosition = transform.position;
}


public void OnDrag(PointerEventData eventData)
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10; // Entfernung der Canvas-Ebene
        Vector3 screenPos = Camera.main.ScreenToWorldPoint(mousePos);
        transform.position = screenPos;

        Vector3 dropPosition = transform.position;

        if (IsWithinAllowedRange(dropPosition))
        {
            Vector3Int cellPosition = gridManager.gridTilemap.WorldToCell(dropPosition);
                Vector3 cellCenter = gridManager.gridTilemap.GetCellCenterWorld(cellPosition);
                transform.position = cellCenter; // Snappen an die Zellenposition
            if(draggedBuilding != null){
            List<Vector3Int> buildingOccupiedCells = draggedBuilding.ReturnOccupiedCells();    
                draggedBuilding.hoverungOccupiedCells = draggedBuilding.GetOccupiedCells(cellPosition);
            }
            if( baseUnit!= null){
            List<Vector3Int> UnitOccupiedCells = baseUnit.ReturnOccupiedCells();    
                baseUnit.hoveringOccupiedCells = baseUnit.GetOccupiedCells(cellPosition);
            }
        }
    }


public void OnEndDrag(PointerEventData eventData)
{
    Vector3 dropPosition = transform.position;
    if (IsWithinAllowedRange(dropPosition))
    {
        Vector3Int cellPosition = gridManager.gridTilemap.WorldToCell(dropPosition);
        if(draggedBuilding != null){

            if (!gridManager.AreCellsOccupied(draggedBuilding.hoverungOccupiedCells))
            {
                // Erhalte die neuen belegten Zellen für das gezogene Gebäude
                List<Vector3Int> buildingOccupiedCells = draggedBuilding.GetOccupiedCells(cellPosition);

                // Aktualisiere die GridManager-Liste mit den neuen belegten Zellen
                gridManager.OccupyCells(buildingOccupiedCells);

                // Snappen an die Zellenposition
                Vector3 cellCenter = gridManager.gridTilemap.GetCellCenterWorld(cellPosition);
                transform.position = cellCenter;

                return;
            }
        } 
        if(baseUnit != null){

        if (!gridManager.AreCellsOccupied(baseUnit.hoveringOccupiedCells))
            {
                // Erhalte die neuen belegten Zellen für das gezogene Gebäude
                List<Vector3Int> UnitOccupiedCells = baseUnit.GetOccupiedCells(cellPosition);

                // Aktualisiere die GridManager-Liste mit den neuen belegten Zellen
                gridManager.OccupyCells(UnitOccupiedCells);

                // Snappen an die Zellenposition
                Vector3 cellCenter = gridManager.gridTilemap.GetCellCenterWorld(cellPosition);
                transform.position = cellCenter;

                return;
            }
        }
    }

    if(draggedBuilding != null){
    gridManager.OccupyCells(draggedBuilding.previousOccupiedCells);
    }

    if(baseUnit != null){
    gridManager.OccupyCells(baseUnit.previousOccupiedCells);
    }
        
   // transform.position = previousPosition; // Setze zurück zur ursprünglichen Position
    if(!IsWithinAllowedRange(dropPosition)){
        if(draggedBuilding != null){
            gridManager.ReleaseCells(draggedBuilding.previousOccupiedCells);
        }
        if(baseUnit != null){
            gridManager.ReleaseCells(baseUnit.previousOccupiedCells);
        }

    transform.position = initialPosition;
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
