using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DomainCheck : MonoBehaviour
{
    public PlayerStats player;
    [SerializeField] AudioSource AS;
    [SerializeField] float audioLockoutTime = 2;
    bool audioLock;

    private void Start()
    {
        AS = GetComponent<AudioSource>();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Domain"))
        {
            if (!audioLock)
            {
                AS.Play();
                StartCoroutine(AudioLockout());
            }
            player.regen = false;
            player.toxic = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Domain"))
        {
            player.regen = true;
            player.toxic = false;
        }
    }

    IEnumerator AudioLockout()
    {
        audioLock = true;
        yield return new WaitForSeconds(audioLockoutTime);
        audioLock = false;
    }

}
