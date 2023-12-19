using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Range(1,10)]
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifetime = 5f;

    private Rigidbody2D rb;
    
    private void Start()
    {
        //Bullet will shoot in the direction that it's rotated to. -90 = shoot right
        rb = GetComponent<Rigidbody2D>();
        transform.Rotate(0, 0, -90);
        Destroy(gameObject, lifetime);
    }

    private void FixedUpdate()
    {
        rb.velocity = transform.up * speed;
    }
}
