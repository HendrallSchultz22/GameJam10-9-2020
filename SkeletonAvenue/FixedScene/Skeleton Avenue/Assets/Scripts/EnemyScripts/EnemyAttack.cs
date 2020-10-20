using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float Damage;
    void OnCollisionEnter(Collision other)
    {
        PlayerStats PS = other.collider.gameObject.GetComponent<PlayerStats>();
        if (PS)
        {
            PS.DoDamage(Damage);
            Debug.Log("Hit");
        }

    }
}
