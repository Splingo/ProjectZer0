using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

public class Grid : MonoBehaviour
{
  public int rows = 10; // Anzahl der Reihen im Raster
    public int columns = 10; // Anzahl der Spalten im Raster
    
    public Tilemap gridTilemap; // Referenz auf die Tilemap
    public TileBase defaultTile; // Standard-Tile für die Zellen
    private List<Vector3Int> occupiedPositions = new List<Vector3Int>();
    
    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {

        // Finde die untere linke Ecke der Tilemap
        Vector3Int bottomLeft = gridTilemap.origin;

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                Vector3Int cellPosition =  new Vector3Int(row-11, col-5,0); // Position der Zelle im Raster
                gridTilemap.SetTile(cellPosition, defaultTile); // Lege das Standard-Tile in der Zelle an
            }
        }
    }
    public bool IsCellFilled(Vector3Int position)
    {
        return occupiedPositions.Contains(position);
    }

    public void PlaceObjectInCell(Vector3Int cellPosition, bool isFilled)
    {
        if (!IsCellFilled(cellPosition))
        {
            occupiedPositions.Add(cellPosition);
            if (isFilled)
            {
                // Überprüfe, ob ein Standard-Tile festgelegt wurde, um gefüllte Zellen darzustellen
                if (defaultTile != null)
                {
                    gridTilemap.SetTile(cellPosition, defaultTile); // Setze das Standard-Tile in der Zelle
                }
                else
                {
                    Debug.LogError("Es wurde kein Standard-Tile festgelegt, um gefüllte Zellen darzustellen!");
                }
            }
        }
        else
        {
            Debug.Log("Zelle ist bereits gefüllt!");
        }
    }

    public void RemoveObjectFromCell(Vector3Int cellPosition)
    {
        if (IsCellFilled(cellPosition))
        {
            occupiedPositions.Remove(cellPosition);
            gridTilemap.SetTile(cellPosition, defaultTile); 
        }
        else
        {
            Debug.Log("Zelle ist bereits leer!");
        }
    }

}
