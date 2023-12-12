using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    public int rows = 10;
    public int columns = 10;
    public Tilemap gridTilemap;
    public TileBase defaultTile;

    [SerializeField]
    private List<Vector2Int> occupiedCells = new List<Vector2Int>(); // Liste für die belegten Zellen

    private List<Vector3Int> occupiedPositions = new List<Vector3Int>();

    void Start()
    {
        GenerateGrid();
        OccupyCells(occupiedCells); // Besetze die Zellen für das spezifische Gebäude
    }

    void GenerateGrid()
    {
        Vector3Int bottomLeft = gridTilemap.origin;

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                Vector3Int cellPosition = new Vector3Int(row, col, 0);
                gridTilemap.SetTile(cellPosition, defaultTile);
            }
        }
    }

    public bool AreCellsOccupied(List<Vector3Int> positions)
    {
        foreach (Vector3Int position in positions)
        {
            if (occupiedPositions.Contains(position))
            {
                return true;
            }
        }
        return false;
    }

    public void OccupyCells(List<Vector2Int> positions)
    {
        foreach (Vector2Int position in positions)
        {
            Vector3Int cellPosition = new Vector3Int(position.x, position.y, 0);
            occupiedPositions.Add(cellPosition);
            gridTilemap.SetTile(cellPosition, defaultTile);
        }
    }

    public void ReleaseCells(List<Vector2Int> positions)
    {
        foreach (Vector2Int position in positions)
        {
            Vector3Int cellPosition = new Vector3Int(position.x, position.y, 0);
            occupiedPositions.Remove(cellPosition);
            gridTilemap.SetTile(cellPosition, null);
        }
    }
}
