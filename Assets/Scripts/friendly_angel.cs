using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class friendly_angel : BaseUnit_Script
{
    private float healRange = 10f;
    private float healAmount = 2f;
    private new bool waiting = false;

    private void Update()
    {
        if (transform.hasChanged)
        {
            SetOccupiedCells(); // Wenn sich die Position geändert hat, rufe die Funktion auf, um den Layer zu aktualisieren
            transform.hasChanged = false; // Setze transform.hasChanged zurück, um weitere Änderungen zu erkennen
        }
        //DetectFriendlyUnits();
        if (IsAnyFriendlyInRange())
        {
            if (waiting == false)
            {
                StartCoroutine(HealWithDelay());
                waiting = true;
            }
        }
    }

    IEnumerator HealWithDelay()
    {
        HealNearbyUnits();
        yield return new WaitForSeconds(attackSpeed);
        waiting = false;
    }

    private void HealNearbyUnits()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, healRange);

        foreach (Collider2D collider in colliders)
        {
            // Check if the collider is a friendly unit with the correct tag and layer
            if (collider.CompareTag("FriendlyUnit") && gameObject.layer == collider.gameObject.layer)
            {
                BaseUnit_Script friendlyUnitScript = collider.GetComponent<BaseUnit_Script>();
                if (friendlyUnitScript != null)
                {
                    friendlyUnitScript.Heal(healAmount);
                }
            }
        }
    }

    protected bool IsAnyFriendlyInRange()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, healRange);

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("FriendlyUnit") && gameObject.layer == collider.gameObject.layer)
            {
                return true;
            }
        }
        return false;
    }


    new public void TakeDamage(float damage)
    {
        currentHP -= damage;

        // Update the HP bar
        UpdateHPBar();

        if (currentHP <= 0f)
        {
            // Implement logic for enemy death
            Destroy(gameObject);
        }
    }

    //private void DetectFriendlyUnits()
    //{
    //    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, healRange);

    //    // Reset the target enemy
    //    targetEnemyUnit = null;

    //    float closestDistance = float.MaxValue;

    //    foreach (Collider2D collider in colliders)
    //    {
    //        // Check if the collider is a friendly unit with the correct tag and layer
    //        if (collider.CompareTag("FriendlyUnit") && gameObject.layer == collider.gameObject.layer)
    //        {
    //            float distance = Vector2.Distance(transform.position, collider.transform.position);
    //            // Set closest friendly unit as targetFriendlyUnit
    //            if (distance < closestDistance)
    //            {
    //                closestDistance = distance;
    //                // Set the detected friendly unit as the target
    //                targetFriendlyUnit = collider.gameObject;
    //            }
    //        }
    //    }
    //}
}
