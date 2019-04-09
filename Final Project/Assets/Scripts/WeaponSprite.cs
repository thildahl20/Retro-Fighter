using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSprite : MonoBehaviour
{
    [HideInInspector] public int uses;
    public int maxUses;

    //[HideInInspector] public bool newWeapon = true;


    //public int weaponType;

    //public WeaponPickup weaponPickup;

    private void Start()
    {
        uses = maxUses;
        //newWeapon = true;
    }
}
