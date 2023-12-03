using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private System.Random rand = new System.Random();
    public float movementSpeed;
    public int healthPoints;
    public int defense;
    public int attackDamage;

    public float attackSpeed;
    public float attackRange = 1f;
    public Transform targetUnit;
    private bool isMoving = true; // Flag to control movement

    // set MovementSpeed to random value between 1(included) and 3(excluded)
    private void Start()
    {
        gameObject.tag = "EnemyUnit";
        movementSpeed = (float)rand.NextDouble() * (3 - 1) + 1;
    }


    private void Update()
    {
        if (targetUnit == null)
        {
            if (isMoving)
            {
                MoveLeft();
                UpdateRangeIndicator();
            }
            DetectFriendlyUnit();
        }
        else
        {
            // If there's a target enemy within attack range, attack
            if (IsEnemyInAttackRange(targetUnit))
            {
                Attack();
            }
        }
    }
    private void UpdateRangeIndicator()
    {
        // Raycast to detect mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        // Check if the hit collider is the same as the collider of this unit
        if (hit.collider != null && hit.collider.gameObject == gameObject)
        {
            ShowRangeIndicator();
        }
        else
        {
            HideRangeIndicator();
        }
    }

    private void ShowRangeIndicator()
    {
        // Get the AttackRangeIndicator script attached to the enemy
        AttackRangeIndicatorForEnemyUnits indicatorScript = GetComponent<AttackRangeIndicatorForEnemyUnits>();

        // Check if the indicator script is not null
        if (indicatorScript != null)
        {
            indicatorScript.ShowIndicator();
        }
    }

    private void HideRangeIndicator()
    {
        // Get the AttackRangeIndicator script attached to the enemy
        AttackRangeIndicatorForEnemyUnits indicatorScript = GetComponent<AttackRangeIndicatorForEnemyUnits>();

        // Check if the indicator script is not null
        if (indicatorScript != null)
        {
            indicatorScript.HideIndicator();
        }
    }

    private void MoveLeft()
    {
        // Move Enemy to the left
        transform.Translate(Vector3.left * movementSpeed * Time.deltaTime);
    }

    private void DetectFriendlyUnit()
{
    // Check if the targetUnit is already set (meaning the friendly unit is already in attack range)
    if (targetUnit == null)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackRange);

        foreach (Collider2D collider in colliders)
        {
            if (collider != null && collider.CompareTag("FriendlyUnit"))
            {
                StopMoving();
                        break;
            }
        }
    }
}
    private void StopMoving()
    {
        // Add any additional logic you need when the enemy stops moving
        isMoving = false; // Stop further movement
        Debug.Log("Enemy stopped moving!");
    }
    private void Attack()
    {
        // Add your attack logic here
        Debug.Log("Enemy: Attacking!");
    }

    private bool IsEnemyInAttackRange(Transform enemyTransform)
    {
        float distanceToEnemy = Vector3.Distance(transform.position, enemyTransform.position);
        return distanceToEnemy <= attackRange;
    }

    //private Vector2 GetGridPosition()
    //{
    //    // Assuming each grid cell is 1 unit in size and starts from (0, 0)
    //    return new Vector2(transform.position.x, transform.position.y);
    //}   
    public Vector2 GetGridPosition()
    {
        // Assuming each grid cell is 100 units in size and starts from (0, 0)
        float gridX = Mathf.Floor(transform.position.x / 100) * 100;
        float gridY = Mathf.Floor(transform.position.y / 100) * 100;

        return new Vector2(gridX, gridY);
    }
}