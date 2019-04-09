using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punch : Weapon
{
    public int punchDamage;

    public WeaponPickup weaponPickup;
    //bool punching;

    string oldWeapon;
    int uses;

    Animator punchAnimator;

    void Start()
    {
        punchAnimator = GetComponent<Animator>();
    }
    
    void OnEnable() //temporarily remove the other weapon
    {
        
        oldWeapon = weaponPickup.lastWeapon;
        weaponPickup.uses = weaponPickup.activeSprite.uses;
        weaponPickup.activeSprite.gameObject.SetActive(false);
    }

    void OnDisable()
    {
        //weaponPickup.activeSprite.gameObject.SetActive(true);
        weaponPickup.SendMessage("ReactivateWeapon", oldWeapon);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damage(collision.gameObject, punchDamage);
            
    }

    void Update()
    {
        if (punchAnimator.GetCurrentAnimatorStateInfo(0).IsName("Done"))
        {
            this.gameObject.SetActive(false);
        }
        
    }

}
