using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    
    public GameObject playerUICanvas;
    public GameObject pauseCanvas;
    public GameObject gameOverCanvas;
    public GameObject winScreenCanvas;

    
    public static UIController instance;

    void Awake()
    {
        
            instance = this;
         
    }

    void Start()
    {
        ShowMainUI();
    }
    
    public void ShowMainUI()
    {
        playerUICanvas.SetActive(true);
        pauseCanvas.SetActive(false);
        gameOverCanvas.SetActive(false);
        winScreenCanvas.SetActive(false);
    }

    public void ShowPauseMenu()
    {
        pauseCanvas.SetActive(true);
        playerUICanvas.SetActive(false);
    }

    public void HidePauseMenu()
    {
        pauseCanvas.SetActive(false);
        playerUICanvas.SetActive(true);
    }

    public void ShowGameOverScreen()
    {
        gameOverCanvas.SetActive(true);
        playerUICanvas.SetActive(false);
    }

    public void ShowWinScreen()
    {
        winScreenCanvas.SetActive(true);
        playerUICanvas.SetActive(false);
    }
    
    public void UnpauseGame()
    {
        GameManager.instance.Unpause();
    }
    
}
