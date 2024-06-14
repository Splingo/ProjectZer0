using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Buy_Building_Script : MonoBehaviour
{
    public building_Inventory buildingInventory; // Referenz auf das building_Inventory-Script
    public CityManager cityManager; // Referenz auf das CityManager-Script

    private Building_Shop_Script buildingShopScript; // Referenz auf das Building_Shop_Script-Script

    void Start()
    {
        buildingShopScript = FindObjectOfType<Building_Shop_Script>();
        if (buildingShopScript == null)
        {
            Debug.LogError("Building_Shop_Script not found in the scene!");
        }
    }

    // Methode, die aufgerufen wird, wenn der Button gedrückt wird
    public void CheckForShopObject()
    {
        // Finde alle GameObjects im Spiel
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();

        // Iteriere durch alle gefundenen GameObjects
        foreach (GameObject obj in allObjects)
        {
            // Überprüfe, ob der Name des GameObjects "_Shop" enthält
            if (obj.name.Contains("_Shop"))
            {
                // Hole das Building_Class-Script des GameObjects
                Building_Class buildingClass = obj.GetComponent<Building_Class>();

                // Überprüfe, ob das Building_Class-Script vorhanden ist
                if (buildingClass != null)
                {
                    // Überprüfe, ob genug Gold vorhanden ist, um das Gebäude zu kaufen
                    int buildingCost = buildingClass.value;
                    if (cityManager.CanAffordReroll(buildingCost))
                    {
                        // Kaufe das Gebäude (ziehe die Kosten ab)
                        cityManager.DeductRerollCost(buildingCost);

                        // Füge das entsprechende Gebäude dem Inventar hinzu
                        buildingInventory.AddbuildingToInventory(buildingClass.buildingID);

                        // Finde ein GameObject mit dem Namen, der dem buildingName entspricht
                        GameObject buildingObject = GameObject.Find(buildingClass.buildingName);
                        if (buildingObject != null)
                        {
                            // Aktiviere das Create_Building_OnDrag Skript, wenn es vorhanden ist
                            Create_Building_OnDrag_Script createBuildingScript = buildingObject.GetComponent<Create_Building_OnDrag_Script>();
                            if (createBuildingScript != null)
                            {
                                createBuildingScript.enabled = true;
                                Debug.Log("Create_Building_OnDrag für " + buildingClass.buildingName + " aktiviert.");
                            }
                            else
                            {
                                Debug.LogWarning("Create_Building_OnDrag Skript nicht gefunden auf: " + buildingClass.buildingName);
                            }
                        }
                        else
                        {
                            Debug.LogWarning("GameObject mit dem Namen " + buildingClass.buildingName + " nicht gefunden.");
                        }

                        // Hier der restliche Code zum Aktivieren des Reroll-Buttons, falls gewünscht
                        GameObject rerollButtonObject = GameObject.Find("Building_Reroll_Button");
                        if (rerollButtonObject != null)
                        {
                            Button buttonComponent = rerollButtonObject.GetComponent<Button>();
                            if (buttonComponent != null)
                            {
                                // Temporär die Reroll-Kosten nicht erhöhen
                                int originalRerollCost = buildingShopScript.rerollCost;
                                buildingShopScript.rerollCost = 0;

                                buttonComponent.onClick.Invoke();

                                // Setze die Reroll-Kosten zurück
                                buildingShopScript.rerollCost = originalRerollCost;
                            }
                        }
                    }
                    else
                    {
                        Debug.Log("Nicht genug Ressourcen, um das Gebäude zu kaufen.");
                    }
                }
                else
                {
                    Debug.LogError("Building_Class nicht gefunden auf: " + obj.name);
                }
            }
        }
    }
}
