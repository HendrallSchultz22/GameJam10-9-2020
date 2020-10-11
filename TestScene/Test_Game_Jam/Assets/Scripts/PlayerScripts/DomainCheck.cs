using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DomainCheck : MonoBehaviour
{
    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.tag == "Domain")
        {
            PlayerStats.EnduranceRegen();
        }
        if (other.gameObject.tag != "Domain")
        {
            PlayerStats.DoDamage(0.2f);
        }
    }

}
