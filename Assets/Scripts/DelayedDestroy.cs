/**************************
 * File: DelayedDestroy
 * Author: Flynn Duniho
 * Description: Destroy GameObject after a time period
**************************/
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class DelayedDestroy : MonoBehaviour
{
    [Tooltip("The amount to wait before Destroying")]
    public float Delay;

    private Stopwatch s;


    void Start()
    {
        s = new Stopwatch();
        s.Start();
    }

    void Update()
    {
        if (s.ElapsedMilliseconds > Delay * 1000)
        {
            Destroy(gameObject);
        }
    }
}
