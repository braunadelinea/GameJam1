using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flames : MonoBehaviour
{
    private Animator myAnim;
    private bool movingBack = false;
    private string state = "invisible";

    [SerializeField] private GameObject lowFlame;
    [SerializeField] private GameObject mediumFlame;
    [SerializeField] private GameObject highFlame;

    // Start is called before the first frame update
    void Start()
    {
        myAnim = GetComponent<Animator>();
        
        StartCoroutine(ChangeStates());
    }

    // Update is called once per frame
    void Update()
    {
        // Do stuff
        if (state == "invisible")
        {
            disableFlame(lowFlame);
            disableFlame(mediumFlame);
            disableFlame(highFlame);
        }
        else if (state == "low")
        {
            enableFlame(lowFlame);
            disableFlame(mediumFlame);
            disableFlame(highFlame);
        }
        else if (state == "mid")
        {
            disableFlame(lowFlame);
            enableFlame(mediumFlame);
            disableFlame(highFlame);
        }
        else if (state == "high")
        {
            disableFlame(lowFlame);
            disableFlame(mediumFlame);
            enableFlame(highFlame);
        }
    }

    IEnumerator ChangeStates()
    {
        // invisible -> low
        if (state == "invisible")
        {
            yield return new WaitForSeconds(1);
            state = "low";
            movingBack = false;
        }
        else if (state == "low")
        {
            // low -> invisible
            if (movingBack)
            {
                yield return new WaitForSeconds(1);
                state = "invisible";
            }
            // low -> mid
            if (!movingBack)
            {
                yield return new WaitForSeconds(1);
                state = "mid";
            }
        }
        else if (state == "mid")
        {
            // mid -> low
            if (movingBack)
            {
                yield return new WaitForSeconds(1);
                state = "low";
            }
            // mid -> high
            if (!movingBack)
            {
                yield return new WaitForSeconds(1);
                state = "high";
            }
        }
        // high -> mid
        else if (state == "high")
        {
            yield return new WaitForSeconds(1);
            state = "mid";
            movingBack = true;
        }

        StartCoroutine(ChangeStates());
    }

    private void enableFlame(GameObject flame)
    {
        flame.GetComponent<BoxCollider2D>().enabled = true;
        flame.GetComponent<SpriteRenderer>().enabled = true;
    }

    private void disableFlame(GameObject flame)
    {
        flame.GetComponent<BoxCollider2D>().enabled = false;
        flame.GetComponent<SpriteRenderer>().enabled = false;
    }
}
