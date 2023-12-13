using UnityEngine;
using System.Collections.Generic;

public class Building_Class : MonoBehaviour
{
    public enum BuildingShape
    {
        Block_2x2,
        Block_1x1,
        Tower_1x3,
        Block_2x1,
        T_Shape,
        L_Shape,
        Cross,
        U_Shape
    }
    public GridManager gridManager;
    public string buildingName;
    public int resourceProductionRate;
    public Sprite buildingSprite;
    public int collectedResources;
    public int resourcesRequiredForUpgrade;
    public bool canUpgrade;
    public int buildingCount;
    private List<Vector3Int> occupiedCells; // Liste der belegten Zellen (Zeile, Spalte)
    public List<Vector3Int> GetOccupiedCells(Vector3Int center)
    {
        OccupyCellsBasedOnShape(center, shape);
        return occupiedCells;
    }

    public BuildingShape shape;

    // Konstruktor für die Klasse Buildings
    public Building_Class(string name, int productionRate, Sprite sprite, int requiredResources, bool upgradeStatus, BuildingShape shape, int count)
{
    buildingName = name;
    resourceProductionRate = productionRate;
    buildingSprite = sprite;
    resourcesRequiredForUpgrade = requiredResources;
    canUpgrade = upgradeStatus;
    buildingCount = count;
    collectedResources = 0; // Initialisiere die gesammelten Ressourcen mit 0
    this.shape = shape;
    this.occupiedCells = new List<Vector3Int>();
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

    public void OccupyCellsBasedOnShape(Vector3Int center, BuildingShape shape)
{
    shape = this.shape;
    this.occupiedCells = new List<Vector3Int>();

    switch (shape)
    {
        case BuildingShape.Block_2x2:
            // Zentrum ist unten links, nimmt eine Zelle rechts und jeweils über den beiden Zellen ein
            occupiedCells.Add(center);
            occupiedCells.Add(new Vector3Int(center.x + 1, center.y, 0));
            occupiedCells.Add(new Vector3Int(center.x, center.y + 1, 0));
            occupiedCells.Add(new Vector3Int(center.x + 1, center.y + 1, 0));
            break;
        case BuildingShape.Block_1x1:
            // Zentrum ist die einzelne Zelle und nimmt nur diese Zelle ein
            occupiedCells.Add(center);
            break;
        case BuildingShape.Tower_1x3:
            // Zentrum ist die mittlere Zelle, nimmt eine Zelle über und unter sich ein
            occupiedCells.Add(center);
            occupiedCells.Add(new Vector3Int(center.x, center.y + 1, 0));
            occupiedCells.Add(new Vector3Int(center.x, center.y - 1, 0));
            break;
        case BuildingShape.Block_2x1:
            // Zentrum ist die linke Zelle, nimmt eine weitere Zelle rechts von sich ein
            occupiedCells.Add(center);
            occupiedCells.Add(new Vector3Int(center.x + 1, center.y, 0));
            break;
        case BuildingShape.T_Shape:
            // Zentrum ist die mittlere Zelle, hat links und rechts eine Zelle und von der Mitte runter 2 Zellen
            occupiedCells.Add(center);
            occupiedCells.Add(new Vector3Int(center.x - 1, center.y, 0));
            occupiedCells.Add(new Vector3Int(center.x + 1, center.y, 0));
            occupiedCells.Add(new Vector3Int(center.x, center.y - 1, 0));
            break;
        case BuildingShape.L_Shape:
            // Korrigierte Implementierung für L_Shape
            occupiedCells.Add(center);
            occupiedCells.Add(new Vector3Int(center.x, center.y - 1, 0));
            occupiedCells.Add(new Vector3Int(center.x, center.y - 2, 0));
            occupiedCells.Add(new Vector3Int(center.x + 1, center.y, 0));
            occupiedCells.Add(new Vector3Int(center.x + 2, center.y, 0));
            occupiedCells.Add(new Vector3Int(center.x + 3, center.y, 0));
            break;
        case BuildingShape.Cross:
            // Korrigierte Implementierung für Cross
            occupiedCells.Add(center);
            occupiedCells.Add(new Vector3Int(center.x, center.y + 1, 0));
            occupiedCells.Add(new Vector3Int(center.x, center.y + 2, 0));
            occupiedCells.Add(new Vector3Int(center.x - 1, center.y, 0));
            occupiedCells.Add(new Vector3Int(center.x + 1, center.y, 0));
            occupiedCells.Add(new Vector3Int(center.x, center.y - 1, 0));
            occupiedCells.Add(new Vector3Int(center.x, center.y - 2, 0));
            break;
        case BuildingShape.U_Shape:
            // Zentrum ist unten in der Mitte, links und rechts eine Zelle und von diesen Zellen eine Zelle nach oben jeweils
            occupiedCells.Add(center);
            occupiedCells.Add(new Vector3Int(center.x - 1, center.y, 0));
            occupiedCells.Add(new Vector3Int(center.x + 1, center.y, 0));
            occupiedCells.Add(new Vector3Int(center.x - 1, center.y + 1, 0));
            occupiedCells.Add(new Vector3Int(center.x + 1, center.y + 1, 0));
            break;
        default:
            // Für den Fall, dass keine passende Form angegeben ist, wird Block_1x1 als Standardform verwendet
            occupiedCells.Add(center);
            break;
    }

    // Übergebe die Liste der belegten Zellen dem GridManager, um diese Zellen zu besetzen
    gridManager.OccupyCells(occupiedCells);
}


}
