using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : Weapon
{
    public int spearDamage;

    [HideInInspector] public EnemyAgent enemyAgent;

    public void Start()
    {
        enemyAgent = FindObjectOfType<EnemyAgent>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject newHitObject = collision.gameObject;
        if (this.tag == "ThrownWeapon")
        {
            if (newHitObject.tag == "Player" || newHitObject.tag == "Enemy")
            {
                newHitObject.SendMessage("takeDamage", spearDamage);
            }
            enemyAgent.thrownWeapons.Remove(this.transform);
            Destroy(this.gameObject);
        }
    }

}
