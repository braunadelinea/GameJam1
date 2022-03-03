using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnOffTrap : MonoBehaviour
{
    [SerializeField] private int swapInterval = 3; // Seconds
    private int lastUpdate; // Only update once per second

    void Update()
    {
        int currentTime = (int)Math.Floor(Time.fixedTime);
        
        if (currentTime % swapInterval == 0 && lastUpdate != currentTime)
        {
            lastUpdate = currentTime;
            
            if (gameObject.CompareTag("Enemy"))
            {
                gameObject.tag = "EnemyInactive";

                // TODO: Set sprite to oven closed
            }
            else
            {
                gameObject.tag = "Enemy";
                
                // TODO: Set sprite to oven open
            }
        }
    }
}
