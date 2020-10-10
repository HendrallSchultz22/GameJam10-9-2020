using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bone : MonoBehaviour
{
    bool active = true;
    public float damage = 10;
    [SerializeField]
    [Tooltip("How many seconds after it make its first collision before the object despawns")]
    float despawnTime = 1;

    private void OnCollisionEnter(Collision collision)
    {
        if (active)
        {
            if (collision.collider.CompareTag("Player"))
            {
                collision.collider.gameObject.SendMessage("DoDamage", damage, SendMessageOptions.DontRequireReceiver);
            }
            StartCoroutine(Despawn());
        }

        active = false;
    }

    IEnumerator Despawn()
    {
        yield return new WaitForSeconds(despawnTime);
        Destroy(gameObject);
    }
}
