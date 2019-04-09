using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunSprite : WeaponSprite
{
    public GameObject bullet;
    public int throwForceX; //For balancing, recommend faster than bow with less damage and similar number of uses
   
    //public int maxUses;
    public Text bulletCount;

    public EnemyAgent enemyAgent;
    public EnemyWeaponPickup enemyWeaponPickup;

    public Player player;
    public WeaponPickup weaponPickup;

    public NoWeapon noWeapon;

    public void OnEnable() //called whenever the script is enabled or reenabled
    {
        //if (newWeapon)
        //uses = maxUses;
        uses = weaponPickup.uses;
        bulletCount.text = "Bullets:\n" + uses;
        //newWeapon = false;
    }

    public void OnDisable()
    {
        bulletCount.text = "";
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
            bulletCount.text = "";
        }

    }

    public void Shoot(Character player)
    {

        uses--;

        bulletCount.text = "Bullets:\n" + uses;
        int throwRight = 1;
        if (!player.facingRight)
            throwRight = -1;
			
		//This shoots 3 arrows with the same horizontal speed and different vertical speeds
        GameObject newBullet = Instantiate(bullet, player.transform.position + new Vector3((float)1 * throwRight, (float).5), Quaternion.identity);

        enemyAgent.thrownWeapons.Add(newBullet.transform);

        newBullet.tag = "ThrownWeapon";        
        //newBullet.GetComponent<Animator>().SetTrigger("Thrown");
        
		//Disable gravity for all arrows
		newBullet.GetComponent<Rigidbody2D>().gravityScale = 0f;
		
		//To shoot arrows at a 45 degree angle, y force must be the same as x force
		newBullet.GetComponent<Rigidbody2D>().AddForce(new Vector2(throwForceX * throwRight, 0));

        if (uses == 0)
        {
            weaponPickup.uses = 0;
            this.gameObject.SetActive(false);
            bulletCount.text = "";
            //newWeapon = true;
        }
    }
}
