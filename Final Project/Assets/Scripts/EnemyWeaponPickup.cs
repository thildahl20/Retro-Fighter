using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponPickup : WeaponPickup
{
    private void Start()
    {
        activeSprite = noWeapon;
        enemyAgent = GetComponent<EnemyAgent>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Mace")
        {
            activeSprite.gameObject.SetActive(false);
            activeSprite = maceSprite;
            SwitchWeapons(collision.gameObject);
            enemyAgent.activeWeapon = (int)theWeapons.mace;
        }
        else if (collision.gameObject.tag == "Grenade")
        {
            activeSprite.gameObject.SetActive(false);
            activeSprite = grenadeSprite;
            SwitchWeapons(collision.gameObject);
            enemyAgent.activeWeapon = (int)theWeapons.grenade;
        }
        else if (collision.gameObject.tag == "Boomerang")
        {
            enemyAgent.thrownWeapons.Remove(collision.transform);
            activeSprite.gameObject.SetActive(false);
            activeSprite = boomerangSprite;
            SwitchWeapons(collision.gameObject);
            enemyAgent.activeWeapon = (int)theWeapons.boomerang;
        }
        else if (collision.gameObject.tag == "Bow")
        {
            activeSprite.gameObject.SetActive(false);
            activeSprite = bowSprite;
            SwitchWeapons(collision.gameObject);
            enemyAgent.activeWeapon = (int)theWeapons.bow;
        }
        else if (collision.gameObject.tag == "Spear")
        {
            activeSprite.gameObject.SetActive(false);
            activeSprite = spearSprite;
            SwitchWeapons(collision.gameObject);
            enemyAgent.activeWeapon = (int)theWeapons.spear;
        }
        else if (collision.gameObject.tag == "Gun")
        {
            activeSprite.gameObject.SetActive(false);
            activeSprite = gunSprite;
            SwitchWeapons(collision.gameObject);
            enemyAgent.activeWeapon = (int)theWeapons.gun;
        }
    }

    void SwitchWeapons(GameObject toDestroy)
    {
        enemyAgent.weapons.Remove(toDestroy.transform);
        activeSprite.gameObject.SetActive(true);
        Destroy(toDestroy);
    }
}