using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseWeaponScript : MonoBehaviour
{
    public static int selectedWeaponIndex;
    [SerializeField] public List<WeaponStats> weaponList = new List<WeaponStats>();
    public static float AmmoLeft;
    public static float AmmoMaX;

    public Text ammunitionLeft; // Used for UI element reference in scene to discern the amount of ammo or uses left on a weapon.
    public Text ammunitionTotal; // Used for UI element reference in scene to discern the total amount of ammo for a weapon / uses on weapon.

    public GameObject BarrelLoc1;
    public GameObject BarrelLoc2;

    Animator anim;

    public GameObject player;
    PlayerStats stats;

    void Awake()
    {
        AmmoMaX = weaponList[0].weaponAmmo;
    }
    void Start()
    {
        player = GameObject.Find("Player");
        stats = player.GetComponent<PlayerStats>();
        anim = GetComponent<Animator>();
        AmmoLeft = AmmoMaX;

    }


    void Update()
    {
        ammunitionLeft.text = "#" + AmmoLeft;
        ammunitionTotal.text = "#" + AmmoMaX;
    }
    public void ReadyShot()
    {
        if (weaponList[selectedWeaponIndex].WeaponName == "SawedOff Shotgun")
        {
            if (AmmoLeft >= 2)
            {
                anim.SetTrigger("Fire");
                AmmoLeft -= 2.0f;
                Shoot();
            }
        }
        else if (weaponList[selectedWeaponIndex].WeaponName == "Nail Gun")
        {
            if (AmmoLeft >= 1)
            {
                AmmoLeft -= 1.0f;
                Shoot();
            }
        }
    }
    public void Shoot()
    {
        if (weaponList[selectedWeaponIndex].WeaponName == "SawedOff Shotgun")
        {
            
            GameObject Shot1 = Instantiate(weaponList[selectedWeaponIndex].BulletPrefab, BarrelLoc1.transform.position, BarrelLoc1.transform.rotation);
            GameObject Shot2 = Instantiate(weaponList[selectedWeaponIndex].BulletPrefab, BarrelLoc2.transform.position, BarrelLoc2.transform.rotation);
        }
        else if (weaponList[selectedWeaponIndex].WeaponName == "Nail Gun")
        {
            GameObject Shot1 = Instantiate(weaponList[selectedWeaponIndex].BulletPrefab, BarrelLoc1.transform.position, BarrelLoc1.transform.rotation);
        }

    }
    public void SwitchWeapon()
    {
        selectedWeaponIndex--;
        if (selectedWeaponIndex > 0)
        {
            selectedWeaponIndex = weaponList.Count - 1;
        }
        selectedWeaponIndex++;
        if (selectedWeaponIndex == weaponList.Count)
        {
            selectedWeaponIndex = 0;
        }
    }


    [System.Serializable]
    public class WeaponStats
    {
        public string WeaponName;
        public float weaponAmmo;
        public GameObject BulletPrefab;
    }
}
