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

    public AudioSource audioSource;
    public AudioClip fireClip;

    public GameObject Torti;

    // Start is called before the first frame update
    void Start()
    {
        myAnim = GetComponent<Animator>();
        audioSource.clip = fireClip;
        
        StartCoroutine(ChangeStates());
    }

    // Update is called once per frame
    void Update()
    {
        audioSource.volume = 1 / (0.2f * Vector2.Distance(gameObject.transform.position, Torti.transform.position));
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
            yield return new WaitForSeconds(0.25f);
            state = "low";
            movingBack = false;
            audioSource.Play();
        }
        else if (state == "low")
        {
            // low -> invisible
            if (movingBack)
            {
                yield return new WaitForSeconds(0.5f);
                state = "invisible";
                audioSource.Stop();
            }
            // low -> mid
            if (!movingBack)
            {
                yield return new WaitForSeconds(0.25f);
                state = "mid";
            }
        }
        else if (state == "mid")
        {
            // mid -> low
            if (movingBack)
            {
                yield return new WaitForSeconds(0.5f);
                state = "low";
            }
            // mid -> high
            if (!movingBack)
            {
                yield return new WaitForSeconds(0.25f);
                state = "high";
            }
        }
        // high -> mid
        else if (state == "high")
        {
            yield return new WaitForSeconds(0.5f);
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
