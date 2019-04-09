using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BowSprite : WeaponSprite
{
    public GameObject upArrow;
	public GameObject straightArrow;
	public GameObject downArrow;
    public int throwForceX;
   
    //public int maxUses;
    public Text arrowCount;

    public EnemyAgent enemyAgent;
    public EnemyWeaponPickup enemyWeaponPickup;

    public Player player;
    public WeaponPickup weaponPickup;

    public NoWeapon noWeapon;

    public void OnEnable() //called whenever the script is enabled or reenabled
    {
        //uses = maxUses;
        uses = weaponPickup.uses;

        arrowCount.text = "Arrows:\n" + uses;

        //newWeapon = false;
    }

    public void OnDisable()
    {
        arrowCount.text = "";

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
         if (uses <= 0)
        {
            weaponPickup.uses = 0;
            this.gameObject.SetActive(false);
            arrowCount.text = "";
        }
    }

    public void Shoot(Character thrower)
    {

        //If switching back to the weapon from punch after using all options


        uses--;

        arrowCount.text = "Arrows:\n" + uses;
        int throwRight = 1;
        if (!thrower.facingRight)
            throwRight = -1;
			
		//This shoots 3 arrows with the same horizontal speed and different vertical speeds
        GameObject newArrow1 = Instantiate(upArrow, thrower.transform.position + new Vector3((float)1 * throwRight, (float).5), Quaternion.identity);
	    GameObject newArrow2 = Instantiate(straightArrow, thrower.transform.position + new Vector3((float)1 * throwRight, 0f), Quaternion.identity);
        GameObject newArrow3 = Instantiate(downArrow, thrower.transform.position + new Vector3((float)1 * throwRight, (float)-.5), Quaternion.identity);

            enemyAgent.thrownWeapons.Add(newArrow1.transform);
            enemyAgent.thrownWeapons.Add(newArrow2.transform);
            enemyAgent.thrownWeapons.Add(newArrow3.transform);

        if (throwRight == -1)
        {
            Vector3 theScale1 = newArrow1.transform.localScale;
            theScale1.x *= -1;
            newArrow1.transform.localScale = theScale1;

            Vector3 theScale2 = newArrow2.transform.localScale;
            theScale2.x *= -1;
            newArrow2.transform.localScale = theScale2;

            Vector3 theScale3 = newArrow3.transform.localScale;
            theScale3.x *= -1;
            newArrow3.transform.localScale = theScale3;
        }
        //newArrow1.tag = "ThrownWeapon";        
        GetComponent<Animator>().SetTrigger("Shoot");

        //Disable gravity for all arrows
        newArrow1.GetComponent<Rigidbody2D>().gravityScale = 0f;
        newArrow2.GetComponent<Rigidbody2D>().gravityScale = 0f;
        newArrow3.GetComponent<Rigidbody2D>().gravityScale = 0f;

        //To shoot arrows at a 45 degree angle, y force must be the same as x force
        newArrow1.GetComponent<Rigidbody2D>().AddForce(new Vector2(throwForceX * throwRight, throwForceX));
		newArrow2.GetComponent<Rigidbody2D>().AddForce(new Vector2(throwForceX * throwRight, 0));
		//To make same velocity vectors, multiply newArrow2 by sqrt(2) 
		newArrow3.GetComponent<Rigidbody2D>().AddForce(new Vector2(throwForceX * throwRight, -throwForceX));

        if (uses == 0)
        {
            weaponPickup.uses = 0;
            this.gameObject.SetActive(false);
            arrowCount.text = "";
        }

    }
}
