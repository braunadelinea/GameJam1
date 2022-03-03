/*****************************************
 * Edited by: Ryan Scheppler
 * Last Edited: 1/27/2021
 * Description: Addd to the main camera and the target is what it will try to follow, includes screen shake to be called as needed.
 * *************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FollowingCamera : MonoBehaviour
{
    public GameObject target;

    public float snapSpeed = 0.5f;


    public float shakeTime = 0;
    public float shakeMagnitude = 0;

    private float furthestX = 0;
    private double lastIncreaseTime = 0;

    [SerializeField] private int forkChopTime = 4;
    private bool stabbed = false;
    
    // Start is called before the first frame update
    void Start()
    {
        furthestX = target.transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        //testing
       /* if(Input.GetKeyDown(KeyCode.G))
        {
            TriggerShake(1, 1);
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            TriggerShake(1, 3);
        }*/
    }

    private void FixedUpdate()
    {
        Vector3 newPos = target.transform.position;
        newPos.z = transform.position.z;
        
        // Stabby Fork Start //
        
        // This sets up a timer to stab the player after they do not progress in the game for too long (forkChopTime)
        double currentTime = Math.Floor(Time.fixedTime);
        
        if (target.transform.position.x > furthestX)
        {
            furthestX = target.transform.position.x;

            lastIncreaseTime = currentTime;
            stabbed = false;
        }

        if (currentTime - lastIncreaseTime > forkChopTime && !stabbed)
        {
            stabbed = true;
            
            // TODO: Add stabbing here
        }
        
        // Stabby Fork End //

        if(shakeTime > 0)
        {
            newPos += Random.insideUnitSphere.normalized * shakeMagnitude;
            shakeTime -= Time.fixedDeltaTime;
        }
        else
        {
            shakeTime = 0;
            shakeMagnitude = 0;
        }



        // transform.position = Vector3.Lerp(transform.position, newPos, snapSpeed); // Follow target x and y
        transform.position = Vector3.Lerp(transform.position, new Vector3(newPos.x, transform.position.y, transform.position.z), snapSpeed); // follow target x
    }

    public void TriggerShake(float time, float magnitude)
    {
        if(shakeTime < time)
        {
            shakeTime = time;
        }
        if(shakeMagnitude < magnitude)
        {
            shakeMagnitude = magnitude;
        }
    }
}
