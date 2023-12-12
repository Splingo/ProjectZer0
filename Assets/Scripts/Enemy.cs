using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    private System.Random rand = new System.Random();
    private float movementSpeed;
    private float maxHP = 5f;
    private float currentHP;
    private int defense;
    private float attackDamage = 1f;
    private float attackSpeed = 1f;
    private float attackRange = 1.05f;

    public GameObject targetFriendlyUnit;

    public GameObject hpBarPrefab;

    private GameObject hpBarInstance;

    private Collider2D enemyCollider;
    private bool shouldMove = true;

     private bool waiting = false;

    private void Start()
    {
        currentHP = maxHP;
        CreateHPBar();
        gameObject.tag = "EnemyUnit";
        movementSpeed = (float)rand.NextDouble() * (3 - 1) + 1;

        // Assuming you have a Collider2D attached to the enemy
        enemyCollider = GetComponent<Collider2D>();

        // Check if the Collider2D is not null
        if (enemyCollider == null)
        {
            enemyCollider = gameObject.AddComponent<BoxCollider2D>();
        }
    }

    private void Update()
    {
        
        if (targetFriendlyUnit == null)
        {
            DetectFriendlyUnit();
            MoveLeft();
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
    private bool IsTargetInRange()
    {
        if (targetFriendlyUnit == null)
        {
            return false;
        }

        float distance = Vector2.Distance(transform.position, targetFriendlyUnit.transform.position);
        return distance <= attackRange;
    }

    private void CreateHPBar()
    {
        hpBarInstance = Instantiate(hpBarPrefab, transform.position + new Vector3(0, 0.7f, 0), Quaternion.identity);
        hpBarInstance.transform.SetParent(transform);
    }
    private void UpdateHPBar()
    {
        Debug.Log("TEST 1");
        if (hpBarInstance != null)
        {
            Image hpBarImage = hpBarInstance.GetComponent<Image>();
            Debug.Log("TEST 2");
            if (hpBarImage != null)
            {
                Debug.Log("TEST 3");
                float fillAmount = currentHP / maxHP;
                Debug.Log("Fill Amount: " + fillAmount);
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
            EnemyKilled();
        }
    }
    /// <summary>
    /// This function destroys the gameObject and triggers on killed stuff
    /// </summary>
    private void EnemyKilled()
    {
            EventManager.EnemeyKilledEvent.Invoke();
            Destroy(gameObject);
    }

    void MoveLeft()
    {
        if (shouldMove)
        {
            // Move the enemy unit to the left
            transform.Translate(Vector3.left * movementSpeed * Time.deltaTime);
        }
    }


    IEnumerator AttackWithDelay()
{
    BaseUnit_Script friendlyTargetScript = targetFriendlyUnit.GetComponent<BaseUnit_Script>();

    // If the script is found, deal damage
    if (friendlyTargetScript != null)
    {
        friendlyTargetScript.TakeDamage(attackDamage);
    }
    yield return new WaitForSeconds(attackSpeed);
    waiting = false;
}
    void DetectFriendlyUnit()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(
            transform.position,
            enemyCollider.bounds.size,
            0f
        );

        targetFriendlyUnit = null; // Reset the target

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("FriendlyUnit") && gameObject.layer == collider.gameObject.layer)
            {
                // Set the detected friendly unit as the target
                targetFriendlyUnit = collider.gameObject;
                shouldMove = false; // Stop moving
                break; // Exit the loop after finding the first friendly unit
            }
        }

        if (targetFriendlyUnit == null)
        {
            // Resume movement if no friendly unit is detected
            shouldMove = true;
        }
    }
}