/**************************
 * File: PlayerController
 * Author: Flynn Duniho
 * Description: Movement for the player
**************************/
using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Tooltip("Speed of forward movement")]
    public float Speed = 2;

    [Tooltip("Speed of rotation")]
    public float RotationSpeed = 2;

    [Tooltip("Boost multiplier increase per second")]
    public float BoostIncrease = 10f;

    [Tooltip("Maxmimum boost multipler used when boosting off an object")]
    public float MaxBoost = 10;

    [Tooltip("Tags of object player can boost off of")]
    public string[] BoostableTags;

    [Tooltip("Tags of object player gets hurt by")]
    public string[] HurtsTags;

    public GameObject SpawnOnHurt;

    [Tooltip("How long the player is invincible for, in seconds")]
    public float InvincibleDuration;

    [Tooltip("Clip to play when moving forward")]
    public AudioClip RocketClip;

    [Tooltip("Clip to play when hit by enemy")]
    public AudioClip HitClip;

    public GameObject Shield;


    private Rigidbody2D rb;
    private bool isBoosting = false;
    //boost multipler
    private float boost = 1;
    private BoxCollider2D boostColl;
    private new AudioSource audio;
    private HealthController health;
    private Stopwatch invincible;
    private bool isHurt;
    private float iDur;
    private SpriteRenderer[] sr;
    private FireBullet bullet;
    private bool fired = false;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector3.zero;
        rb.rotation = 0;
        boostColl = GetComponent<BoxCollider2D>();
        health = GetComponent<HealthController>();
        sr = new SpriteRenderer[] { GetComponent<SpriteRenderer>(), transform.GetChild(0).GetComponent<SpriteRenderer>(), transform.GetChild(2).GetComponent<SpriteRenderer>() };
        audio = GetComponent<AudioSource>();
        audio.clip = RocketClip;
        invincible = new Stopwatch();
        bullet = GetComponent<FireBullet>();
        
        //shield = transform.GetChild(1).GetComponent<SpriteRenderer>();
        //flame = transform.GetChild(0).GetComponent<SpriteRenderer>();

        Tools.GetGame().GameOver += PlayerController_GameOver;
    }

    private void PlayerController_GameOver()
    {
        audio.Stop();
        isBoosting = false;

    }

    private void FixedUpdate()
    {
        //Turn player
        rb.rotation -= Input.GetAxis("Horizontal") * RotationSpeed;
        float mag = Input.GetAxis("Vertical") * Speed;

        if (isBoosting && mag > 0)
        {
            boost += BoostIncrease * Time.deltaTime;
            boost = Mathf.Min(boost, MaxBoost);
            mag *= boost + 1;
        }
        else
        {
            boost = 1;
        }

        if (mag > 0)
        {
            //move forward
            rb.velocity += new Vector2(Mathf.Cos(Mathf.Deg2Rad * rb.rotation) * mag, Mathf.Sin(Mathf.Deg2Rad * rb.rotation) * mag);
            audio.volume = mag / Speed / 4;
            if (!audio.isPlaying)
                audio.Play();
        }
        else
        {
            audio.Stop();
        }

        if (invincible.IsRunning)
        {
            Shield.SetActive(true);

            if (isHurt)
            {
                foreach (var r in sr)
                {
                    r.enabled = (invincible.ElapsedMilliseconds / 250) % 2 == 0;
                }
            }

            if (invincible.ElapsedMilliseconds > iDur * 1000)
            {
                invincible.Stop();
                isHurt = false;
            }
        }
        else
        {
            Shield.SetActive(false);
            foreach (var r in sr)
            {
                r.enabled = true;
            }
        }

        //flame.enabled = sr.enabled;

        //If the fire button is pressed more than halfway
        bool fire = Input.GetAxis("Jump") > 0.5;
        if (!fired && fire)
        {
            fired = true;
            bullet.Fire();
        }
        //Player can shoot again only if been long enough and not trying to shoot
        else if (!fire)
        {
            fired = false;
        }
    }

    //Boost
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (IsBoostable(col.gameObject))
        {
            if (boostColl.IsTouching(col))
            {
                isBoosting = true;
            }
        }
    }

    //Stop boosting
    private void OnTriggerExit2D(Collider2D col)
    {
        if (IsBoostable(col.gameObject))
        {
            if (!boostColl.IsTouching(col))
            {
                isBoosting = false;
            }
        }
    }

    /// <summary>
    /// Figures out if player can boost off of GameObject
    /// </summary>
    /// <param name="o">GameObject to check</param>
    /// <returns>True if player can boost off GameObject, else false</returns>
    private bool IsBoostable(GameObject o)
    {
        foreach (string s in BoostableTags)
        {
            if (o.CompareTag(s))
            {
                return true;
            }
        }
        return false;
    }

    private bool Hurts(GameObject o)
    {
        foreach (string s in HurtsTags)
        {
            if (o.CompareTag(s))
            {
                return true;
            }
        }
        return false;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (Hurts(col.gameObject) && !invincible.IsRunning)
        {
            if (SpawnOnHurt != null)
            {
                Instantiate(SpawnOnHurt, transform.position, Quaternion.identity);
            }
            health.Health--;
            Invincible(InvincibleDuration);
            isHurt = true;
            Tools.PlayClip(HitClip, 0.6f);
            Camera.main.GetComponent<CameraController>().Shake(HitClip.length, new Vector3(0.8f, 0.8f, 0));
        }
    }

    public void Invincible (float duration)
    {
        invincible.Restart();
        iDur = duration;
    }
}
