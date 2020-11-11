/**************************
 * File: GameManager
 * Author: Flynn Duniho
 * Description: Takes care of score, game time, and spawning enemies
**************************/
using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Timers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Manager;

    [Tooltip("Enemy prefab to spawn")]
    public GameObject EnemyObject;

    [Tooltip("Player GameObject")]
    public GameObject Player;

    [Tooltip("Obstacles GameObject")]
    public PolygonCollider2D Level;

    [Tooltip("Length between spawning enemies, in seconds")]
    public float SpawnInterval = 10;

    [Tooltip("The bounds of the field")]
    public BoxCollider2D Bounds;

    [Tooltip("Song clips")]
    public AudioClip[] songs;

    [Tooltip("The maximum number of enemies in game at once")]
    public int MaxEnemies = 50;

    [Tooltip("Object to hide on game over")]
    public GameObject StartShown;

    [Tooltip("Object to show on game over")]
    public GameObject StartHidden;

    public GameState State { get; set; }
    public event Action GameOver;
    
    private Stopwatch enemySpawn;
    private new AudioSource audio;

    private bool firstFrame = true;


    void Awake()
    {
        if (Manager == null)
        {
            Manager = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        State = new GameState();
        enemySpawn = new Stopwatch();
        enemySpawn.Start();
        audio = GetComponent<AudioSource>();
        StartShown.SetActive(true);
        StartHidden.SetActive(false);
    }

    private void SpawnEnemy()
    {
        //Spawn enemy
        GameObject enemy = Instantiate(EnemyObject, RandPos(), Quaternion.Euler(0, 0, 0));
        //Set the target to the player
        enemy.GetComponent<EnemyController>().Target = Player;
        //Set the bounds of the field
        enemy.GetComponent<Tile>().Bounds = Bounds;

        //Move enemy if it's in a wall
        BoxCollider2D hitbox = enemy.GetComponent<BoxCollider2D>();
        //PolygonCollider2D level = Level.GetComponent<PolygonCollider2D>();
        while (Physics2D.OverlapBox(enemy.transform.position, hitbox.bounds.size / 2, 0) == Level || (enemy.transform.position - Tools.GetClosestPoint(enemy.transform.position, Player.transform.position, Bounds.bounds)).magnitude < 20)
        {
            enemy.transform.position = RandPos();
        }
    }

    void Update()
    {
        if (firstFrame)
        {
            firstFrame = false;
            Player.GetComponent<HealthController>().Die += PlayerDie;
        }
        if (Time.timeScale != 0 && enemySpawn.ElapsedMilliseconds > SpawnInterval * 1000 && State.TotalEnemies < MaxEnemies)
        {
            SpawnEnemy();
            enemySpawn.Restart();
        }

        if (!audio.isPlaying && songs.Length > 0)
        {
            audio.clip = songs[UnityEngine.Random.Range(0, songs.Length)];
            audio.Play();
        }
    }

    /// <summary>
    /// Stop play when the player dies
    /// </summary>
    private void PlayerDie()
    {
        GameOver?.Invoke();
        Time.timeScale = 0;
        StartShown.SetActive(false);
        StartHidden.SetActive(true);
    }

    /// <summary>
    /// Create a random Vector within Bounds
    /// </summary>
    /// <returns>Random Vector</returns>
    private Vector3 RandPos()
    {
        return new Vector3(UnityEngine.Random.Range(Bounds.bounds.min.x, Bounds.bounds.max.x), UnityEngine.Random.Range(Bounds.bounds.min.y, Bounds.bounds.max.y), 0);
    }

    /// <summary>
    /// Reload the current scene
    /// </summary>
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }

    /// <summary>
    /// Close the application
    /// </summary>
    public void Quit()
    {
        Application.Quit();
    }
}
