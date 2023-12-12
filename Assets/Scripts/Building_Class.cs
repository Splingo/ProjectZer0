using UnityEngine;
using System.Collections.Generic;

public class Building_Class : MonoBehaviour
{
    public enum BuildingShape
    {
        T_Shape,
        L_Shape
        // Weitere Formen hier hinzufügen
    }
    public GridManager gridManager;
    public string buildingName;
    public int resourceProductionRate;
    public Sprite buildingSprite;
    public int collectedResources;
    public int resourcesRequiredForUpgrade;
    public bool canUpgrade;
    public int buildingCount;
    public List<Vector2Int> occupiedCells; // Liste der belegten Zellen (Zeile, Spalte)
    public BuildingShape shape;

    // Konstruktor für die Klasse Buildings
    public Building_Class(string name, int productionRate, Sprite sprite, int requiredResources, bool upgradeStatus, int count, List<Vector2Int> occupiedCells)
    {
        buildingName = name;
        resourceProductionRate = productionRate;
        buildingSprite = sprite;
        resourcesRequiredForUpgrade = requiredResources;
        canUpgrade = upgradeStatus;
        buildingCount = count;
        collectedResources = 0; // Initialisiere die gesammelten Ressourcen mit 0
        this.occupiedCells = occupiedCells; // Setze die Liste der belegten Zellen
    }

    // Methode zum Erhöhen der gesammelten Ressourcen
    public void IncreaseResources(int amount)
    {
        collectedResources += amount;
    }

    // Methode zum Überprüfen, ob ein Upgrade durchgeführt werden kann
    public bool CanPerformUpgrade()
    {
        return collectedResources >= resourcesRequiredForUpgrade && canUpgrade;
    }

    // Methode zum Durchführen eines Upgrades, wenn möglich
    public void PerformUpgrade()
    {
        if (CanPerformUpgrade())
        {
            collectedResources -= resourcesRequiredForUpgrade;
            // Führen Sie hier die Upgrade-Logik durch (z. B. erhöhen Sie die Produktionsrate, aktualisieren Sie Werte usw.)
            Debug.Log("Upgrade durchgeführt für " + buildingName);
        }
        else
        {
            Debug.Log("Upgrade nicht möglich für " + buildingName);
        }
    }

    public void OccupyCellsBasedOnShape(Vector2Int center, List<Vector2Int> shape)
    {
        List<Vector2Int> occupiedCells = new List<Vector2Int>();

        // Für jedes Zellenschema in der Form berechne die tatsächliche Zellenposition und füge sie zu den belegten Zellen hinzu
        foreach (Vector2Int cell in shape)
        {
            Vector2Int cellPosition = new Vector2Int(center.x + cell.x, center.y + cell.y);
            occupiedCells.Add(cellPosition);
        }

        // Übergebe die Liste der belegten Zellen dem GridManager, um diese Zellen zu besetzen
        gridManager.OccupyCells(occupiedCells);
    }
}
