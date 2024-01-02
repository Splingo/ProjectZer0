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

    public List<Vector3Int> occupiedPositions = new List<Vector3Int>();

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        Vector3Int bottomLeft = gridTilemap.origin;

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                Vector3Int cellPosition = new Vector3Int(row, col, 0);
               // gridTilemap.SetTile(cellPosition, defaultTile);
            }
        }
    }

 public bool AreCellsOccupied(List<Vector3Int> buildingOccupiedCells)
{
    foreach (Vector3Int position in buildingOccupiedCells)
    {
        if (occupiedPositions.Contains(position))
        {
            return true; // Wenn eine Position bereits besetzt ist, dann ist die gesamte Zelle besetzt
        }
    }
    return false; // Keine der Positionen des Elements ist besetzt
}




    public void OccupyCells(List<Vector3Int> positions)
{
    foreach (Vector3Int position in positions)
    {
        Vector3Int cellPosition = new Vector3Int(position.x, position.y, 0);

        // Überprüfe, ob die Zelle bereits in der Liste enthalten ist
        if (!occupiedPositions.Contains(cellPosition))
        {
            occupiedPositions.Add(cellPosition);
            gridTilemap.SetTile(cellPosition, defaultTile);
        }
    }
}


   public void ReleaseCells(List<Vector3Int> positions)
{
    foreach (Vector3Int position in positions)
    {
        Vector3Int cellPosition = new Vector3Int(position.x, position.y, 0);

        // Entferne alle Vorkommen der Zelle aus occupiedPositions
        while (occupiedPositions.Contains(cellPosition))
        {
            occupiedPositions.Remove(cellPosition);
            gridTilemap.SetTile(cellPosition, null);
        }
    }
}

}
