using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class enemy_ghost : Enemy
{
    // Start is called before the first frame update


    private float attackRange = 10f;
    private float attackDamage = 4f;
    private new float attackSpeed = 1.5f;
    public GameObject bullet;
    public Sprite animatedSprite;
    Animator animator;
    public RuntimeAnimatorController animatorController;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    } 
    // Update is called once per frame
    new void Update()
    {
        DetectFriendlyUnit();
        if (targetFriendlyUnit == null)
        {
            DetectFriendlyUnit();
            MoveLeft();
        }
        else
        {
            if (IsTargetInRange())
            {
                if (waiting == false)
                {
                    animator.Play("enemy_ghost_attack");
                    StartCoroutine(AttackWithDelay());
                    waiting = true;
                }
            }
        }
    }

    new IEnumerator AttackWithDelay()
    {
        yield return new WaitForSeconds(0.2f);
        // telling the bullet how much damage it can cause and spawn it
        GameObject newBullet = Instantiate(bullet, transform.position, Quaternion.identity);
        Bullet newBulletScript = newBullet.GetComponent<Bullet>();
        newBulletScript.SetTargetTag("FriendlyUnit");
        newBulletScript.damage = attackDamage;
        newBulletScript.rayDistance = attackRange;
        newBulletScript.speed = 4f;
        newBulletScript.sourceUnit = gameObject;
        newBulletScript.SetDirection(Bullet.bulletDirection.left);
        newBulletScript.SetAnimatedSprite(animatedSprite, new Vector3(0.4f,0.4f,0.4f), animatorController);

        yield return new WaitForSeconds(attackSpeed - 0.2f);
        waiting = false;
    }

    new protected bool IsTargetInRange()
    {
        if (targetFriendlyUnit == null)
            return false;

        float distance = Vector2.Distance(transform.position, targetFriendlyUnit.transform.position);
        return distance <= attackRange;
    }

    protected void DetectFriendlyUnit()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(
            transform.position,
            new Vector2(attackRange, attackRange),
            0f
        );

        // Reset the target enemy
        targetFriendlyUnit = null;

        float closestDistance = float.MaxValue;

        foreach (Collider2D collider in colliders)
        {
            // Check if the collider is an friendly unit with the correct tag and layer
            if (collider.CompareTag("FriendlyUnit") && gameObject.layer == collider.gameObject.layer)
            {
                float distance = Vector2.Distance(transform.position, collider.transform.position);
                // Set closest enemy as targetEnemy
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    // Set the detected enemy unit as the target
                    targetFriendlyUnit = collider.gameObject;
                }
            }
        }
    }
}
