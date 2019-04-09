using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrenadeSprite : WeaponSprite
{
    public GameObject grenade;

    public int throwForceX, throwForceY;

    //public int maxUses;
    public Text grenadeCount;

    public EnemyAgent enemyAgent;
    public EnemyWeaponPickup enemyWeaponPickup;

    public Player player;
    public WeaponPickup weaponPickup;

    public NoWeapon noWeapon;

    public void OnEnable() //called whenever the script is enabled or reenabled
    {
        //uses = maxUses;
        uses = weaponPickup.uses;

        grenadeCount.text = "Grenades:\n" + uses;
    }

    public void OnDisable()
    {
        grenadeCount.text = "";

        if (transform.parent.tag == "Enemy")
        {
            enemyAgent.activeWeapon = -1;
            enemyWeaponPickup.activeSprite = noWeapon;
        }
        else
        {
            player.activeWeapon = -1;
            weaponPickup.activeSprite = noWeapon;
        }
    }

    private void Update()
    {
        //If switching back to the weapon from punch after using all options
        if (uses <= 0)
        {
            weaponPickup.uses = 0;
            this.gameObject.SetActive(false);
            grenadeCount.text = "";
        }
    }

    public void Throw(Character thrower)
    {


        uses--;

        grenadeCount.text = "Grenades:\n" + uses;
        int throwRight = 1;
        if (!thrower.facingRight)
            throwRight = -1;

        GameObject newGrenade = Instantiate(grenade, thrower.transform.position + new Vector3((float)1 * throwRight, (float).5), Quaternion.identity);
        enemyAgent.thrownWeapons.Add(newGrenade.transform);

        newGrenade.tag = "ThrownWeapon";
        newGrenade.GetComponent<Animator>().SetTrigger("Thrown");
        newGrenade.GetComponent<Rigidbody2D>().AddForce(new Vector2(throwForceX * throwRight, throwForceY));

        if (uses == 0)
           {
             weaponPickup.uses = 0;
             this.gameObject.SetActive(false);
             grenadeCount.text = "";
           }
    }
}
