using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sink : MonoBehaviour
{

    public AudioSource audioSource;
    public AudioClip waterSound;

    public GameObject Torti;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        audioSource.volume = 1 / (0.2f * Vector2.Distance(gameObject.transform.position, Torti.transform.position));
    }
}
