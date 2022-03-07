using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knives : MonoBehaviour
{
    private Rigidbody2D knife;
    private float downwardsSpeed = 10.0f;
    private float upwardsSpeed = 5.0f;
    private bool movingDown = true;
    public float timeOnGround = 0.5f;
    private bool onGround = false;
    private float currentTime = 0.0f;
    
    [SerializeField] String startDirection = "down";

    // Start is called before the first frame update
    void Start()
    {
        movingDown = startDirection == "down" ? true : false;
    }

    // Update is called once per frame
    void Update()
    {

        // move down
        if (transform.position.y > 2 && movingDown == false)
        {
            movingDown = true;
        }
        if (transform.position.y < -2.52 && movingDown)
        {
            // wait for 2 seconds
            onGround = true;

            movingDown = false;
        }

        if (onGround)
        {
            currentTime += Time.deltaTime;
            if (currentTime > timeOnGround)
            {
                onGround = false;
                currentTime = 0.0f;
            }
            return;
        }

        if (movingDown)
        {
            transform.position -= new Vector3(0, downwardsSpeed * Time.deltaTime);
        }

        // move back up, but quicker
        if (movingDown == false)
        {
            transform.position += new Vector3(0, upwardsSpeed * Time.deltaTime);
        }
    }
}
