using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//se puede usar en cualquier script porque es publico y fuera de la clase
public enum GameState
{
    menu,
    inGame,
    gameOver
}

public class GameManager : MonoBehaviour {

    //identifica el estado del juego
    public GameState currentGameState = GameState.menu;

    public static GameManager sharedInstance; //para que éste controlador predomine si se crea otro

    PlayerController controller;

    //contador de los objetos
    public int collectedObject = 0;

    void Awake()
    {
        if(sharedInstance == null)
        {
            sharedInstance = this;
        }
            
    }

    // Use this for initialization
    void Start () {
        controller = GameObject.Find("Player").GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Submit") && currentGameState != GameState.inGame)
        {
            StartGame();
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            BackToMenu();
        }
        
	}

    public void StartGame()
    {
        SetGameState(GameState.inGame);
    }
    public void GameOver()
    {
        SetGameState(GameState.gameOver);
    }
    public void BackToMenu()
    {
        SetGameState(GameState.menu);
    }

    //maneja el estado del juego
    void SetGameState(GameState newGameState)
    {
        if(newGameState == GameState.menu)
        {
            MenuManager.sharedInstance.ShowMainMenu();
            MenuManager.sharedInstance.HideCanvasGameOver();
        }else if(newGameState == GameState.inGame)
        {
            //aqui poner el audio cuando inicia el juego
            LevelManager.sharedInstance.RemoveAllLevelBlocks();
            LevelManager.sharedInstance.GenerateInitialBlocks();
            controller.StartGame();

            MenuManager.sharedInstance.HideMainMenu();
            MenuManager.sharedInstance.ShowCanvasInGame();
            MenuManager.sharedInstance.HideCanvasGameOver();

        }else if(newGameState == GameState.gameOver)
        {
            GetComponent<AudioSource>().Play();
            MenuManager.sharedInstance.HideCanvasInGame();
            MenuManager.sharedInstance.ShowCanvasGameOver();
        }

        this.currentGameState = newGameState;
    }

    //de tipo de la clase Collectable
    public void CollectObject(Collectable collectable)
    {
        collectedObject += collectable.value;
    }

}
