using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_bomb : Enemy
{
    new protected float attackDamage = 10;
    new protected float maxHP = 5f;
    Animator animator;
    public RuntimeAnimatorController animatorController;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    new void Update()
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
                if (waiting == false)
                {
                    animator.Play("Enemy_bomb_explosion");
                    StartCoroutine(AttackWithDelay());
                    waiting = true;
                }
            }
        }
    }


    // TODO: bomb should damage more than one target unit. Maybe multiple lanes too?
    new protected IEnumerator AttackWithDelay()
    {
        // wait 2s before explosion
        yield return new WaitForSeconds(0.85f);

        BaseUnit_Script friendlyTargetScript = targetFriendlyUnit.GetComponent<BaseUnit_Script>();

        // If the script is found, deal damage
        if (friendlyTargetScript != null)
        {
            friendlyTargetScript.TakeDamage(attackDamage);
        }
        TakeDamage(maxHP);
        yield return new WaitForSeconds(attackSpeed);
        waiting = false;
    }
}
