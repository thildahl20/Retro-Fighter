using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Weapon
{
    public int bulletDamage;
    [HideInInspector] public EnemyAgent enemyAgent;

    public void Start()
    {
        enemyAgent = FindObjectOfType<EnemyAgent>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject newHitObject = collision.gameObject;
        if (newHitObject.tag == "Player" || newHitObject.tag == "Enemy")
        {
			newHitObject.SendMessage("takeDamage", bulletDamage);
		}
        enemyAgent.thrownWeapons.Remove(this.transform);
        Destroy(this.gameObject);
    }

}
