/*****************************************
 * Edited by: Ryan Scheppler
 * Last Edited: 1/27/2021
 * Description: Add this to an object the player can collide with to go to a new level
 * *************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndDoor : MonoBehaviour
{
    public string LevelToLoad = "EndScene";
    public AudioSource audioSource;
    public AudioClip winSound;

    private bool gameWon = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool getGameWon()
    {
        return gameWon;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        gameWon = true;
        if(collision.gameObject.GetComponent<PlayerController>() != null)
        {
            audioSource.PlayOneShot(winSound);
            
            System.Threading.Thread.Sleep(6000);
            
            SceneManager.LoadScene(LevelToLoad);
        }
    }
}
