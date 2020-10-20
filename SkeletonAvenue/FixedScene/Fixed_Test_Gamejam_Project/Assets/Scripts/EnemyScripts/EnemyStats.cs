using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public float health = 5;

    AudioSource AS;
    Animator anim;

    public void Start()
    {
        AS = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
    }

    public void DoDamage(float damage)
    {
        health -= damage;
        if (health <= 0) Die();
    }

    public void Die()
    {
        
        EnemyManager.instance.EnemyDied();
        anim.SetTrigger("Death");
        Destroy(gameObject, 3);
    }
}
