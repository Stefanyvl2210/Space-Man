using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour {


    public static MenuManager sharedInstance;//Para que éste sea el unico controlador de la clase
    public Canvas menuCanvas, gameCanvas, gameOverCanvas;

    private void Awake()
    {
        if(sharedInstance == null)
        {
            sharedInstance = this;
        }
    }

    public void ShowMainMenu()
    {
        menuCanvas.enabled = true;
    }
    
    public void ExitGame()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit(); 
    #endif
    }

    public void HideMainMenu()
    {
        menuCanvas.enabled = false;
    }

    public void HideCanvasInGame()
    {
        gameCanvas.enabled = false;
    }

    public void ShowCanvasInGame()
    {
        gameCanvas.enabled = true;
    }

    public void HideCanvasGameOver()
    {
        gameOverCanvas.enabled = false;
    }

    public void ShowCanvasGameOver()
    {
        gameOverCanvas.enabled = true;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
