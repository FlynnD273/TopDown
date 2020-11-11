/**************************
 * File: FlameController
 * Author: Flynn Duniho
 * Description: Set the size of the flame nd make it wobble
**************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameController : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer r;
    float i = 0;


    void Start()
    {
        r = GetComponent<SpriteRenderer>();
        rb = transform.parent.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        i++;
        //size is based on the user input
        float s = (float)(Input.GetAxis("Vertical") + Mathf.Sin(i / 5) / 10 - 0.2);
        if (s > 0)
        {
            //r.enabled = true;
            transform.localScale = new Vector3(s, s, 1);
            //Wobble
            transform.localRotation = Quaternion.Euler(0, 0, (float)(Mathf.Cos(i/30) * 5));
        }
        else
        {
            //Hide the flame
            //r.enabled = false;
        }
    }
}
