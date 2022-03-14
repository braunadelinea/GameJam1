using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fork : MonoBehaviour
{
    private float furthestX = -10000;
    private double lastIncreaseTime = 0;

    [SerializeField] private int forkChopTime = 3;
    [SerializeField] private float stabSpeed = 0.25f;
    [SerializeField] private float upSpeed = 0.1f;
    private bool stabbed = false;
    private bool movingUp = false;
    
    [SerializeField] private Transform torti;
    private bool stabbedTorti = false;
    
    // Shake
    [SerializeField] private FollowingCamera camera;
    
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        // This sets up a timer to stab the player after they do not progress in the game for too long (forkChopTime)
        double currentTime = Math.Floor(Time.fixedTime);

        if (!stabbedTorti || !movingUp)
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
                
                transform.position = new Vector3(torti.position.x - 3.6f, 15, 0);
            }

            if (!stabbed)
            {
                transform.position = new Vector3(0, -50, 0);
            }
            else
            {
                if (movingUp)
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y + upSpeed, 0);
                    if (transform.position.y > 15)
                    {
                        movingUp = false;
                        stabbed = false;
                    }
                }
                else
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y - stabSpeed, 0);
                }
                

                if (transform.position.y <= -1.1 && !movingUp)
                {
                    camera.TriggerShake(0.25f, 0.5f);
                    movingUp = true;
                }
            }
        }
    }

    public void setTortiStabbed()
    {
        stabbedTorti = true;
    }
}
