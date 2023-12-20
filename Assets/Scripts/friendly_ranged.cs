using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class friendly_ranged : BaseUnit_Script
{
    private float attackRange = 10f;
    private float attackDamage = 2f;
    public GameObject bullet;

    // Update is called once per frame
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

        // telling the bullet how much damage it can cause and spawn it
        GameObject newBullet = Instantiate(bullet, transform.position, Quaternion.identity);
        Bullet newBulletScript = newBullet.GetComponent<Bullet>();
        newBulletScript.damage = attackDamage;
        newBulletScript.rayDistance = attackRange;

        // If the script is found, deal damage
        //if (enemyTargetScript != null)
        //{
        //    enemyTargetScript.TakeDamage(attackDamage);
        //}
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
}



