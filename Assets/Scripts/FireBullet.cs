/**************************
 * File: FireBullet
 * Author: Flynn Duniho
 * Description: Fire a bullet prefab, apply kickback. 
 * Is meant to be general, maybe enemeies will shoot in the future
**************************/
using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class FireBullet : MonoBehaviour
{
    [Tooltip("Bullet prefab that gets spawned in")]
    public GameObject Bullet;

    [Tooltip("The speed of the bullet")]
    public float BulletSpeed;

    [Tooltip("How long the bullet lasts, in seconds")]
    public float BulletLife = 10;

    [Tooltip("The amount of kickback applied when shooting a bullet")]
    public float Kickback = 2f;

    [Tooltip("Minimum time between shots")]
    public float fireInterval = 0.2f;

    [Tooltip("The bounds of the field")]
    public BoxCollider2D Bounds;

    [Tooltip("Clip to play when shooting")]
    public AudioClip ShootClip;


    //So you can't hold down the shoot button
    private bool fired = true;
    private Rigidbody2D rigidb;
    private CameraController cc;
    private Stopwatch fireDelay;


    void Start()
    {
        fireDelay = new Stopwatch();
        rigidb = GetComponent<Rigidbody2D>();
        cc = Camera.main.GetComponent<CameraController>();
    }

    void Update()
    {
        if (fireDelay.IsRunning && fireDelay.ElapsedMilliseconds > fireInterval * 1000)
        {
            fireDelay.Stop();
        }
    }

    public void Fire()
    {
        if (fireDelay.ElapsedMilliseconds > fireInterval * 1000 || !fireDelay.IsRunning)
        {
            //Spawn bullet
            GameObject b = Instantiate(Bullet, new Vector3(transform.position.x, transform.position.y, Bullet.transform.position.z), transform.rotation);
            b.GetComponent<Tile>().Bounds = Bounds;
            b.GetComponent<BulletController>().Life = BulletLife;
            Rigidbody2D rb = b.GetComponent<Rigidbody2D>();
            b.transform.position += transform.right * 0.1f;
            //Set the speed of the bullet
            rb.velocity = transform.right * BulletSpeed;
            rb.angularVelocity = 0;

            //Kickback for parent
            rigidb.AddForce(rb.velocity * -Kickback);
            //Camera shake
            cc.Shake(0.2f, new Vector3(0.25f, 0.25f, 0));
            fireDelay.Restart();

            if (ShootClip != null)
            {
                Tools.PlayClip(ShootClip, Random.Range(0.4f, 0.7f));
            }
        }
    }
}
