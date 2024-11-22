using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimerController : MonoBehaviour
{
    public static TimerController instance;
    public float survivalTime = 1800f; //30 mins
    public float elapsedTime = 0f;
    public bool isFinished = false;
    public TextMeshProUGUI elapsedTimeText;
    

    private void Awake()
    {
        instance = this;
         
    }

    void Update()
    {
        if (!isFinished)
        {
            elapsedTime += Time.deltaTime;
            UpdateElapsedTimeText(elapsedTime);
        }

        if (elapsedTime >= survivalTime)
        {
            isFinished = true;
        }
    }

    void UpdateElapsedTimeText(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
    
        elapsedTimeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    
}
