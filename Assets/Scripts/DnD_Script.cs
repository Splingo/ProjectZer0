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
        // Speichere die Zellenposition, in der sich das Objekt zu Beginn befindet
        initialCellPosition = gridManager.gridTilemap.WorldToCell(transform.position);

        // Entferne die Zelle, in der sich das Objekt zu Beginn befand, aus der besetzten Liste
        gridManager.ReleaseCells(new List<Vector3Int> { new Vector3Int(initialCellPosition.x, initialCellPosition.y,0) });
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
            if (gridManager.AreCellsOccupied(new List<Vector3Int> { cellPosition }))
            {
                dropPosition = previousPosition;
                transform.position = previousPosition;
            }
        }
    }

public void OnEndDrag(PointerEventData eventData)
{
    Vector3 dropPosition = transform.position;
    if (IsWithinAllowedRange(dropPosition))
    {
        Vector3Int cellPosition = gridManager.gridTilemap.WorldToCell(dropPosition);

        if (!gridManager.AreCellsOccupied(new List<Vector3Int> { cellPosition }))
        {
            Vector3Int initialCellPosition = gridManager.gridTilemap.WorldToCell(initialPosition);
            gridManager.ReleaseCells(new List<Vector3Int> { new Vector3Int(initialCellPosition.x, initialCellPosition.y, 0) });
            List<Vector3Int> convertedCells = new List<Vector3Int>();
            if (draggedBuilding != null)
            {
                List<Vector3Int> buildingOccupiedCells = draggedBuilding.GetOccupiedCells(initialCellPosition);
                    if(buildingOccupiedCells != null){

                foreach (Vector3Int cell in buildingOccupiedCells)
                {
                    convertedCells.Add(cell);
                }
                    }

                gridManager.OccupyCells(convertedCells);
            }

            Vector3 cellCenter = gridManager.gridTilemap.GetCellCenterWorld(cellPosition);
            transform.position = cellCenter; // Snappen an die Zellenposition

            return;
        }
    }

    transform.position = startPosition; // Setze zurück zur ursprünglichen Position
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
