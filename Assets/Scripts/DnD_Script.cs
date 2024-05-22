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
    private friendly_ranged rangedUnit;

    public Unit_Inventory unit_Inventory;

    private GameObject copyObject;

    public int unitTypeIndex;



    private void Awake()
    {
        initialPosition = transform.position; // Speichere die ursprüngliche Position bei Start/Awake
        startPosition = initialPosition;
        draggedBuilding = GetComponent<Building_Class>();
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
 if (unit_Inventory != null && unit_Inventory.unitInInventoryCount[unitTypeIndex] > 0)
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
            baseUnit.previousOccupiedCells = baseUnit.GetOccupiedCells(initialCellPosition);
        }
        if (rangedUnit != null)
        {
            rangedUnit.previousOccupiedCells = baseUnit.GetOccupiedCells(initialCellPosition);

        }


        previousPosition = transform.position;
        }

    }





    public void OnDrag(PointerEventData eventData)
    {
         if (unit_Inventory != null && unit_Inventory.unitInInventoryCount[unitTypeIndex] > 0)
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
            if (baseUnit != null)
            {

                List<Vector3Int> UnitOccupiedCells = baseUnit.ReturnOccupiedCells();
                baseUnit.hoveringOccupiedCells = baseUnit.GetOccupiedCells(cellPosition);
            }
            if (rangedUnit != null)
            {

                List<Vector3Int> UnitOccupiedCells = rangedUnit.ReturnOccupiedCells();
                rangedUnit.hoveringOccupiedCells = rangedUnit.GetOccupiedCells(cellPosition);
            }
        }}
    }



    public void OnEndDrag(PointerEventData eventData)
    {
         if (unit_Inventory != null && unit_Inventory.unitInInventoryCount[unitTypeIndex] > 0)
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

                    if (!draggedBuilding.activatedEffect)
                    {
                        ApplyBuildingBonus(draggedBuilding.GetBuildingType(draggedBuilding.shape));
                    }

                    return; // Exit early if placement is successful
                }
            }
            else if (rangedUnit != null)
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
                    unit_Inventory.RemoveUnit(1);
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
                    unit_Inventory.RemoveUnit(0);

                    return; // Exit early if placement is successful
                }
            }

        }

        // Re-occupy previous cells if placement was not successful
        if (draggedBuilding != null)
        {
            gridManager.OccupyCells(draggedBuilding.previousOccupiedCells);
        }

        if (rangedUnit != null)
        {
            List<Vector3Int> occupiedBattleCells = rangedUnit.GetOccupiedCells(initialCellPosition);
            rangedUnit.previousOccupiedCells = occupiedBattleCells;

            // Log the state of gridManager before releasing cells

            if (occupiedBattleCells != null && occupiedBattleCells.Count > 0)
            {
                gridManager.ReleaseCells(occupiedBattleCells);
            }

            // Log the state of gridManager after releasing cells
            unit_Inventory.AddUnit(1);

            Destroy(gameObject);
        }
        else if (baseUnit != null)
        {

            List<Vector3Int> occupiedBattleCells = baseUnit.GetOccupiedCells(initialCellPosition);
            baseUnit.previousOccupiedCells = occupiedBattleCells;
            if (occupiedBattleCells != null && occupiedBattleCells.Count > 0)
            {
                gridManager.ReleaseCells(occupiedBattleCells);
            }
            unit_Inventory.AddUnit(0);

            Destroy(gameObject);
        }


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

}


