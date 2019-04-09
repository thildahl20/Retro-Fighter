using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAgent : MonoBehaviour
{

    Vector3 startPos, playerStartPosition;

    public Transform[] platforms = new Transform[6];
    [HideInInspector] public List<Transform> weapons = new List<Transform>();

    const int halfWorldSpaceSize = 12; //Should not be half??
    const int halfWorldSpaceSizeVertical = 4;//^^

    public GameManager gameManager;

    protected Rigidbody2D enemyRGB2D;

    public int activeWeapon;

    public Enemy enemy;
    public Player player;

    public EnemyWeaponPickup enemyWeaponPickup;
    public WeaponPickup playerWeaponPickup;

    public NoWeapon noWeapon;

    public List<Transform> thrownWeapons = new List<Transform>();

    public int moveDirection;

    public float damageRewardMultiplier = .01f;
    public float timeReward = -.005f;

    int previousPlayerHealth, previousEnemyHealth; // to track health loss over time

    private void Start()
    {
        moveDirection = 0;
        startPos = transform.position;
        playerStartPosition = player.transform.position;
        enemyRGB2D = GetComponent<Rigidbody2D>();
        previousPlayerHealth = player.maxHealth;
        previousEnemyHealth = enemy.maxHealth;
    }

    List<float> fillList(List<float> originalList)//to normalize the size of lists
    {
        List<float> newList = originalList;
        while(newList.Count > 100)
        {
            newList.RemoveAt(newList.Count - 1);
        }
        while(newList.Count < 100)
        {
            newList.Add(float.MinValue);
        }
        return newList;
    }

    private void Update()
    {
        System.Random myRandom = new System.Random();

        //This AI method uses randomization to make the enemy behave sporadically, randomly approaching the player, jumping and attacking
        int moveEnemy = myRandom.Next(0, 10);
        int valid = myRandom.Next(0, 10);
        //Randomize movement with a small chance of changing direction to move uniformly  
        //and a small chance of not actually moving toward the player
        if ((player.transform.position.x - enemy.transform.position.x < -1) && moveEnemy == 1)  //Player on enemy left
        {
            if (valid == 1)
                moveDirection = 1;
            else
                moveDirection = -1;
        }
        else if ((player.transform.position.x - enemy.transform.position.x > 1) && moveEnemy == 1)  //Player on enemy right
        {
            if (valid == 1)
                moveDirection = -1;
            else
                moveDirection = 1;
        }
        else if (moveEnemy == 1)
            moveDirection = 0;

        //Jump
        int jumping = myRandom.Next(0, 10);
        if (jumping == 1)
            enemy.SendMessage("jump");

        //Attack
        int attack = myRandom.Next(0, 50);
        if (attack == 1)
        {
            if (enemyWeaponPickup.activeSprite != noWeapon)
                enemy.AttackTwo(enemy);
            else
                enemy.AttackOne();
        }

    }
    /*
     * This is our work for neural networks
     * Includes reset, collect observations and reward methods
    public override void AgentReset()// called when the player or enemy wins the round
    {
        //reset health
        player.health = player.maxHealth;
        enemy.health = enemy.maxHealth;

        // reset positions
        this.transform.position = startPos;
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;

        player.transform.position = playerStartPosition;
        player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;

        //reset weapon sprites
        playerWeaponPickup.activeSprite.gameObject.SetActive(false);
        enemyWeaponPickup.activeSprite.gameObject.SetActive(false);

        //destroy all weapons on the map
        foreach(Transform weapon in weapons)//could be done in academyReset instead
        {
            Destroy(weapon.gameObject);
        }

        gameManager.timeRemainingUntilWeaponSpawn = 0;
    }

    public override void CollectObservations() //feeds the neural network
    {
        List<float> platformsX = new List<float>();
        List<float> platformsY = new List<float>();

        List<float> weaponsX = new List<float>();
        List<float> weaponsY = new List<float>();

        List<float> thrownWeaponsX = new List<float>();
        List<float> thrownWeaponsY = new List<float>();

        List<float> weaponsVelocityX = new List<float>();
        List<float> weaponsVelocityY = new List<float>();

        List<float> thrownWeaponsVelocityX = new List<float>();
        List<float> thrownWeaponsVelocityY = new List<float>();

        List<float> weaponTypes = new List<float>();
        List<float> thrownWeaponTypes = new List<float>();

        //relative values used to give normalized inputs from -1 to 1 to the neural network
        foreach (Transform platform in platforms)
        {
            platformsX.Add((transform.position.x - platform.transform.position.x) / halfWorldSpaceSize);//position relative to all platforms present
            platformsY.Add((transform.position.y - platform.transform.position.y) / halfWorldSpaceSizeVertical);
        }
        platformsX = fillList(platformsX);
        platformsY = fillList(platformsY);
        AddVectorObs(platformsX);
        AddVectorObs(platformsY);

        int weaponType;
        foreach (Transform weapon in weapons)
        {
            weaponsX.Add((transform.position.x - weapon.position.x) / halfWorldSpaceSize);//position relative to all spawned weapons
            weaponsY.Add((transform.position.y - weapon.position.y) / halfWorldSpaceSizeVertical);

            weaponsVelocityX.Add(weapon.GetComponent<Rigidbody2D>().velocity.x / halfWorldSpaceSize);
            weaponsVelocityY.Add(weapon.GetComponent<Rigidbody2D>().velocity.y / halfWorldSpaceSizeVertical);

            if (weapon.tag == "mace")
                weaponType = 0;
            else if (weapon.tag == "grenade")
                weaponType = 1;
            else if (weapon.tag == "boomerang")
                weaponType = 2;
            else if (weapon.tag == "bow")
                weaponType = 3;
            else if (weapon.tag == "spear")
                weaponType = 4;
            else//gun
                weaponType = 5;
            weaponTypes.Add(weaponType);
        }

        weaponsX = fillList(weaponsX);
        weaponsY = fillList(weaponsY);
        AddVectorObs(weaponsX);
        AddVectorObs(weaponsY);

        AddVectorObs((transform.position.x - player.transform.position.x) / halfWorldSpaceSize);//position relative to player
        AddVectorObs((transform.position.y - player.transform.position.y) / halfWorldSpaceSizeVertical);

        AddVectorObs((transform.position.x + halfWorldSpaceSize) / halfWorldSpaceSize);//position relative to walls
        AddVectorObs((transform.position.x - halfWorldSpaceSize) / halfWorldSpaceSize);
        AddVectorObs((transform.position.y + halfWorldSpaceSizeVertical) / halfWorldSpaceSizeVertical);
        AddVectorObs((transform.position.y - halfWorldSpaceSizeVertical) / halfWorldSpaceSizeVertical);

        AddVectorObs(enemyRGB2D.velocity.x / halfWorldSpaceSize);//relative velocity
        AddVectorObs(enemyRGB2D.velocity.y / halfWorldSpaceSizeVertical);
        AddVectorObs(player.gameObject.GetComponent<Rigidbody2D>().velocity.x / halfWorldSpaceSize);
        AddVectorObs(player.gameObject.GetComponent<Rigidbody2D>().velocity.y / halfWorldSpaceSizeVertical);

        AddVectorObs(activeWeapon);//current weapons
        AddVectorObs(player.activeWeapon);

        AddVectorObs(player.health);//health standings
        AddVectorObs(enemy.health);

        AddVectorObs(enemyWeaponPickup.activeSprite.uses);//ammo (-1 if N/A)
        AddVectorObs(playerWeaponPickup.activeSprite.uses);

        foreach(Transform thrownWeapon in thrownWeapons)//weapons to avoid
        {
            thrownWeaponsX.Add((transform.position.x - thrownWeapon.transform.position.x) / halfWorldSpaceSize);
            thrownWeaponsY.Add((transform.position.y - thrownWeapon.transform.position.y) / halfWorldSpaceSizeVertical);

            thrownWeaponsVelocityX.Add(thrownWeapon.GetComponent<Rigidbody2D>().velocity.x / halfWorldSpaceSize);
            thrownWeaponsVelocityY.Add(thrownWeapon.GetComponent<Rigidbody2D>().velocity.y / halfWorldSpaceSizeVertical);

            if (thrownWeapon.tag == "mace")
                weaponType = 0;
            else if (thrownWeapon.tag == "grenade")
                weaponType = 1;
            else if (thrownWeapon.tag == "boomerang")
                weaponType = 2;
            else if (thrownWeapon.tag == "bow")
                weaponType = 3;
            else if (thrownWeapon.tag == "spear")
                weaponType = 4;
            else//gun
                weaponType = 5;
            thrownWeaponTypes.Add(weaponType);
        }

        thrownWeaponsX = fillList(thrownWeaponsX);
        thrownWeaponsY = fillList(thrownWeaponsY);
        AddVectorObs(thrownWeaponsX);
        AddVectorObs(thrownWeaponsY);

        weaponsVelocityX = fillList(weaponsVelocityX);
        weaponsVelocityY = fillList(weaponsVelocityY);
        AddVectorObs(weaponsVelocityX);
        AddVectorObs(weaponsVelocityY);

        thrownWeaponsVelocityX = fillList(thrownWeaponsVelocityX);
        thrownWeaponsVelocityY = fillList(thrownWeaponsVelocityY);
        AddVectorObs(thrownWeaponsVelocityX);
        AddVectorObs(thrownWeaponsVelocityY);

        weaponTypes = fillList(weaponTypes);
        thrownWeaponTypes = fillList(thrownWeaponTypes);
        AddVectorObs(weaponTypes);
        AddVectorObs(thrownWeaponTypes);

        int jumps;
        if (enemy.grounded)
            jumps = 0;
        else if (!enemy.doubleJump)
            jumps = 1;
        else
            jumps = 2;

        AddVectorObs(jumps);
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        //rewards
        AddReward((previousPlayerHealth - player.health) * damageRewardMultiplier);
        AddReward((enemy.health - previousEnemyHealth) * damageRewardMultiplier);

        previousPlayerHealth = player.health;
        previousEnemyHealth = enemy.health;

        AddReward(timeReward);


        if(player.health <= 0 || enemy.health <= 0)//game over
        {
            Done();
            if(player.health <= 0)
                AddReward(1f);
            if (enemy.health <= 0)//they may both die at the same time
                AddReward(-1f);
        }

        
        //actions
        moveDirection = (int)vectorAction[0];
        if (vectorAction[1] == 1)
            enemy.jump();
        if (vectorAction[2] == 1)
            enemy.AttackOne();
        if (vectorAction[3] == 1)
            enemy.AttackTwo(enemy);
    } */
}
