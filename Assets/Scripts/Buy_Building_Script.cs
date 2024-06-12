using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using UnityEngine.UI;

public class Buy_Building_Script : MonoBehaviour
{
    public building_Inventory buildingInventory; // Referenz auf das building_Inventory-Script

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
                    int buildingID = buildingClass.buildingID;

                    // Überprüfe, ob die buildingID gültig ist
                    if (buildingID >= 0 && buildingID < buildingInventory.buildingGameObjects.Length)
                    {
                        // Füge das entsprechende Gebäude dem Inventar hinzu
                        if (buildingInventory != null)
                        {
                            buildingInventory.AddbuildingToInventory(buildingID);

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
                                    buttonComponent.onClick.Invoke();
                                }
                            }
                        }
                        else
                        {
                            Debug.LogError("building_Inventory nicht zugewiesen!");
                        }
                    }
                    else
                    {
                        Debug.LogError("Ungültige buildingID: " + buildingID);
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
