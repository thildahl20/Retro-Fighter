using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public WeaponSprite maceSprite;
    public WeaponSprite grenadeSprite;
    public WeaponSprite boomerangSprite;
    public WeaponSprite bowSprite;
    public WeaponSprite spearSprite;
    public WeaponSprite gunSprite;

    [HideInInspector] public WeaponSprite activeSprite;

    public Player player;

    public EnemyAgent enemyAgent;

    public enum theWeapons { mace, grenade, boomerang, bow, spear, gun };

    public NoWeapon noWeapon;

    //These are used to keep track of data about the previous weapon when punch is temporarily activated
    public string lastWeapon;
    public int uses;

    private void Start()
    {
        activeSprite = noWeapon;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //activeSprite.newWeapon = true;

        if (collision.gameObject.tag == "Mace")
        {
            lastWeapon = "Mace";
            uses = 0;
            activeSprite.gameObject.SetActive(false);
            activeSprite = maceSprite;
            SwitchWeapons(collision.gameObject);
            player.activeWeapon = (int)theWeapons.mace;
        }
        else if(collision.gameObject.tag == "Grenade")
        {
            lastWeapon = "Grenade";
            uses = grenadeSprite.maxUses;
            activeSprite.gameObject.SetActive(false);
            activeSprite = grenadeSprite;
            SwitchWeapons(collision.gameObject);
            player.activeWeapon = (int)theWeapons.grenade;
        }
        else if(collision.gameObject.tag == "Boomerang")
        {
            lastWeapon = "Boomerang";
            uses = 0;
            enemyAgent.thrownWeapons.Remove(collision.transform);
            activeSprite.gameObject.SetActive(false);
            activeSprite = boomerangSprite;
            SwitchWeapons(collision.gameObject);
            player.activeWeapon = (int)theWeapons.boomerang;
        }
        else if (collision.gameObject.tag == "Bow")
        {
            lastWeapon = "Bow";
            uses = bowSprite.maxUses;
            activeSprite.gameObject.SetActive(false);
            activeSprite = bowSprite;
            SwitchWeapons(collision.gameObject);
            player.activeWeapon = (int)theWeapons.bow;
        }
        else if (collision.gameObject.tag == "Spear")
        {
            lastWeapon = "Spear";
            uses = spearSprite.maxUses;
            activeSprite.gameObject.SetActive(false);
            activeSprite = spearSprite;
            SwitchWeapons(collision.gameObject);
            player.activeWeapon = (int)theWeapons.spear;
        }
        else if (collision.gameObject.tag == "Gun")
        {
            lastWeapon = "Gun";
            uses = gunSprite.maxUses;
            activeSprite.gameObject.SetActive(false);
            activeSprite = gunSprite;
            SwitchWeapons(collision.gameObject);
            player.activeWeapon = (int)theWeapons.gun;
        }
    }

    public void ReactivateWeapon(string name)
    {
        if (name == "Mace")
        {
            //activeSprite.gameObject.SetActive(false);
            activeSprite = maceSprite;
            player.activeWeapon = (int)theWeapons.mace;
        }
        else if (name == "Grenade")
        {
            //activeSprite.gameObject.SetActive(false);
            activeSprite = grenadeSprite;
            player.activeWeapon = (int)theWeapons.grenade;
            grenadeSprite.uses = uses;
        }
        else if (name == "Boomerang")
        {
            //activeSprite.gameObject.SetActive(false);
            activeSprite = boomerangSprite;
            player.activeWeapon = (int)theWeapons.boomerang;
        }
        else if (name == "Bow")
        {
            //activeSprite.gameObject.SetActive(false);
            activeSprite = bowSprite;
            player.activeWeapon = (int)theWeapons.bow;
            bowSprite.uses = uses;
        }
        else if (name == "Spear")
        {
            //activeSprite.gameObject.SetActive(false);
            activeSprite = spearSprite;
            player.activeWeapon = (int)theWeapons.spear;
            spearSprite.uses = uses;
        }
        else if (name == "Gun")
        {
            //activeSprite.gameObject.SetActive(false);
            activeSprite = gunSprite;
            player.activeWeapon = (int)theWeapons.gun;
            gunSprite.uses = uses;
        }

        activeSprite.gameObject.SetActive(true);
    }

    void SwitchWeapons(GameObject toDestroy)
    {
        enemyAgent.weapons.Remove(toDestroy.transform);
        activeSprite.gameObject.SetActive(true);
        Destroy(toDestroy);
    }
}
