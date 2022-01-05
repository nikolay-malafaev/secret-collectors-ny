using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;


public class GameManager : MonoBehaviour
{
    public bool pause;
    private int time;
    
    
    void Start()
    {

    }
    
    void FixedUpdate()
    {
       
    }

    public void Pause()
    {
        if (Time.timeScale == 0)
            Time.timeScale = 1;
        else Time.timeScale = 0;
    }



    /* public void Pause(int timeForPause)
     {
         //500 - for 10 second
         switch (timeForPause)
         {
             case 10:
                 time = 500;
                 break;
             case 5:
                 time = 250;
                 break;
             case 1:
                 time = 50;
                 break;

             default:
                 break;
         }
        pause = true;

         if (pause)
         {
             time--;
             if (time == 0)
                 pause = false;
         }
     }*/
}
