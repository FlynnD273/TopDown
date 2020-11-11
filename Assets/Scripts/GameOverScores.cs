/**************************
 * File: GameOverScore
 * Author: Flynn Duniho
 * Description: Display the score at the end of the game
**************************/
using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScores : MonoBehaviour
{
    private Text text;
    private GameManager game;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
        game = Tools.GetGame();
        ShowScores();
    }

    private void ShowScores()
    {
        string high;
        if (!File.Exists("HighScore.txt"))
        {
            File.WriteAllText("HighScore.txt", "0");
            high = "0";
        }
        else
        {
            high = File.ReadAllText("HighScore.txt");
        }

        string prefix = "";
        int h;
        if (int.TryParse(high, out h) && h < game.State.Score)
        {
            high = game.State.Score.ToString();
            File.WriteAllText("HighScore.txt", high);
            prefix = "NEW ";

        }

        text.text = $"SCORE: {game.State.Score}\n{prefix}HIGH SCORE: {high}";
    }
}
