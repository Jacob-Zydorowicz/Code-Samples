/*
 * Jacob Zydorowicz
 * Project 2 Mole Mania
 * Timer for game in seconds
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    //Timer based on article : https://gamedevbeginner.com/how-to-make-countdown-timer-in-unity-minutes-seconds/
    
    //max and remaining time entered in seconds
    public int  maxTime;
    public float timeRemaining;

    private float mins;
    private float secs;
    public bool timerIsRunning;
    public bool gameOver;


    public Text timeText;
    void Start()
    {
        //maxTime = Mathf.FloorToInt(timeRemaining);
        display();
        timerIsRunning = false;
        gameOver = false;
        
    }

   
    //creates a minutes and seconds timer for game
    void FixedUpdate()
    {
        if(timerIsRunning && !gameOver)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
              
            }
            else
            {
                mins = 0;
                secs = 0;
                timeRemaining = 0;
                timerIsRunning = false;
                gameOver = true;
            }
            display();
            
        }
    }

    //displays time in minutes and seconds
    public void display()
    {
        mins = Mathf.FloorToInt(timeRemaining / 60);
        secs = Mathf.FloorToInt(timeRemaining % 60);

        timeText.text = string.Format("{0:00}:{1:00}", mins, secs);
    }
}
