using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flames : MonoBehaviour
{
    private Animator myAnim;
    private bool movingBack = false;
    private string state = "invisible";

    // Start is called before the first frame update
    void Start()
    {
        myAnim = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {

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
}
