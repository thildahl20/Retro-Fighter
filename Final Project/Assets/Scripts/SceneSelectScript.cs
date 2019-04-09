using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSelectScript : MonoBehaviour {

public void selectScene()
    {
        //This should be placed on all buttons in the main menu 
        switch (this.gameObject.name)
        {
            //To play main game
            case "Menu":
                SceneManager.LoadScene("Menu");
                break;
            case "Main":
                SceneManager.LoadScene("Main");
                break;
            case "Boomer-Run!":
                SceneManager.LoadScene("Boomer-Run!");
                break;
            case "MainMedium":
                SceneManager.LoadScene("MainMedium");
                break;
            case "Exit":
                Application.Quit();
                break;
            case "AdaptiveBattle":
                SceneManager.LoadScene("AdaptiveBattle");
                break;
            case "Menu2":
                SceneManager.LoadScene("Menu2");
                break;
            case "Battle1":
                SceneManager.LoadScene("Battle1");
                break;
            case "Battle2":
                SceneManager.LoadScene("Battle2");
                break;
            case "Battle3":
                SceneManager.LoadScene("Battle3");
                break;
            case "Battle4":
                SceneManager.LoadScene("Battle4");
                break;
            case "Battle5":
                SceneManager.LoadScene("Battle5");
                break;
        }
    }
}
