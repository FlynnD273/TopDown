/**************************
 * File: BulletController
 * Author: Flynn Duniho
 * Description: Determines when to Destroy the bullet, after a time, or when it slows down
**************************/
using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Timers;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [Tooltip("How long the bullet lasts, in seconds")]
    public float Life = 8;

    [Tooltip("Bullet dies on contact with these Tags")]
    public string[] Tags;

    [Tooltip("The Bounds of the field")]
    public BoxCollider2D Bounds;

    [Tooltip("Audio clip to play when bullet bounces")]
    public AudioClip BounceClip;



    //Keeps track of how long the bullets been in existence
    private Stopwatch time;

    //So we can destroy the bullet's tiles
    private Tile tile;
    private Rigidbody2D rb;

    //This is so we can Destroy when the bullet slows down
    private float startSpeed;


    void Start()
    {
        time = new Stopwatch();
        time.Start();

        tile = GetComponent<Tile>();
        rb = GetComponent<Rigidbody2D>();
        startSpeed = rb.velocity.magnitude;
    }


    void Update()
    {
        //Bullet's lasted for over the time limit, or the speed is too slow
        if (time.ElapsedMilliseconds > Life * 1000 || rb.velocity.magnitude < startSpeed / 2)
        {
            tile.Remove();
        }

        //The Bullet always faces the way it's moving
        rb.rotation = 90 - Mathf.Rad2Deg * Mathf.Atan2(rb.velocity.x, rb.velocity.y);
    }

    //It hit something, or something hit it
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //If collides with the right Tag, Destroy
        if (Tags.Contains(collision.gameObject.tag))
        {
            if (tile != null)
            {
                tile.Remove();
            }
        }
        else
        {
            Tools.PlayClip(BounceClip, 0.3f);
        }
    }
}
