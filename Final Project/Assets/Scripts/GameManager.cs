using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text gameOverText;
    public Button retry;
    public Button menu;

    public float timeRemainingUntilWeaponSpawn = 0;
    System.Random myRandom = new System.Random();
    public GameObject[] Weapons = new GameObject[5];

    public int minSpawnTime, maxSpawnTime;
    public EnemyAgent enemyAgent;

    public void GameOver(GameObject loser)
    {
        if (loser.tag == "Player")
            gameOverText.text = "Game Over! \nEnemy Wins!";
        else if (loser.tag == "Enemy")
            gameOverText.text = "Game Over! \nPlayer Wins!";
        gameOverText.gameObject.SetActive(true);
        retry.gameObject.SetActive(true);
        menu.gameObject.SetActive(true);
        loser.SetActive(false);
    }

    public void GameOverRun(float timeRemaining)
    {
        if (timeRemaining <= 0)
            gameOverText.text = "Game Over! \nYou Win!";
        else
            gameOverText.text = "Game Over! \nYou Lose!";

        gameOverText.gameObject.SetActive(true);
        retry.gameObject.SetActive(true);
        menu.gameObject.SetActive(true);
    }

    public void Update()
    {
        timeRemainingUntilWeaponSpawn = timeRemainingUntilWeaponSpawn - Time.deltaTime;

        if(timeRemainingUntilWeaponSpawn <= 0)
        {
            float coordinateToSpawn = myRandom.Next(-12, 12);
            if (coordinateToSpawn < 0)
                coordinateToSpawn = coordinateToSpawn + (float)0.5;
            else if (coordinateToSpawn > 0)
                coordinateToSpawn = coordinateToSpawn - (float)0.5;
            else
                coordinateToSpawn = myRandom.Next(-12, 12);

            int weaponToSpawn = myRandom.Next(Weapons.Length);

            GameObject spawnWeapon = Weapons[weaponToSpawn];
            GameObject newWeapon = Instantiate(spawnWeapon, new Vector3(coordinateToSpawn, (float)4.5, 0), Quaternion.identity);
            enemyAgent.weapons.Add(newWeapon.transform);

            timeRemainingUntilWeaponSpawn = myRandom.Next(minSpawnTime, maxSpawnTime + 1);
        }
    }

}
