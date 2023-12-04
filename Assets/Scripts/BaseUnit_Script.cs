using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseUnit_Script : MonoBehaviour
{
    private float maxHP = 10f;
    private float currentHP;
    private int defense;
    private float attackDamage = 1f;

    private float attackSpeed = 1f;
    private float attackRange = 1.05f;

    public GameObject targetEnemyUnit;

    public GameObject hpBarPrefab;

    private GameObject hpBarInstance;

    private bool waiting = false;

    private void Start()
    {
        currentHP = maxHP;
        gameObject.tag = "FriendlyUnit";
        CreateHPBar(); // Move CreateHPBar to Start
    }

    private void Update()
    {
        if (targetEnemyUnit == null)
        {
            DetectEnemyUnit();
        }
        else
        {
            if (IsTargetInRange())
            {
                if(waiting == false)
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
    private bool IsTargetInRange()
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

    void DetectEnemyUnit()
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
}