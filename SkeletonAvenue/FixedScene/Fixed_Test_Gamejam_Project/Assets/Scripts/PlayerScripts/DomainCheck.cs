using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DomainCheck : MonoBehaviour
{
    public PlayerStats player;
    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.tag == "Domain")
        {
            player.EnduranceRegen();
        }
        if (other.gameObject.tag == "NotDomain")
        {
            player.DoDamage(0.2f);
        }
    }

}
