using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Range(1,10)]
    [SerializeField] private float speed = 10f;
    private float lifetime = 5f;
    public float rayDistance;
    [SerializeField] public float damage;

    private Rigidbody2D rb;
    
    private void Start()
    {
        //Bullet will shoot in the direction that it's rotated to. -90 = shoot right
        rb = GetComponent<Rigidbody2D>();
        transform.Rotate(0, 0, -90);
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        // if collision with Enemy is detected, deal damage and despawn bullet
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.up, rayDistance);
        if (hitInfo.collider != null)
        {
            if (hitInfo.collider.CompareTag("EnemyUnit"))
            {
                GameObject targetEnemy = hitInfo.collider.gameObject;
                Enemy targetEnemyScript = targetEnemy.GetComponent<Enemy>();
                targetEnemyScript.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = transform.up * speed;
    }
}
