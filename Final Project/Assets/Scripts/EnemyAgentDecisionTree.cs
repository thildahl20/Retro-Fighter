using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//AI: Decision Tree 
public class EnemyAgentDecisionTree : EnemyAgent
{
    Vector3 lastPosition;   //Checks to see that the enemy is changing positions and not stuck

    public int framesPerDecision = 3;   //Lets a command last for multiple frames to make changes such as getting unstuck noticeable
    private int frames;

    //If the enemy gets stuck directly above or below something (player or platform) this tracks until it is free
    bool stuck = false;
    int freedomDirection;

    //In order to prevent the enemy spamming a weapon and wasting its uses, it can only fire after so many cycles
    //Enemy oddly avoided player when added
    //public int attackDelay = 5;
    //private int attackLag = 0;

    public int randomness;  //Allows the user to decide how often random acts occur

    private void Start()
    {
        moveDirection = 0;
        enemyRGB2D = GetComponent<Rigidbody2D>();
        frames = framesPerDecision;

        System.Random myRandom = new System.Random();
        int randDirection = myRandom.Next(0, 2);
        if (randDirection == 1)
            freedomDirection = 1;
        else
            freedomDirection = -1;
    }

    private void Update()   
    {
        System.Random myRandom = new System.Random();

        frames--;
        if (frames == 0) {
            frames = framesPerDecision;

                //Determine player's location relative to enemy
                bool levelToPlayer = false; //If the player is of the same verticality of the enemy
                bool closeToPlayer = false;  //If the player is within a melee range attack of the enemy
                bool properDirection = true;//If the enemy is facing the player so the attack will collide
                bool belowPlayer = false, rightOfPlayer = false, abovePlayer = false;

                //Determines if the player is of the same vertical level, above or below and left or right of the enemy
                if ((player.transform.position.y - enemy.transform.position.y < 2 && player.transform.position.y - enemy.transform.position.y >= 0) ||
                       (player.transform.position.y - enemy.transform.position.y > -2 && player.transform.position.y - enemy.transform.position.y <= 0))
                    levelToPlayer = true;

                if (player.transform.position.y > enemy.transform.position.y)
                {
                    belowPlayer = true;
                }
                if (player.transform.position.y < enemy.transform.position.y)
                {
                    abovePlayer = true;
                }
                //Attempted to make enemy go around player when stuck above, but also caused strange behaviour
                //if ((player.transform.position.y - enemy.transform.position.y > .5)
                //       || (player.transform.position.y - enemy.transform.position.y < .5))
                //{
                //     stuck = false;
                //     int randDirection = myRandom.Next(0, 2);
                //     if (randDirection == 1)
                //        freedomDirection = 1;
                //     else
                //        freedomDirection = -1;
                // }

                if (player.transform.position.x <= enemy.transform.position.x)
                {
                    rightOfPlayer = true;

                    if (enemy.facingRight)
                        properDirection = false;

                    if (enemy.transform.position.x - player.transform.position.x < 2)
                        closeToPlayer = true;
                }
                else if (!enemy.facingRight)
                    properDirection = false;

                bool clearShot = true;  //Determine if any platform or thrown weapon is in the way

                //Determine where platforms are located around the enemy
                //bool platformLeft = false, platformRight = false; //There is one between player and enemy
                bool platformUp = false; //Know if it is chasing 
                bool platformCloseLeft = false, platformCloseRight = false;           //Should be jumped: close to enemy

                //For each platform, determines if there is a clear shot at the enemy and whether or not there is one close enough to get in the way
                foreach (Transform transform in platforms)
                {
                    //If the platform is at a similar enough height to the enemy to be in the way
                    bool levelToEnemy = false;
                    if ((transform.position.y - enemy.transform.position.y < 2 && transform.position.y - enemy.transform.position.y >= 0) ||
                        (transform.position.y - enemy.transform.position.y > -2 && transform.position.y - enemy.transform.position.y <= 0))
                        levelToEnemy = true;

                    //Platform on left in the way
                    if (transform.position.x - enemy.transform.position.x < 0
                        && transform.position.x - player.transform.position.x > 0
                        && levelToEnemy && levelToPlayer)
                    {
                        //platformLeft = true;
                        clearShot = false;
                    }
                    //Platform on right in the way
                    if (transform.position.x - enemy.transform.position.x > 0
                        && transform.position.x - player.transform.position.x < 0
                        && levelToEnemy && levelToPlayer)
                    {
                        //platformRight = true;
                        clearShot = false;
                    }

                    //Platform on left close
                    if (transform.position.x - enemy.transform.position.x < 0 && transform.position.x - enemy.transform.position.x > -3 && levelToEnemy)
                        platformCloseLeft = true;
                    //Platform on right close
                    if (transform.position.x - enemy.transform.position.x > 0 && transform.position.x - enemy.transform.position.x < 3 && levelToEnemy)
                        platformCloseRight = true;

                    //Platform above (within range)
                    if (transform.position.y - enemy.transform.position.y > 0 && transform.position.y - enemy.transform.position.y < 5 && levelToEnemy)
                        platformUp = true;
                }

                //Store the nearest weapon
                Transform nearestWeapon = null;
                float closestWeaponDistance = 100;

                //Check to see where the nearest weapons are and if it should approach them
                foreach (Transform weapon in weapons)
                {
                    float distance = Vector3.Distance(weapon.position, enemy.transform.position);
                    if (nearestWeapon == null || distance < closestWeaponDistance)
                    {
                        closestWeaponDistance = distance;
                        nearestWeapon = weapon;
                    }
                }

                //Stores the nearest projectile and whether or not it is close (melee deflect range) or of the same height
                //Also, know if there is one above the enemy (not to jump)
                Transform nearestThrownWeapon = null;
                float closestThrownWeaponDistance = 100;

                bool closeThrownWeaponLeft = false;
                bool closeThrownWeaponRight = false;
                bool thrownWeaponAbove = false;
                //Check to see if there are any weapons approaching the player, and either block attack or dodge
                foreach (Transform thrownWeapon in thrownWeapons)
                {
                    bool levelToEnemy = false;
                    if ((thrownWeapon.position.y - enemy.transform.position.y < 2 && thrownWeapon.position.y - enemy.transform.position.y >= 0) ||
                        (thrownWeapon.position.y - enemy.transform.position.y > -2 && thrownWeapon.position.y - enemy.transform.position.y <= 0))
                        levelToEnemy = true;

                    float distance = Vector3.Distance(thrownWeapon.position, enemy.transform.position);
                    if (nearestThrownWeapon == null || distance < closestThrownWeaponDistance)
                    {
                        closestThrownWeaponDistance = distance;
                        nearestThrownWeapon = thrownWeapon;
                    }

                    //If a thrown weapon is directly between, horizontally, the enemy and player, not clear to shoot
                    if ((thrownWeapon.position.x < enemy.transform.position.x &&
                        player.transform.position.x < thrownWeapon.position.x) ||
                        (thrownWeapon.position.x > enemy.transform.position.x &&
                        player.transform.position.x > thrownWeapon.position.x)
                        && levelToEnemy)
                        clearShot = false;

                    //thrownWeapon on left close
                    if (thrownWeapon.position.x - enemy.transform.position.x < 0 && thrownWeapon.position.x - enemy.transform.position.x > -2)
                        closeThrownWeaponLeft = true;
                    //thrownWeapon on right close
                    if (transform.position.x - enemy.transform.position.x > 0 && transform.position.x - enemy.transform.position.x < 2)
                        closeThrownWeaponRight = true;
                    //thrownWeapon above the enemy
                    if ((closeThrownWeaponLeft || closeThrownWeaponRight) && thrownWeapon.position.y > enemy.transform.position.y)
                        thrownWeaponAbove = true;
                }

                //When it reaches the attack decision tree, should know from movement decision if it wants to attack and or jump
                bool shouldAttack = false;
                bool shouldJump = false;
                //if necessary to change movement direction after attack is called with current direction, use this
                int newMoveDirection = moveDirection;

                //Move Decision Tree
                //If it can attack the enemy, do so
                if (properDirection && levelToPlayer && clearShot)
                {
                    shouldAttack = true;
                }

                //First, if user is going to be hit by weapon, evade
                if (closeThrownWeaponLeft || closeThrownWeaponRight)
                {
                    //Decides which route is the safest based on the closest bad weapon
                    bool dodgeLeft = false; //As in dodge an object on the left: want to move right
                    bool dodgeRight = false;
                    if (nearestThrownWeapon.position.x - enemy.transform.position.x < 0 && closeThrownWeaponLeft == true)
                        dodgeLeft = true;
                    if (nearestThrownWeapon.position.x - enemy.transform.position.x > 0 && closeThrownWeaponRight == true)
                        dodgeRight = true;

                    //No way out, attack in direction of closest weapon
                    if (closeThrownWeaponLeft && closeThrownWeaponRight && thrownWeaponAbove)
                    {
                        if (dodgeLeft)  //Turn to face danger instantly
                        {
                            enemy.facingRight = true;
                        }
                        else if (dodgeRight)
                        {
                            enemy.facingRight = false;
                        }

                        shouldAttack = true;
                    }

                    //There is a way out, pick the route
                    if (closeThrownWeaponLeft && closeThrownWeaponRight)    ///Will pick
                    {
                        shouldJump = true;

                        //Still set movement in the direction away from closest weapon to help evade
                        if (dodgeLeft)
                            newMoveDirection = 1;
                        else if (dodgeRight)
                            newMoveDirection = -1;
                    }
                    else if (closeThrownWeaponLeft)
                    {
                        int mightJump = myRandom.Next(0, 2);
                        if (mightJump == 1)
                        {
                            shouldJump = true;
                            newMoveDirection = 0;
                        }
                        else
                            newMoveDirection = 1;
                    }
                    else if (closeThrownWeaponRight)
                    {
                        int mightJump = myRandom.Next(0, 2);
                        if (mightJump == 1)
                        {
                            shouldJump = true;
                            newMoveDirection = 0;
                        }
                        else
                            newMoveDirection = -1;
                    }
                }
                //Pick different target, either weapon or player
                //3rd, if no special weapon, try to grab one
                else
                {
                    if ((enemyWeaponPickup.activeSprite == noWeapon || enemyWeaponPickup.activeSprite.tag == "Mace")
                                && nearestWeapon != null)
                    {
                        if (nearestWeapon.position.x < enemy.transform.position.x)
                            newMoveDirection = -1;
                        else
                            newMoveDirection = 1;

                        // if (stuckByPlatform)
                        //     stuck = true;
                    }
                    //4th, line up verticality for open shot, or approach to attack
                    //Move towards player
                    else
                    {
                        if (rightOfPlayer)
                            newMoveDirection = -1;
                        else
                            newMoveDirection = 1;

                        //If the enemy is directly above or below the player, pick a random direction and move that way for a set amount of frames
       //                 if (belowPlayer || abovePlayer)
        //                {
         //                   int mightJump = myRandom.Next(0, 4);
          //                  if (mightJump == 1)
           //                     shouldJump = true;

                           // stuck = true;
            //            }
                    }

                    if ((newMoveDirection == -1 && platformCloseLeft) ||
                        (newMoveDirection == 1 && platformCloseRight))
                        shouldJump = true;

                }


                //Attack Decision Tree
                //(To a degree, Use which weapon is equipped to decide how to attack)
                if (shouldAttack)//&& attackLag == 0)
                {
                    //attackLag = attackDelay;
                    //Open horizontal ranged shot
                    if (enemyWeaponPickup.activeSprite != noWeapon && enemyWeaponPickup.activeSprite.tag != "Mace")

                        enemy.AttackTwo(enemy);
                    //Open horizontal close shot
                    else if (closeToPlayer || shouldAttack)
                    {
                        if (enemyWeaponPickup.activeSprite.tag == "Mace")
                            enemy.AttackTwo(enemy);
                        else
                            enemy.AttackOne();
                    }
                }
                //else
                //    attackLag--;

                if (shouldJump)
                    enemy.SendMessage("jump");

                //Little bit of randomness
                float jumpy = myRandom.Next(0, randomness);
                float dizzy = myRandom.Next(0, randomness);
                float punchy = myRandom.Next(0, randomness);

                if (jumpy == 1)
                    enemy.SendMessage("jump");
                if (dizzy == 1)
                    newMoveDirection = -newMoveDirection;
                if (punchy == 1)
                    enemy.AttackOne();


                //If enemy has not changed positions, most likely stuck on a wall, so try to get out
                if (enemy.transform.position == lastPosition)
                {
                    newMoveDirection = -newMoveDirection;
                    enemy.SendMessage("jump");
                }
                lastPosition = enemy.transform.position;

               if (!stuck)
                   moveDirection = newMoveDirection;
               else
                   moveDirection = freedomDirection;
        }
        else
            frames--;
    }
}
