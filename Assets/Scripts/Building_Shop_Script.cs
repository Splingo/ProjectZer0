using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building_Shop_Script : MonoBehaviour
{
    // Referenz auf das Prefab, das instanziiert werden soll
    public GameObject prefab;

    // Referenz auf den Canvas, in dem das Prefab erstellt werden soll
    public Canvas cityCanvas;

    // Referenz auf den Gridmanager
    public GridManager cityGrid;

    // Methode, um ein Prefab zu erzeugen
    public void CreatePrefab(Vector3 worldPosition)
    {
        if (prefab != null)
        {
            if (cityCanvas != null && cityGrid != null)
            {
                // Prefab instanziieren und als Kind des Canvas setzen
                GameObject instantiatedPrefab = Instantiate(prefab, worldPosition, Quaternion.identity);
                instantiatedPrefab.transform.SetParent(cityCanvas.transform, false);

                // Konvertiere die Weltposition in lokale Canvas-Koordinaten
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    cityCanvas.transform as RectTransform, 
                    worldPosition, 
                    cityCanvas.worldCamera, 
                    out Vector2 localPosition
                );

                // Setze die lokale Position
                instantiatedPrefab.GetComponent<RectTransform>().localPosition = localPosition;

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
            Debug.LogError("Prefab ist nicht zugewiesen!");
        }
    }
}
