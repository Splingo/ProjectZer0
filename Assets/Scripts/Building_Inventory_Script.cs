using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingInventoryScript : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> prefabs;

    [SerializeField]
    private RectTransform content;

    [SerializeField]
    private Canvas cityCanvas;

    [SerializeField]
    private GridManager cityGrid;

    void Start()
    {
        // Instantiate and place prefabs vertically
        foreach (var prefab in prefabs)
        {
            // Instantiate the prefab as a child of the content RectTransform
            GameObject instance = Instantiate(prefab, content);
            
            // Get the RectTransform component of the instance
            RectTransform rectTransform = instance.GetComponent<RectTransform>();

            // Optionally adjust the size and position of each prefab if needed
            rectTransform.sizeDelta = new Vector2(content.rect.width, rectTransform.sizeDelta.y);

            // Set the local position (ignoring Z position)
            rectTransform.localPosition = new Vector3(rectTransform.localPosition.x, rectTransform.localPosition.y, 0);

            // Ensure the instantiated prefab has the correct canvas and grid manager references
            SetupPrefab(instance);
        }
    }

    private void SetupPrefab(GameObject instantiatedPrefab)
    {
        // Set the grid manager in Building_Class
        Building_Class buildingClass = instantiatedPrefab.GetComponent<Building_Class>();
        if (buildingClass != null)
        {
            buildingClass.gridManager = cityGrid;
        }

        // Set the grid manager in DragAndDropBuilding
        DragAndDropBuilding dragAndDrop = instantiatedPrefab.GetComponent<DragAndDropBuilding>();
        if (dragAndDrop != null)
        {
            dragAndDrop.gridManager = cityGrid;
        }

        // If needed, set other components here
    }
}
