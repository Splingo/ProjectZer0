using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Range(1,10)]
    [SerializeField] private float speed = 10f;
    private float lifetime = 5f;
    public float rayDistance;
    public GameObject sourceUnit;
    [SerializeField] public float damage;

    private Rigidbody2D rb;
    
    private void Start()
    {
        //Bullet will shoot in the direction that it's rotated to. -90 = shoot right
        rb = GetComponent<Rigidbody2D>();
        transform.Rotate(0, 0, -90);
        Destroy(gameObject, lifetime);
        gameObject.layer = sourceUnit.layer;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyUnit"))
        {
            Enemy targetEnemyScript = collision.gameObject.GetComponent<Enemy>();
            targetEnemyScript.TakeDamage(damage);
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = transform.up * speed;
    }
}
