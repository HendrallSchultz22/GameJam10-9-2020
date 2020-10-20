using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ProjectileScript : MonoBehaviour
{
    public float MoveSpeed = 30f;
    public float LifeTime = 1.2f;
    public float Damage;
    public float slow = 0.5f;
    
    Rigidbody rb;
 
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.velocity = transform.forward * MoveSpeed;
    }

    void Update()
    {
        Lifetime();
    }

    void FixedUpdate()
    {
        rb.velocity = transform.forward * MoveSpeed * slow;
    }
   
    void Lifetime()
    {
        Destroy(gameObject, LifeTime);
    }
}
