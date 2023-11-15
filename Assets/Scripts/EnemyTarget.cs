using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTarget : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // remove enemy from canvas on collision
        if (other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
        }
    }
}
