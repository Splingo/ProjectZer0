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
    private bool onSpawn = true;

     private GameObject copyObject;


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
        if (baseUnit != null)
        {
            List<Vector3Int> occupiedBattleCells = baseUnit.GetOccupiedCells(initialCellPosition);
            baseUnit.previousOccupiedCells = occupiedBattleCells;
            if (occupiedBattleCells != null && occupiedBattleCells.Count > 0)
            {
                gridManager.ReleaseCells(occupiedBattleCells);
            }
        }

        previousPosition = transform.position;

        if (onSpawn == true && transform.position.y <= -3.5)
        {
            CreateCopy();
        }
    }





public void OnDrag(PointerEventData eventData)
{

    Vector3 mousePos = Input.mousePosition;
    mousePos.z = 10; // Entfernung der Canvas-Ebene
    Vector3 screenPos = Camera.main.ScreenToWorldPoint(mousePos);

    // Bewege das kopierte Objekt, wenn es vorhanden ist
    if (copyObject != null)
    {
        copyObject.transform.position = screenPos;
    }
    else
    {
        transform.position = screenPos; // Bewege das Originalobjekt
    }

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
        if (baseUnit != null)
        {
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

                if(draggedBuilding.activatedEffect == false){
                ApplyBuildingBonus(draggedBuilding.GetBuildingType(draggedBuilding.shape));
                }

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
        
   transform.position = previousPosition; // Setze zurück zur ursprünglichen Position
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


private void ApplyBuildingBonus(string buildingType)
{
    BaseUnit_Script[] units = FindObjectsOfType<BaseUnit_Script>();
        foreach (BaseUnit_Script unit in units)
        {    
    // Füge hier die Logik hinzu, um den entsprechenden Bonus für den Gebäudetyp anzuwenden
    switch (buildingType)
    {
        case "Schmiede":
            Debug.Log("Einheiten erhalten Angriffsbonus durch die Schmiede.");
            draggedBuilding.activatedEffect = true;
            unit.UpdateAttackDamage(unit.attackDamage + 0.5f);
            break;
        case "Kirche":
            Debug.Log("Einheiten erhalten HP durch die Kirche.");
            draggedBuilding.activatedEffect = true;
            unit.UpdateMaxHP(unit.maxHP + 10f);
            break;
        case "Turm":
            Debug.Log("Einheiten erhalten Range durch den Turm.");
            draggedBuilding.activatedEffect = true;
            unit.UpdateAttackRange(unit.attackRange + 0.15f);
            break;
        case "Rathaus":
            Debug.Log("Stadt erhält mehr Einheiten durch das Rathaus.");
            draggedBuilding.activatedEffect = true;
            break;
        case "Bank":
            Debug.Log("Stadt erhält mehr Gold durch die Bank.");
            draggedBuilding.activatedEffect = true;
            break;
        case "Taverne":
            Debug.Log("Einheiten erhalten AttakSpeed durch die Taverne.");
            draggedBuilding.activatedEffect = true;
            unit.UpdateAttackSpeed(unit.attackSpeed + 0.2f);
            break;
        case "Shop":
            Debug.Log("Man kann einkaufen.");
            draggedBuilding.activatedEffect = true;
            break;
        case "Bewohner":
           Debug.Log("Prduktivität steigt um Prozente");
            draggedBuilding.activatedEffect = true;
            break;
        default:
            break;
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
   private GameObject CreateCopy()
    {
        if(onSpawn == true){

        GameObject copy = Instantiate(gameObject); // Erzeuge eine Kopie des aktuellen GameObjects
        // Füge die Kopie zum Battle_Setup_Canvas hinzu oder einem anderen geeigneten Elternobjekt hinzu
        copy.transform.SetParent(canvas.transform, false);
        // Setze die Position der Kopie auf die aktuelle Position des Originals
        copy.transform.position = transform.position;
        return copy; // Gib die kopierte Instanz zurück
        }
        return gameObject;
    }

}



public class DragAndDropObject : MonoBehaviour
{
      public bool onSpawn = false;
}

