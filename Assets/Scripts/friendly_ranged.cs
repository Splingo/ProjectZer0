using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class friendly_ranged : BaseUnit_Script
{
    new private float attackRange = 10f;
    new private float attackDamage = 2f;
    public GameObject bullet;

    // Update is called once per frame
    private void Update()
    {
        DetectEnemyUnit();
        if (IsTargetInRange())
        {
            if (waiting == false)
            {
                StartCoroutine(AttackWithDelay());
                waiting = true;
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
        newBulletScript.sourceUnit = gameObject;

        yield return new WaitForSeconds(attackSpeed);
        waiting = false;
    }
    new protected bool IsTargetInRange()
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

    new protected void DetectEnemyUnit()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(
            transform.position,
            new Vector2(attackRange, attackRange),
            0f
        );

        // Reset the target enemy
        targetEnemyUnit = null;

        float closestDistance = float.MaxValue;

        foreach (Collider2D collider in colliders)
        {
            // Check if the collider is an enemy unit with the correct tag and layer
            if (collider.CompareTag("EnemyUnit") && gameObject.layer == collider.gameObject.layer)
            {
                float distance = Vector2.Distance(transform.position, collider.transform.position);
                // Set closest enemy as targetEnemy
                if (distance < closestDistance) {
                    closestDistance = distance;
                    // Set the detected enemy unit as the target
                    targetEnemyUnit = collider.gameObject;
                }
            }
        }
    }
}



