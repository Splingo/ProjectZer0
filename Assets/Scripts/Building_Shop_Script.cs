using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building_Shop_Script : MonoBehaviour
{
    [SerializeField]
    // Liste der Prefabs, die instanziiert werden können
    private List<GameObject> prefabs;

    // Referenz auf den Canvas, in dem das Prefab erstellt werden soll
    public Canvas cityCanvas;

    // Referenz auf den Gridmanager
    public GridManager cityGrid;

    public int rerollCost = 0;

    // Methode, um ein zufälliges Prefab zu erzeugen
    public void CreatePrefab(Vector3 worldPosition)
    {
        rerollCost++;
        if (prefabs != null && prefabs.Count > 0)
        {
            if (cityCanvas != null && cityGrid != null)
            {
                // Konvertiere die Weltposition in lokale Canvas-Koordinaten
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    cityCanvas.transform as RectTransform, 
                    worldPosition, 
                    cityCanvas.worldCamera, 
                    out Vector2 localPosition
                );

                // Überprüfe, ob ein Objekt an der Position existiert
                Collider2D[] colliders = Physics2D.OverlapPointAll(worldPosition);
                foreach (Collider2D collider in colliders)
                {
                    if (collider.gameObject != null)
                    {
                        // Zerstöre das vorhandene Objekt
                        Destroy(collider.gameObject);
                    }
                }

                // Wähle ein zufälliges Prefab aus der Liste
                GameObject prefab = prefabs[Random.Range(0, prefabs.Count)];

                // Prefab instanziieren und als Kind des Canvas setzen
                GameObject instantiatedPrefab = Instantiate(prefab, worldPosition, Quaternion.identity);
                instantiatedPrefab.transform.SetParent(cityCanvas.transform, false);

                // Setze die lokale Position des instanziierten Prefabs
                instantiatedPrefab.GetComponent<RectTransform>().localPosition = localPosition;
                instantiatedPrefab.name += "_Shop";

                // Setze den Gridmanager in Building_Class
                Building_Class buildingClass = instantiatedPrefab.GetComponent<Building_Class>();
                if (buildingClass != null)
                {
                    buildingClass.gridManager = cityGrid;
                }

                // Setze den Gridmanager in DragAndDrop
                DragAndDropBuilding dragAndDrop = instantiatedPrefab.GetComponent<DragAndDropBuilding>();
                if (dragAndDrop != null)
                {
                    dragAndDrop.gridManager = cityGrid;
                }
            }
            else
            {
                Debug.LogError("City Canvas oder City Grid ist nicht zugewiesen!");
            }
        }
        else
        {
            Debug.LogError("Prefabs-Liste ist leer oder nicht zugewiesen!");
        }
    }
}
