using System.Collections.Generic;
using UnityEngine;

public class Unit_Inventory : MonoBehaviour
{
    // Die Prefabs, die instanziiert werden sollen
    public List<GameObject> unitPrefabs;

    // Anzahl der zu erzeugenden Einheiten pro Prefab
    private int numberOfUnitsPerPrefab = 1;

    // Abstand zwischen den erzeugten Einheiten
    public float spacing = 1.0f;

    // Basisposition für das Spawnen der Einheiten
    private Vector3 spawnBasePosition = Vector3.zero;

    // Referenzen für DragAndDrop-Script Einstellungen
    public GameObject battleSetupGridObject; // Referenz auf das Battle_Setup_Grid GameObject
    private Vector3 gridOffset = new Vector3(2.1244f, -3.1312f, 0f);
    private Vector2Int gridRange = new Vector2Int(4, 6);

    // Referenz auf das Canvas
    public GameObject battleSetupCanvas; // Referenz auf das Battle_Setup_Canvas GameObject

    private Unit_Inventory unit_Inventory;

    void Start()
    {
        if (unitPrefabs == null || unitPrefabs.Count == 0)
        {
            Debug.LogError("Unit Prefabs list is empty.");
            return;
        }

        if (battleSetupGridObject == null)
        {
            Debug.LogError("Battle Setup Grid Object is not assigned.");
            return;
        }

        if (battleSetupCanvas == null)
        {
            Debug.LogError("Battle Setup Canvas Object is not assigned.");
            return;
        }
        
        SpawnUnits();
    }

    void SpawnUnits()
    {
     /*   float currentXPosition = -11;
        int currentUnit = 0;
        foreach (var unitPrefab in unitPrefabs)
        {
            for (int i = 0; i < numberOfUnitsPerPrefab; i++)
            {
                // Berechne die Position für die Einheit basierend auf der aktuellen Position
                Vector3 position = new Vector3(currentXPosition, 5f, spawnBasePosition.z);

                // Erzeuge die Einheit und setze ihre Position
                GameObject newUnit = Instantiate(unitPrefab, position, Quaternion.identity);

                // Setze das neue Objekt als Kind des Battle_Setup_Canvas
                newUnit.transform.SetParent(battleSetupCanvas.transform, false);

                // Füge das BaseUnit_script hinzu (falls es nicht bereits am Prefab vorhanden ist)
                var baseUnitScript = newUnit.GetComponent<BaseUnit_Script>();
                if (baseUnitScript == null)
                {
                    baseUnitScript = newUnit.AddComponent<BaseUnit_Script>();
                }

                // Füge das DragAndDrop-Script hinzu und setze die Parameter
                var dragAndDropScript = newUnit.GetComponent<DragAndDrop>();
                if (dragAndDropScript == null)
                {
                    dragAndDropScript = newUnit.AddComponent<DragAndDrop>();
                }
                dragAndDropScript.gridManager = battleSetupGridObject.GetComponent<GridManager>();
                dragAndDropScript.gridOffset = gridOffset;
                dragAndDropScript.gridRange = gridRange;

                baseUnitScript.unitID = "Original " + currentUnit;
                // Setze die GridManager-Referenz auch im BaseUnit_Script
                baseUnitScript.gridManager = battleSetupGridObject.GetComponent<GridManager>();

                // Aktualisiere die aktuelle X-Position für die nächste Einheit
                currentXPosition += spacing;
            }

            // Füge den zusätzlichen Abstand zwischen den Prefabs hinzu
            currentXPosition += spacing;
            currentUnit++;
        }
    */
    }
}
