using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseWeaponScript : MonoBehaviour
{
    [SerializeField] public List<WeaponStats> weaponList = new List<WeaponStats>();
    public static float AmmoLeft;
    public static float AmmoMaX;

    public Text ammunitionLeft; // Used for UI element reference in scene to discern the amount of ammo or uses left on a weapon.
    public Text ammunitionTotal; // Used for UI element reference in scene to discern the total amount of ammo for a weapon / uses on weapon.

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

        AmmoLeft = AmmoMaX;

    }

   
    void Update()
    {
        ammunitionLeft.text = "#" + AmmoLeft;
        ammunitionTotal.text = "#" + AmmoMaX;
    }

    void SetDmg()
    {

    }

    [System.Serializable]
    public class WeaponStats
    {
        public string WeaponName;
        public float weaponDMG;
        public float weaponAmmo;
    }
}
