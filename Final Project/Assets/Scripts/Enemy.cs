using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : Character
{
    public GameManager theGameManager;

    [HideInInspector] public bool grounded, doubleJump;

    public Rigidbody2D enemyRB2D;

    public float jumpForce = 1500f;
    public float fallMultiplier = 2.5f;
    public float riseMultiplier = .5f;

    public int move;

    [HideInInspector] public Animator enemyAnimator;

    public EnemyAgent enemyAgent;

    public Punch punch;
    public SpearSprite spear;
    public Mace mace;
    public GrenadeSprite grenade;
    public BowSprite bow;
    public BoomerangSprite boomerang;
    public GunSprite gun;

    public void AttackOne()
    {
        punch.gameObject.SetActive(true);
    }

    public void AttackTwo(Character thrower)
    {
        if (mace.gameObject.activeInHierarchy)
        {
            mace.Swing();
        }
        else if (grenade.gameObject.activeInHierarchy)
        {
            grenade.Throw(thrower);
        }
        else if (bow.gameObject.activeInHierarchy)
        {
            bow.Shoot(thrower);
        }
        else if (boomerang.gameObject.activeInHierarchy)
        {
            boomerang.Throw(thrower);
        }
        else if (spear.gameObject.activeInHierarchy)
        {
            spear.Throw(thrower);
        }
        else if (gun.gameObject.activeInHierarchy)
        {
            gun.Shoot(thrower);
        }
    }
    private void Start()
    {
        health = maxHealth;
        enemyRB2D = GetComponent<Rigidbody2D>();
        facingRight = false;
        enemyAnimator = GetComponent<Animator>();
    }

    public void takeDamage(int dmgToTake)
    {
        if (dmgToTake >= health)
        {
            health = 0;
            healthText.text = "Enemy health:\n" + health;
            theGameManager.GameOver(this.gameObject);
        }
        else
        {
            health = health - dmgToTake;
            healthText.text = "Enemy health:\n" + health;
        }
    }

    void Update()
    {
        healthBar.fillAmount = (float)health / maxHealth;

        //jump
        if (enemyRB2D.velocity.y < 0)
        {
            enemyRB2D.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (enemyRB2D.velocity.y > 0)// && !Input.GetButton("Jump Button"))
        {
            enemyRB2D.velocity += Vector2.up * Physics2D.gravity.y * (riseMultiplier - 1) * Time.deltaTime;
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void jump()
    {
        if ((grounded || !doubleJump))
        {
            //anim.SetBool("Grounded", false);
            enemyRB2D.AddForce(new Vector2(0, jumpForce));

            if (!doubleJump && !grounded)
                doubleJump = true;

            grounded = false;
        }
    }

    void FixedUpdate()
    {
        if (grounded)
            doubleJump = false;


        //anim.SetFloat("Speed", Mathf.Abs(move));

        move = enemyAgent.moveDirection;

        enemyRB2D.velocity = new Vector2(move * maxSpeed, enemyRB2D.velocity.y);

        if (move > 0 && !facingRight)
            Flip();
        else if (move < 0 && facingRight)
            Flip();

        if (move != 0)
            enemyAnimator.SetBool("enemyRunning", true);
        else
            enemyAnimator.SetBool("enemyRunning", false);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")//reset the jump counts
        {
            grounded = true;
            doubleJump = false;
        }
        else if (collision.gameObject.tag == "Player")
            doubleJump = false;
    }
}
