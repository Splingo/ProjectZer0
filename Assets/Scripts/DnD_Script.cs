using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class DragAndDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector2 initialPosition;
    private Vector3 startPosition;
    private Vector3 previousPosition;
    private RectTransform rectTransform;
    private Canvas canvas;
    public Grid grid;
    public Vector3 gridOffset; // Offset basierend auf der Grid-Position
    public Vector2Int gridRange; // Bereich des Rasters, in dem das Objekt platziert werden kann
    private Vector3Int initialCellPosition;
    private void Awake()
    {
        initialPosition = transform.position; // Speichere die ursprüngliche Position bei Start/Awake
        startPosition = initialPosition;
    }

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = FindObjectOfType<Canvas>(); // Finde die Canvas im Spiel

        grid = FindObjectOfType<Grid>(); // Finde das Grid-Skript im Spiel
        if (grid != null)
        {
            // Nehme die Position des Grids als Offset
            gridOffset = grid.transform.position;
            // Definiere den Bereich des Rasters, in dem das Objekt platziert werden kann
            gridRange = new Vector2Int(grid.rows, grid.columns);
        }
        else
        {
            Debug.LogError("Grid-Skript nicht gefunden!");
        }
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        // Speichere die Zellenposition, in der sich das Objekt zu Beginn befindet
        initialCellPosition = grid.gridTilemap.WorldToCell(transform.position);
        
        // Entferne die Zelle, in der sich das Objekt zu Beginn befand, aus der besetzten Liste
        grid.RemoveObjectFromCell(initialCellPosition);
        previousPosition = transform.position;
    }
    public void OnDrag(PointerEventData eventData)
    { 
        Vector2 mousePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out mousePos);
        transform.position = canvas.transform.TransformPoint(mousePos);
        Vector3 dropPosition = transform.position;

        if (IsWithinAllowedRange(dropPosition))
        {
            Vector3Int cellPosition = grid.gridTilemap.WorldToCell(dropPosition);
            Vector3 cellCenter = grid.gridTilemap.GetCellCenterWorld(cellPosition);
            transform.position = cellCenter; // Snappen an die Zellenposition
            if(grid.IsCellFilled(cellPosition)){

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
            Vector3Int cellPosition = grid.gridTilemap.WorldToCell(dropPosition);

            if (!grid.IsCellFilled(cellPosition))
            {
                grid.PlaceObjectInCell(cellPosition, true);
                Vector3 cellCenter = grid.gridTilemap.GetCellCenterWorld(cellPosition);
                transform.position = cellCenter; // Snappen an die Zellenposition

                return;
            }
        }

        transform.position = startPosition; // Setze zurück zur ursprünglichen Position
    }

   private bool IsWithinAllowedRange(Vector3 position)
    {
        Vector3 cellPosition = grid.gridTilemap.WorldToCell(position);

        // Stelle sicher, dass die Position relativ zum Grid-Offset ist
        cellPosition -= gridOffset;

        // Überprüfe, ob die Zellenposition innerhalb des erlaubten Bereichs liegt
        return cellPosition.x >= 0 && cellPosition.x < gridRange.x &&
            cellPosition.y >= 0 && cellPosition.y < gridRange.y;
    }

   

}
