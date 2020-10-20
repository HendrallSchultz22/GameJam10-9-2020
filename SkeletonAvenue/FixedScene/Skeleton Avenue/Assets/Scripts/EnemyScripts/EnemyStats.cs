using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStats : MonoBehaviour
{
    public float health = 5;

    AudioSource AS;
    Animator anim;
    [SerializeField] SkeletonAI AI;

    public void Start()
    {
        AS = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
    }

    public void DoDamage(float damage)
    {
        if (health > 0)
        {
            health -= damage;
            if (health <= 0) Die();
        }
    }

    public void Die()
    {
        AI.enabled = false;
        GetComponent<NavMeshAgent>().SetDestination(transform.position);
        EnemyManager.instance.EnemyDied();
        anim.SetTrigger("Death");
        Destroy(gameObject, 3);
    }
}
