using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flames : MonoBehaviour
{
    private Animator myAnim;
    private float timeInvicible = 2.0f;
    private int state = 0;

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
        // invicible -> low
        if (state == 0)
        {
            yield return new WaitForSeconds(1);
            state = 1;
            movingBack = false;
        }
        else if (state == 1)
        {
            // low -> invicible
            if (movingBack)
            {
                yield return new WaitForSeconds(1);
                state = 0;
            }
            // low -> mid
            if (!movingBack)
            {
                yield return new WaitForSeconds(1);
                state = 2;
            }
        }
        else if (state == 2)
        {
            // mid -> low
            if (movingBack)
            {
                yield return new WaitForSeconds(1);
                state = 1;
            }
            // mid -> high
            if (!movingBack)
            {
                yield return new WaitForSeconds(1);
                state = 3;
            }
        }
        // high -> mid
        else if (state == 3)
        {
            yield return new WaitForSeconds(1);
            state = 2;
            movingBack = true;
        }
        StartCoroutine(ChangeStates());
    }
}
