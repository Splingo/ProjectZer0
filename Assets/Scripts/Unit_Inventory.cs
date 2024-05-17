using System.Collections.Generic;
using UnityEngine;

public class Unit_Inventory : MonoBehaviour
{
  // Das Prefab, das instanziert werden soll
    public GameObject baseUnitPrefab;

    // Anzahl der zu erzeugenden Einheiten
    public int numberOfUnits = 12;

    // Abstand zwischen den erzeugten Einheiten
    public float spacing = 2.0f;

    // Basisposition für das Spawnen der Einheiten
    public Vector3 spawnBasePosition = Vector3.zero;

    // Referenzen für DragAndDrop-Script Einstellungen
    public GameObject battleSetupGridObject; // Referenz auf das Battle_Setup_Grid GameObject
    public Vector3 gridOffset = new Vector3(2.1244f, -3.1312f, 0f);
    public Vector2Int gridRange = new Vector2Int(4, 6);

    // Referenz auf das Canvas
    public GameObject battleSetupCanvas; // Referenz auf das Battle_Setup_Canvas GameObject

    void Start()
    {
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
        for (int i = 0; i < numberOfUnits; i++)
        {
            // Berechne die Position für die Einheit basierend auf der Basisposition
            Vector3 position = spawnBasePosition + new Vector3(i * spacing, 0, 0);
            
            // Erzeuge die Einheit und setze ihre Position
            GameObject newUnit = Instantiate(baseUnitPrefab, position, Quaternion.identity);

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

            // Setze die GridManager-Referenz auch im BaseUnit_Script
            baseUnitScript.gridManager = battleSetupGridObject.GetComponent<GridManager>();
        }
    }
}
