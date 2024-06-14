using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building_Shop_Script : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> prefabs;

    public Canvas cityCanvas;
    public GridManager cityGrid;

    private CityManager cityManager; // Reference to CityManager

    public int rerollCost = 0;

    void Start()
    {
        cityManager = FindObjectOfType<CityManager>();
        if (cityManager == null)
        {
            Debug.LogError("CityManager not found in the scene!");
        }
    }

    public void CreatePrefab(Vector3 worldPosition)
    {
        if (cityManager == null)
        {
            Debug.LogError("CityManager is not assigned!");
            return;
        }

        // Check if city can afford the reroll cost
        if (cityManager.CanAffordReroll(rerollCost))
        {
            // Deduct the reroll cost from city resources
            cityManager.DeductRerollCost(rerollCost);
            rerollCost++;

            // Convert world position to local Canvas coordinates
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                cityCanvas.transform as RectTransform,
                worldPosition,
                cityCanvas.worldCamera,
                out Vector2 localPosition
            );

            // Check if there's an object at the position and destroy it
            Collider2D[] colliders = Physics2D.OverlapPointAll(worldPosition);
            foreach (Collider2D collider in colliders)
            {
                if (collider.gameObject != null)
                {
                    Destroy(collider.gameObject);
                }
            }

            // Instantiate a random prefab from the list
            if (prefabs != null && prefabs.Count > 0)
            {
                GameObject prefab = prefabs[Random.Range(0, prefabs.Count)];
                GameObject instantiatedPrefab = Instantiate(prefab, worldPosition, Quaternion.identity);
                instantiatedPrefab.transform.SetParent(cityCanvas.transform, false);
                instantiatedPrefab.GetComponent<RectTransform>().localPosition = localPosition;
                instantiatedPrefab.name += "_Shop";

                // Set GridManager in Building_Class if available
                Building_Class buildingClass = instantiatedPrefab.GetComponent<Building_Class>();
                if (buildingClass != null)
                {
                    buildingClass.gridManager = cityGrid;
                }

                // Set GridManager in DragAndDropBuilding if available
                DragAndDropBuilding dragAndDrop = instantiatedPrefab.GetComponent<DragAndDropBuilding>();
                dragAndDrop.enabled = false;
                if (dragAndDrop != null)
                {
                    dragAndDrop.gridManager = cityGrid;
                }
            }
            else
            {
                Debug.LogError("Prefabs list is empty or not assigned!");
            }
        }
        else
        {
            Debug.Log("Not enough resources (gold) to reroll prefab.");
        }
    }
}
