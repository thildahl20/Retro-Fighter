using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

    public float maxTime;
    //float previousTime;
    public Text text;

    //[HideInInspector] public float timeRemaining;

    public GameManager gameManager;
	
	void Update() {
        maxTime = maxTime - Time.deltaTime;
        text.text = "Time: " + (int)maxTime;

        //previousTime = timeRemaining;

        if (maxTime <= 0)
            gameManager.SendMessage("GameOverRun", maxTime);
	}

}
