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
    public List<Vector3Int> occupiedCells; // Liste der belegten Zellen (Zeile, Spalte)
    public List<Vector3Int> previousOccupiedCells; // Liste der belegten Zellen (Zeile, Spalte)
    public List<Vector3Int> hoverungOccupiedCells; // Liste der belegten Zellen (Zeile, Spalte)
    public bool activatedEffect = false;

    public List<Vector3Int> GetOccupiedCells(Vector3Int center)
    {
        OccupyCellsBasedOnShape(center, shape);
        return occupiedCells;
    }
    public List<Vector3Int> ReturnOccupiedCells()
    {
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

    public void OccupyCellsBasedOnShape(Vector3Int beginCell, BuildingShape shape)
{
    shape = this.shape;
    this.occupiedCells = new List<Vector3Int>();

    switch (shape)
    {
        case BuildingShape.Block_2x2:
            // beginCell ist unten links, nimmt eine Zelle rechts und jeweils über den beiden Zellen ein
            occupiedCells.Add(beginCell);
            occupiedCells.Add(new Vector3Int(beginCell.x + 1, beginCell.y, 0));
            occupiedCells.Add(new Vector3Int(beginCell.x, beginCell.y + 1, 0));
            occupiedCells.Add(new Vector3Int(beginCell.x + 1, beginCell.y + 1, 0));
            break;
        case BuildingShape.Block_1x1:
            // beginCell ist die einzelne Zelle und nimmt nur diese Zelle ein
            occupiedCells.Add(beginCell);

            break;
        case BuildingShape.Tower_1x3:
            // beginCell ist die mittlere Zelle, nimmt eine Zelle über und unter sich ein
            occupiedCells.Add(beginCell);
            occupiedCells.Add(new Vector3Int(beginCell.x, beginCell.y + 1, 0));
            occupiedCells.Add(new Vector3Int(beginCell.x, beginCell.y - 1, 0));

            break;
        case BuildingShape.Block_2x1:
            // beginCell ist die linke Zelle, nimmt eine weitere Zelle rechts von sich ein
            occupiedCells.Add(beginCell);
            occupiedCells.Add(new Vector3Int(beginCell.x + 1, beginCell.y, 0));

            break;
        case BuildingShape.T_Shape:
            // beginCell ist die mittlere Zelle, hat links und rechts eine Zelle und von der Mitte runter 2 Zellen
            occupiedCells.Add(beginCell);
            occupiedCells.Add(new Vector3Int(beginCell.x - 1, beginCell.y, 0));
            occupiedCells.Add(new Vector3Int(beginCell.x + 1, beginCell.y, 0));
            occupiedCells.Add(new Vector3Int(beginCell.x, beginCell.y - 1, 0));

            break;
        case BuildingShape.L_Shape:
            // Korrigierte Implementierung für L_Shape
            occupiedCells.Add(beginCell);
            occupiedCells.Add(new Vector3Int(beginCell.x, beginCell.y - 1, 0));
            occupiedCells.Add(new Vector3Int(beginCell.x + 1, beginCell.y, 0));

            break;
        case BuildingShape.Cross:
            // Korrigierte Implementierung für Cross
            occupiedCells.Add(beginCell);
            occupiedCells.Add(new Vector3Int(beginCell.x, beginCell.y + 1, 0));
            occupiedCells.Add(new Vector3Int(beginCell.x - 1, beginCell.y, 0));
            occupiedCells.Add(new Vector3Int(beginCell.x + 1, beginCell.y, 0));
            occupiedCells.Add(new Vector3Int(beginCell.x, beginCell.y - 1, 0));
            occupiedCells.Add(new Vector3Int(beginCell.x, beginCell.y - 2, 0));

            break;
        case BuildingShape.U_Shape:
            // beginCell ist unten in der Mitte, links und rechts eine Zelle und von diesen Zellen eine Zelle nach oben jeweils
            occupiedCells.Add(beginCell);
            occupiedCells.Add(new Vector3Int(beginCell.x - 1, beginCell.y, 0));
            occupiedCells.Add(new Vector3Int(beginCell.x + 1, beginCell.y, 0));
            occupiedCells.Add(new Vector3Int(beginCell.x - 1, beginCell.y - 1, 0));
            occupiedCells.Add(new Vector3Int(beginCell.x + 1, beginCell.y - 1, 0));

            break;
        default:
            // Für den Fall, dass keine passende Form angegeben ist, wird Block_1x1 als Standardform verwendet
            occupiedCells.Add(beginCell);

            break;
    }

}
  public string GetBuildingType(BuildingShape shape)
    {
        switch (shape)
        {
            case BuildingShape.Block_2x2:
                return "Bank";
            case BuildingShape.Block_1x1:
                return "Bewohner";
            case BuildingShape.Tower_1x3:
                return "Turm";
            case BuildingShape.Block_2x1:
                return "Shop";
            case BuildingShape.T_Shape:
                return "Rathaus";
            case BuildingShape.L_Shape:
                return "Taverne";
            case BuildingShape.Cross:
                return "Kirche";
            case BuildingShape.U_Shape:
                return "Schmiede";
            default:
                return null;
        }
    }



}
