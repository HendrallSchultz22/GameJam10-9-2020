﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Rigidbody))] //requires any game model we use for the player to have rigidbody, as it is a neccesity.
public class PlayerStats : MonoBehaviour
{
    Rigidbody rb; // Fundamental Reference for Rigidbody for future use.

    PlayerController player; //Reference to the player controller script for neccesery state changes.
    
    //public Slider healthVisual; // Used for UI element reference in scene for health.
    //public Slider enduranceVisual; // Used for UI element reference in scene for endurance.
    //public TextMeshProUGUI playerName; // Can be used as a reference to record the name of the controlled character for flavor.
    //public TextMeshProUGUI ammunitionLeft; // Used for UI element reference in scene to discern the amount of ammo or uses left on a weapon.
    //public TextMeshProUGUI ammunitionTotal; // Used for UI element reference in scene to discern the total amount of ammo for a weapon / uses on weapon.

    //public GameObject playerNameOb;

    //[Range(0, 100)] // max range for the health points of the player.
    //public float playerHealth; // actual number of health points that the player has, will be updated in this script later and is alterable. 
    //public int playerMaxHealth;// Total Starting number or maximum health points the player can have at one time.

    //[Range(0, 200)] // max range for the endurance points of the player.
    //public float playerEnderance; // actual amount of encurance points the player has, and will be updated through interactions in this script.
    //public float playerMaxEndurance; // total starting and maximum number of endurance points the player can have at one time.

    //public GameObject HitEffect; // storage point for the aesthetic response to the player being hit I.E blood or other colored effects on taking damage.

    void Awake()
    {   
        //playerMaxHealth = 100;                                    
        //playerHealth = playerMaxHealth;                                         
        //playerMaxEndurance = 200;
        //playerEndurance = 200;                                                      

        //rb = GetComponent<Rigidbody>();                                          
    }

    void Start()
    {
        //Char2Name = g.Char2Nameob.GetComponent<TextMeshProUGUI>();
        //healthVisual.maxValue = playerMaxHealth;
        //enduranceVisual.maxValue = playerMaxEndurance;
    }

    
    void Update()
    {
        
    }
}
