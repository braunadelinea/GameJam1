using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fork : MonoBehaviour
{
    private float furthestX = -10000;
    private double lastIncreaseTime = 0;

    [SerializeField] private int forkChopTime = 3;
    [SerializeField] private float stabSpeed = 0.1f;
    private bool stabbed = false;
    
    [SerializeField] private Transform torti;
    private bool stabbedTorti = false;
    
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        // This sets up a timer to stab the player after they do not progress in the game for too long (forkChopTime)
        double currentTime = Math.Floor(Time.fixedTime);

        if (!stabbedTorti)
        {
            if (torti.position.x > furthestX + 0.25 && !stabbed)
            {
                furthestX = torti.position.x;

                lastIncreaseTime = currentTime;
                stabbed = false;
            }

            if (currentTime - lastIncreaseTime > forkChopTime && !stabbed)
            {
                stabbed = true;
            
                // TODO: Add stabbing here
                transform.position = new Vector3(torti.position.x + 5, 10, 0);
            }

            if (!stabbed)
            {
                transform.position = new Vector3(0, -50, 0);
            }
            else
            {
                transform.position = new Vector3(transform.position.x, transform.position.y - stabSpeed, 0);

                if (transform.position.y <= 0.6)
                {
                    stabbed = false;
                    // hits table
                }
            }
        }
    }

    public void setTortiStabbed()
    {
        stabbedTorti = true;
    }
}
