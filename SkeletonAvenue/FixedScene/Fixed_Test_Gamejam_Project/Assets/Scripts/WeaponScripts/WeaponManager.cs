using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponManager : MonoBehaviour
{
    public InputDevice pDevice;

    public Gamepad pDevicePad;

    Animator anim;

    public PlayerStats player;

    public GameObject Wep;

    BaseWeaponScript Weapon;

    void Start()
    {
        anim = GetComponent<Animator>();
    }


    void Update()
    {
        Wep = GameObject.FindWithTag("Weapon");
        Weapon = Wep.GetComponent<BaseWeaponScript>();

        Gamepad[] pads = Gamepad.all.ToArray();
        if (pads.Length < 1)
        {
            Debug.LogError("Connect A Controller!!!");
            return;
        }
        pDevicePad = pads[0];

        if (pDevicePad.rightTrigger.wasPressedThisFrame)
        {
            Attack();
            
        }

        if (pDevicePad.buttonWest.wasPressedThisFrame)
        {
            Reload();


        }
    }

    public void Attack()
    {
        Debug.Log("Attack");
        Weapon.ReadyShot();
        

    }

    public void Reload()
    {
        Debug.Log("Reload");
        if (player.playerEndurance >= 100.0f && BaseWeaponScript.AmmoLeft <= 0)
        {
            anim.SetTrigger("Reload");
            player.playerEndurance -= 100.0f;
            BaseWeaponScript.AmmoLeft = BaseWeaponScript.AmmoMaX;
        }
        

    }
}
