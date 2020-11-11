using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class ShowScore : MonoBehaviour
{
    private Text text;
    private GameManager game;
    private HealthController playerH;
    private bool firstFrame = true;
    // Start is called before the first frame update
    void Start()
    {
        game = Tools.GetGame();
        text = GetComponent<Text>();
        game.State.ScoreChanged += () => UpdateText(null, null);
        playerH = game.Player.GetComponent<HealthController>();
        playerH.PropertyChanged += UpdateText;
    }

    private void LateUpdate()
    {
        if (firstFrame)
        {
            UpdateText(null, null);
            firstFrame = false;
        }
    }

    private void UpdateText(object sender, PropertyChangedEventArgs e)
    {
        text.text = $"{game.State.Score}\n{new string('♥', playerH.Health)}";
    }
}
