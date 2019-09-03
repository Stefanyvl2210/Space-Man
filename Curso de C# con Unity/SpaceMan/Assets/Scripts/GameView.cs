using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameView : MonoBehaviour {

    public Text coinsText, scoreText; 
    public Text maxScoreText;

    int coins = 0;
    float score = 0.0f;
    float maxScore = 0.0f;
   
    PlayerController controller;

    // Use this for initialization
    void Start () {
        controller = GameObject.Find("Player").GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
		if(GameManager.sharedInstance.currentGameState == GameState.inGame)
        {
            coins = GameManager.sharedInstance.collectedObject;
            score = controller.GetTravelDistance();
            maxScore = PlayerPrefs.GetFloat("maxscore",0);

            coinsText.text = coins.ToString();
            scoreText.text = "Score: " + score.ToString("f1"); //f1 indica que que es float con un decimal
            maxScoreText.text = "MaxScore: " + maxScore.ToString("f1");
        }
        if (GameManager.sharedInstance.currentGameState == GameState.gameOver)
        {
            
            coinsText.text = coins.ToString();
            scoreText.text = "Score: " + score.ToString("f1"); //f1 indica que que es float con un decimal
            maxScoreText.text = "MaxScore: " + maxScore.ToString("f1");
        }

    }

}
