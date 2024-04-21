using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Range(1,10)]
    [SerializeField] private float speed = 10f;
    private float lifetime = 5f;
    public float rayDistance;
    public GameObject sourceUnit;
    [SerializeField] public float damage;
    public enum bulletDirection { left, right };
    public enum bulletSender { friendly, enemy };
    private string targetTag;

    private Rigidbody2D rb;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifetime);
        gameObject.layer = sourceUnit.layer;
    }

    public void SetDirection(bulletDirection direction)
    {
        //Bullet will shoot in the direction that it's rotated to. -90 = shoot right
        if (direction == bulletDirection.left)
            transform.Rotate(0, 0, 90);
        else if (direction == bulletDirection.right)
            transform.Rotate(0, 0, -90);
    }

    // setting Target that the bullet will deal damage on
    public void SetTargetTag(string tag)
    {
        targetTag = tag;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.tag);
        // bullet coming from enemy
        if (targetTag == "FriendlyUnit" && collision.gameObject.CompareTag("FriendlyUnit"))
        {
            BaseUnit_Script baseUnitScript = collision.gameObject.GetComponent<BaseUnit_Script>();
            baseUnitScript.TakeDamage(damage);
            Destroy(gameObject);
        }

        // bullet coming from friendly unit
        if (targetTag == "EnemyUnit" && collision.gameObject.CompareTag("EnemyUnit"))
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
