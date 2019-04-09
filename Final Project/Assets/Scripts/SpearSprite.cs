using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearSprite : WeaponSprite
{
    public GameObject spear;
    public int throwForceX;

    public NoWeapon noWeapon;

    public EnemyAgent enemyAgent;
    public EnemyWeaponPickup enemyWeaponPickup;

    public Player player;
    public WeaponPickup weaponPickup;

    //public int maxUses;

    public void OnEnable()
    {
        //uses = maxUses;
        //newWeapon = false;

        uses = weaponPickup.uses;
    }

    public void OnDisable()
    {
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
        }
    }

    public void Throw(Character thrower)
    {


        uses--;
        int throwRight = 1;
        if (!thrower.facingRight)
            throwRight = -1;

        GameObject newSpear = Instantiate(spear, thrower.transform.position + new Vector3((float)1 * throwRight, (float).3), Quaternion.identity);

        enemyAgent.thrownWeapons.Add(newSpear.transform);

        if (throwRight == -1)
        {
            Vector3 theScale1 = newSpear.transform.localScale;
            theScale1.x *= -1;
            newSpear.transform.localScale = theScale1;
        }

            newSpear.tag = "ThrownWeapon";
        newSpear.GetComponent<Rigidbody2D>().gravityScale = 0f;
        newSpear.GetComponent<Rigidbody2D>().AddForce(new Vector2(throwForceX * throwRight, 0));

        if (uses == 0)
        {
            weaponPickup.uses = 0;
            gameObject.SetActive(false);
            //newWeapon = true;
        }
    }
}
