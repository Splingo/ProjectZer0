using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private System.Random rand = new System.Random();
    public float MovementSpeed;
    public float Health;
    public float Range;

    // set MovementSpeed to random value between 1(included) and 3(excluded)
    private void Start()
    {
        MovementSpeed = (float)rand.NextDouble() * (3 - 1) + 1;
    }


    private void Update()
    {
        // move Enemy to the left
        transform.Translate(Vector3.left * MovementSpeed * Time.deltaTime);
    }
}
