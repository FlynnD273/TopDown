/**************************
 * File: EnemyController
 * Author: Flynn Duniho
 * Description: Enemy Movement, and hit effects
**************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;
using System;
using System.ComponentModel;

public class EnemyController : MonoBehaviour
{
    [Tooltip("GameObject to follow")]
    public GameObject Target;

    [Tooltip("Movement speed multiplier")]
    public float Speed = 5;

    [Tooltip("GameObjects to spawn when dead")]
    public GameObject[] OnDeath;

    [Tooltip("GameObjects to spawn when hit")]
    public GameObject[] OnHit;

    [Tooltip("Clip to play when hit")]
    public AudioClip HitClip;

    [Tooltip("Clip to play when dead")]
    public AudioClip DieClip;


    private WrapAround w;
    private Tile tile;
    private Rigidbody2D rb;
    private ParticleSystem ps;
    private HealthController health;
    private Animator anim;
    private List<Animator> anims;


    void Start()
    {
        anims = new List<Animator>();
        tile = GetComponent<Tile>();
        w = GetComponent<WrapAround>();
        rb = GetComponent<Rigidbody2D>();
        ps = GetComponent<ParticleSystem>();
        health = GetComponent<HealthController>();
        anim = GetComponent<Animator>();

        health.PropertyChanged += HealthChanged;
        health.Die += Died;

        BoxCollider2D hitbox = gameObject.GetComponent<BoxCollider2D>();
        foreach (GameObject o in tile.Children)
        {
            o.AddComponent(hitbox);
            o.AddComponent(rb);
            anims.Add(o.AddComponent(anim));
        }

        Tools.GetGame().State.TotalEnemies++;
    }

    /// <summary>
    /// Called when health has changed
    /// </summary>
    private void HealthChanged(object sender, PropertyChangedEventArgs e)
    {
        Tools.PlayClip(HitClip, 0.5f);
        //Spawn all the hit effects
        foreach (GameObject o in OnHit)
        {
            Instantiate(o, transform.position, transform.rotation);
            foreach (GameObject c in tile.Children)
            {
                Instantiate(o, c.transform.position, c.transform.rotation);
            }
        }

        //Update the animation state
        anim.SetInteger("Health", health.Health);
        foreach (Animator a in anims)
        {
            a.SetInteger("Health", health.Health);
        }
    }

    /// <summary>
    /// Called when health reaches 0
    /// </summary>
    private void Died()
    {
        Camera.main.GetComponent<CameraController>().Shake(0.3f, new Vector2(0.75f, 0.75f));
        Tools.PlayClip(DieClip, 0.5f);

        foreach (GameObject o in OnDeath)
        {
            Instantiate(o, transform.position, transform.rotation);
        }

        var g = Tools.GetGame();
        g.State.TotalEnemies--;
        g.State.Score += 10;

        g.Player.GetComponent<PlayerController>().Invincible(0.5f);

        //Remove all instances
        tile.Remove();
    }

    private void FixedUpdate()
    {
        Vector3 tar = Tools.GetClosestPoint(transform.position, Target.transform.position, w.Bounds.bounds);

        //Move toward the target
        rb.AddForce((tar - transform.position) * Speed);
    }

    //Hit by bullet
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Bullet"))
        {
            health.Health--;
        }
    }
}
