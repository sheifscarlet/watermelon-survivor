using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Kill Counter")] 
    [SerializeField] private int killCount;

    [Header("Pause Settings")] 
    [SerializeField] private bool isPaused;
    private bool isGameOver; 
    
    [Header("PickUp Spawn Settings")]
    [SerializeField] float nextSpawnTime = 300f;

    // Components
    [SerializeField] private PlayerHealthController playerHealthController;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        isGameOver = false; 
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();

        if (!isGameOver) 
        {
            if (TimerController.instance.isFinished)
            {
                Win();
            }

            if (playerHealthController.IsDead)
            {
                GameOver();
            }
        }

        if (!isGameOver) 
        {
            if (isPaused)
            {
                Cursor.lockState = CursorLockMode.Confined; 
                Time.timeScale = 0f;
                UIController.instance.ShowPauseMenu();
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked; 
                Time.timeScale = 1f;
                UIController.instance.ShowMainUI();
            }
        }

        HandleSpawning();
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isGameOver)
        {
            isPaused = !isPaused;
        }
    }

    void HandleSpawning()
    {
       
        if (!TimerController.instance.isFinished && !playerHealthController.IsDead)
        {
            
            if (TimerController.instance.elapsedTime >= nextSpawnTime)
            {
                PickUpsSpawner.instance.SpawnPickUps();
                FoodSpawner.instance.SpawnFood();

               
                nextSpawnTime += 300f; // Increment by 5 minutes
            }
        }
    }

    void Win()
    {
        isGameOver = true; 
        isPaused = false; 
        Cursor.lockState = CursorLockMode.Confined; 
        Time.timeScale = 0f;
        UIController.instance.ShowWinScreen();
    }

    void GameOver()
    {
        isGameOver = true; 
        isPaused = false; 
        Cursor.lockState = CursorLockMode.Confined; 
        Time.timeScale = 0f;
        UIController.instance.ShowGameOverScreen();
    }

    public bool Unpause()
    {
        isPaused = false;
        return isPaused;
    }

    public void AddKillCount()
    {
        killCount++;
        AudioController.instance.PlaySound("ZombieDeath");
    }
}
