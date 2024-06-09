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

                    if (draggedBuilding.activatedEffect == true)
                    {
                        ApplyBuildingBonus(draggedBuilding.GetBuildingType(draggedBuilding.shape));
                    }
                    if(freshSpawn == true){

                    GameObject rerollButtonObject = GameObject.Find("Building_Reroll_Button");

                    if (rerollButtonObject != null)
                    {
                        Button buttonComponent = rerollButtonObject.GetComponent<Button>();

                        if (buttonComponent != null)
                        {
                            // Klicken Sie auf den Button, um das neue Prefab zu erstellen
                            buttonComponent.onClick.Invoke();
                        }
                    }
                    freshSpawn = false;
                    }
                   
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


    private void ApplyBuildingBonus(string buildingType)
    {
        BaseUnit_Script[] units = FindObjectsOfType<BaseUnit_Script>();
        Debug.Log(buildingType	);
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
                    Debug.Log("Produktivität steigt um Prozente");
                    draggedBuilding.activatedEffect = true;
                    break;
                default:
                    break;
            }
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
