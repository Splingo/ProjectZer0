using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUnit_Script : MonoBehaviour
{
    public int healthPoints;
    public int defense;
    public int attackDamage;

    public float attackSpeed;
    public float attackRange = 2f;

    private Transform targetEnemy;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.tag = "FriendlyUnit";
    }

    // Update is called once per frame
    void Update()
    {
        if (targetEnemy == null)
        {
            DetectEnemiesInAttackRange();
        }

        if (targetEnemy != null)
        {
            Debug.Log("Target enemy not null");
            if (IsEnemyInAttackRange(targetEnemy))
            {
                Debug.Log("Enemy in attack range");
                Attack();
            }
        }
    }

    void Attack()
{
    Debug.Log("Friendly: Attacking!");
}

    void DetectEnemiesInAttackRange()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackRange);

        foreach (Collider2D collider in colliders)
        {
            if (collider != null && collider.CompareTag("EnemyUnit"))
            {
                Enemy enemyScript = collider.GetComponent<Enemy>();

                if (enemyScript != null)
                {
                    Vector2 enemyGridPosition = enemyScript.GetGridPosition();

                    if (Mathf.RoundToInt(transform.position.y) == Mathf.RoundToInt(enemyGridPosition.y))
                    {
                        targetEnemy = collider.transform;
                        Debug.Log("Target enemy set: " + targetEnemy.name);
                        break;
                    }
                }
            }
        }
    }
    bool IsEnemyInAttackRange(Transform enemyTransform)
    {
        float distanceToEnemy = Vector3.Distance(transform.position, enemyTransform.position);
        Debug.Log("Distance to enemy: " + distanceToEnemy);
        return distanceToEnemy <= attackRange;
    }
}
