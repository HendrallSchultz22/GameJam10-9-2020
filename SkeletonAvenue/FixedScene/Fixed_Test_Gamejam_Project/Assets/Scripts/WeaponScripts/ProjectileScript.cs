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
        rb.AddForce(Vector3.forward * MoveSpeed);
        //rb.velocity = transform.forward * MoveSpeed;
    }

    void Update()
    {
        StartCoroutine(Despawn());
    }

    void FixedUpdate()
    {
        rb.AddForce(transform.forward * MoveSpeed);
        //rb.velocity = transform.forward * MoveSpeed * slow;
    }
    void OnCollisionEnter(Collision other)
    {
        EnemyStats ES = other.collider.gameObject.GetComponentInParent<EnemyStats>();
        if (ES)
        {
            ES.DoDamage(Damage);
            //SendMessage("DoDamage", Damage, SendMessageOptions.DontRequireReceiver);
            Destroy(gameObject);
            Debug.Log("Hit");
        }
           
    }

    IEnumerator Despawn()
    {
        yield return new WaitForSeconds(LifeTime);
        Destroy(gameObject);
    }
   
}