using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Rigidbody))] //requires any game model we use for the player to have rigidbody, as it is a neccesity.
public class PlayerStats : MonoBehaviour
{
    public Rigidbody rb; // Fundamental Reference for Rigidbody for future use.

    public PlayerController player;



    public Slider healthVisual; // Used for UI element reference in scene for health.
    public Slider enduranceVisual; // Used for UI element reference in scene for endurance.


    [Range(0, 100)] // max range for the health points of the player.
    public float playerHealth; // actual number of health points that the player has, will be updated in this script later and is alterable. 
    public float playerMaxHealth;// Total Starting number or maximum health points the player can have at one time.

    [Range(0, 200)] // max range for the endurance points of the player.
    public float playerEndurance; // actual amount of encurance points the player has, and will be updated through interactions in this script.
    public float playerMaxEndurance; // total starting and maximum number of endurance points the player can have at one time.

    public GameObject HitEffect; // storage point for the aesthetic response to the player being hit I.E blood or other colored effects on taking damage.

    void Awake()
    {
        playerMaxHealth = 100;
        playerHealth = playerMaxHealth;
        playerMaxEndurance = 200;
        playerEndurance = 200;



        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        healthVisual.maxValue = playerMaxHealth;
        enduranceVisual.maxValue = playerMaxEndurance;


    }


    void Update()
    {
        healthVisual.value = playerHealth;
        enduranceVisual.value = playerEndurance;


    }
    public void IsDead()
    {
        playerHealth = 0;
        playerEndurance = 0;
        WavePanel.instance.GameOver();
        rb.freezeRotation = false;      // Allows them to fall over at any angle.

    }

    public void EnduranceRegen()
    {
        if (playerEndurance < playerMaxEndurance - 2)
        {
            playerEndurance += 0.2f;
        }
    }

    public void DoDamage(float damage)
    {
        playerHealth -= damage;
        if (playerHealth <= 0)
        {

            IsDead();

        }
    }
}

