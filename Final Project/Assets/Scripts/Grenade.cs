using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : Weapon
{
    Animator grenadeAnimator;
    public int grenadeDamage;

    List<GameObject> hitObject = new List<GameObject>();

    [HideInInspector] public EnemyAgent enemyAgent;

    public void Start()
    {
        grenadeAnimator = GetComponent<Animator>();
        enemyAgent = FindObjectOfType<EnemyAgent>();
    }

    public void Update()
    {
        if (grenadeAnimator.GetCurrentAnimatorStateInfo(0).IsName("Done") && tag == "ThrownWeapon")
        {
            foreach (GameObject aHitObject in hitObject)
            {
                aHitObject.SendMessage("takeDamage", grenadeDamage);
            }
            enemyAgent.thrownWeapons.Remove(this.transform);
            Destroy(this.gameObject);
        }
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject newHitObject = collision.gameObject;
        if (!hitObject.Contains(newHitObject) && (newHitObject.tag == "Player" || newHitObject.tag == "Enemy"))
            hitObject.Add(newHitObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        hitObject.Remove(collision.gameObject);
    }
}
