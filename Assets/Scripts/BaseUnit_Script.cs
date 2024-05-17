using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseUnit_Script : MonoBehaviour
{
    public  float maxHP = 10f;
    public float currentHP;
    public int defense;
    public float attackDamage = 1f;

    public float attackSpeed = 1f;
    public float attackRange = 1.05f;

    public GameObject targetEnemyUnit;

    protected GameObject hpBarPrefab;

    protected GameObject hpBarInstance;

    public bool waiting = false;
    public GridManager gridManager;
    public List<Vector3Int> occupiedCells; // Liste der belegten Zellen (Zeile, Spalte)
    public List<Vector3Int> previousOccupiedCells; // Liste der belegten Zellen (Zeile, Spalte)
    public List<Vector3Int> hoveringOccupiedCells; // Liste der belegten Zellen (Zeile, Spalte)
    public List<Vector3Int> GetOccupiedCells(Vector3Int center)
    {
        OccupyCells(center);
        return occupiedCells;
    }
    public List<Vector3Int> ReturnOccupiedCells()
    {
        return occupiedCells;
    }

    protected void Start()
    {
        currentHP = maxHP;
        gameObject.tag = "FriendlyUnit";
        SetOccupiedCells();
        CreateHPBar(); // Move CreateHPBar to Start
    }

    private void Update()
    {
        if (transform.hasChanged)
        {
            SetOccupiedCells(); // Wenn sich die Position geändert hat, rufe die Funktion auf, um den Layer zu aktualisieren
            transform.hasChanged = false; // Setze transform.hasChanged zurück, um weitere Änderungen zu erkennen
        }
       
        if (targetEnemyUnit == null)
        {
            DetectEnemyUnit();
        }
        else
        {
            if (IsTargetInRange())
            {
                if (waiting == false)
                {
                    StartCoroutine(AttackWithDelay());
                    waiting = true;
                }

            }
        }
    }
    IEnumerator AttackWithDelay()
    {

        Enemy enemyTargetScript = targetEnemyUnit.GetComponent<Enemy>();

        // If the script is found, deal damage
        if (enemyTargetScript != null)
        {
            enemyTargetScript.TakeDamage(attackDamage);
        }
        yield return new WaitForSeconds(attackSpeed);
        waiting = false;
    }
    protected bool IsTargetInRange()
    {
        if (targetEnemyUnit == null)
            return false;

        float distance = Vector2.Distance(transform.position, targetEnemyUnit.transform.position);
        return distance <= attackRange;
    }

    private void CreateHPBar()
    {
        if (hpBarPrefab != null)
        {
            hpBarInstance = Instantiate(hpBarPrefab, transform.position + new Vector3(0, 0.7f, 0), Quaternion.identity);
            hpBarInstance.transform.SetParent(transform);
            UpdateHPBar(); // Call UpdateHPBar immediately after creating hpBarInstance
        }
    }

    private void UpdateHPBar()
    {
        if (hpBarInstance != null)
        {
            Image hpBarImage = hpBarInstance.GetComponent<Image>();

            if (hpBarImage != null)
            {
                float fillAmount = currentHP / maxHP;
                hpBarImage.fillAmount = fillAmount;
            }
        }
    }

    public void TakeDamage(float damage)
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

    protected void DetectEnemyUnit()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(
            transform.position,
            new Vector2(attackRange, attackRange),
            0f
        );

        // Reset the target enemy
        targetEnemyUnit = null;

        foreach (Collider2D collider in colliders)
        {
            float distance = Vector2.Distance(transform.position, collider.transform.position);

            // Check if the collider is an enemy unit with the correct tag and layer
            if (collider.CompareTag("EnemyUnit") && gameObject.layer == collider.gameObject.layer)
            {
                // Set the detected enemy unit as the target
                targetEnemyUnit = collider.gameObject;
                break; // Exit the loop after finding the first enemy unit
            }
        }
    }
    protected void SetOccupiedCells()
    {
        // Überprüfe, ob der GridManager verfügbar ist
        if (gridManager != null)
        {

            Vector3Int gridPosition = gridManager.gridTilemap.WorldToCell(transform.position);
            List<Vector3Int> occupiedCells = new List<Vector3Int>();
            occupiedCells.Add(gridPosition);
            //gridManager.OccupyCells(occupiedCells);

            int yPosition = gridPosition.y;
            int layerNumber;

            // Bestimme den Layer basierend auf der Y-Position
            if (yPosition >= 2)
            {
                layerNumber = 0;
            }
            else if (yPosition == 1)
            {
                layerNumber = 1;
            }
            else if (yPosition == 0)
            {
                layerNumber = 2;
            }
            else if (yPosition == -1)
            {
                layerNumber = 3;
            }
            else if (yPosition == -2)
            {
                layerNumber = 4;
            }
            else if (yPosition == -3)
            {
                layerNumber = 5;
            }
            else
            {
                // Wenn die Y-Position nicht in den genannten Bereichen liegt, setze auf default Layer
                layerNumber = 0; // Ändere dies entsprechend deiner Anforderungen
            }

            // Setze den Layer des Spielobjekts entsprechend der Y-Position
            gameObject.layer = LayerMask.NameToLayer("ROW " + layerNumber);
        }
        else
        {
            Debug.LogWarning("GridManager-Referenz in BaseUnit_Script ist nicht zugewiesen!");
        }
    }
    public void OccupyCells(Vector3Int beginCell)
    {
        this.occupiedCells = new List<Vector3Int>();
        occupiedCells.Add(beginCell);
    }

    public void UpdateAttackDamage(float newValue)
    {
        attackDamage = newValue;
    }

    public void UpdateMaxHP(float newValue)
    {
        maxHP = newValue;
        currentHP = maxHP; // Setze auch currentHP auf den neuen Maximalwert
        UpdateHPBar(); // Aktualisiere die HP-Leiste, um die Änderung anzuzeigen
    }

    public void UpdateAttackRange(float newValue)
    {
        attackRange = newValue;
    }

    public void UpdateAttackSpeed(float newValue)
    {
        attackSpeed = newValue;
    }
}