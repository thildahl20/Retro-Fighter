using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : Character
{
    public int activeWeapon;

    Animator playerAnimator;

    public GameManager theGameManager;

    FloatingJoystick theJoystick;

    bool grounded = false;

    public float jumpForce = 1500f;

    Rigidbody2D playerRB2D;

    bool doubleJump = false;

    public float fallMultiplier = 2.5f;
    public float riseMultiplier = .5f;

    void Start()
    {
        //anim = GetComponent<Animator>();
        playerRB2D = GetComponent<Rigidbody2D>();
        playerRB2D.velocity = new Vector2(0, 0);//not needed??
        theJoystick = FindObjectOfType<FloatingJoystick>();
        playerAnimator = GetComponent<Animator>();
        health = maxHealth;
    }

    void FixedUpdate()
    {
        if (grounded)
            doubleJump = false;

        int move = 0;
        if (theJoystick.Horizontal < 0)
            move = -1;
        else if (theJoystick.Horizontal > 0)
            move = 1;
            

        //anim.SetFloat("Speed", Mathf.Abs(move));

        playerRB2D.velocity = new Vector2(move * maxSpeed, playerRB2D.velocity.y);

        if (move > 0 && !facingRight)
            Flip();
        else if (move < 0 && facingRight)
            Flip();

        if (move != 0)
            playerAnimator.SetBool("Running", true);
        else
            playerAnimator.SetBool("Running", false);
    }

    void Update()
    {
        healthBar.fillAmount = (float)health / maxHealth;

        //jump
        if (playerRB2D.velocity.y < 0)
        {
            playerRB2D.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (playerRB2D.velocity.y > 0)// && !Input.GetButton("Jump Button"))
        {
            playerRB2D.velocity += Vector2.up * Physics2D.gravity.y * (riseMultiplier - 1) * Time.deltaTime;
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
            playerRB2D.AddForce(new Vector2(0, jumpForce));

            if (!doubleJump && !grounded)
                doubleJump = true;

            grounded = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")//reset the jump counts
        {
            grounded = true;
            doubleJump = false;
        }
        else if (collision.gameObject.tag == "Enemy") // the player can jump once off of the enemy
            doubleJump = false;
    }

    public void takeDamage(int dmgToTake)
    {
        if (dmgToTake >= health)
        {
            health = 0;
            healthText.text = "Player Health:\n" + health;
            theGameManager.GameOver(this.gameObject);
            //this.enabled = false;
        }
        else
        {
            health = health - dmgToTake;
            healthText.text = "Player Health:\n" + health;
        }
    }
}
