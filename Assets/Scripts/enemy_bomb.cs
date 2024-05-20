using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_bomb : Enemy
{
    new protected float attackDamage = 10;

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
                    // TODO: doesn't wait 2 seconds for "explosion" yet
                    StartCoroutine(Wait(2));
                    StartCoroutine(AttackWithDelay());
                    waiting = true;
                }
            }
        }
    }


    // TODO: bomb should damage more than one target unit. Maybe multiple lanes too?
    new protected IEnumerator AttackWithDelay()
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

    protected IEnumerator Wait(float delay)
    {
        yield return new WaitForSeconds(delay);
    }
}
