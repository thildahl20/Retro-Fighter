using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerRun : MonoBehaviour {

    //Animator grenadeAnimator;
    public int boomerangDamage;
    public GameObject boomerangSprite;

    public Character target;
    //public float returnSpeed;
    public float returnForce;
    public float awayForce;

    Rigidbody2D boomerangRB2D;

    //[HideInInspector] public EnemyAgent enemyAgent;
    public void Start()
    {
        boomerangRB2D = GetComponent<Rigidbody2D>();
        boomerangRB2D.gravityScale = 0;
        boomerangRB2D.freezeRotation = true;

        GetComponent<Animator>().SetBool("Thrown", true);

        //enemyAgent = FindObjectOfType<EnemyAgent>();
    }

    public void Update()
    {
        boomerangRB2D.AddForce((target.transform.position - transform.position).normalized * returnForce);//add a force in the direction of the thrower
    }

    public void setTarget(Character newTarget)
    {
        target = newTarget;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject newHitObject = collision.gameObject;

        if (newHitObject == target.gameObject)
        {
            //boomerangSprite.SetActive(true);
            //enemyAgent.thrownWeapons.Remove(this.transform);
            //Destroy(this.gameObject);
            newHitObject.SendMessage("takeDamage", boomerangDamage);
            boomerangRB2D.AddForce((transform.position - target.transform.position).normalized * awayForce);
        }
        
    }
}
