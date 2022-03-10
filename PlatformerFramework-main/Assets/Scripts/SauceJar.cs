using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SauceJar : MonoBehaviour
{
    [SerializeField] private float stretch = 1f;
    [SerializeField] private float speed = 0.5f;
    [SerializeField] private string sauceColor = "red";

    void FixedUpdate()
    {
        float change = Mathf.Sin(Time.frameCount * speed) * stretch;
        transform.position = new Vector3(transform.position.x, transform.position.y + change, 0);
    }
}
