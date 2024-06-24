using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    public Building_Shop_Script buildingShopScript;
    public float xOffset = 1.0f; // Der Offset, um den der y-Wert erhöht wird

    void Start()
    {
        // Button-Komponente holen und Listener hinzufügen
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(OnButtonClick);
        }
    }

    void OnButtonClick()
    {
        if (buildingShopScript != null)
        {
            // Position des Buttons im Weltkoordinatensystem erhalten
            RectTransform rectTransform = GetComponent<RectTransform>();
            Vector3 buttonPosition = rectTransform.position;

            // y-Wert anpassen
            Vector3 spawnPosition = new Vector3(buttonPosition.x + xOffset, buttonPosition.y , buttonPosition.z);

            // Prefab erzeugen
            buildingShopScript.CreatePrefab(spawnPosition);
        }
    }
}
