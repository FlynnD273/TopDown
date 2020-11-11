/**************************
 * File: GameInfo
 * Author: Flynn Duniho
 * Description: Stores information about the state of the game
**************************/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public class GameState : NotifyPropertyChangedBase
    {
        private int _totalEnemies = 0;
        public int TotalEnemies
        {
            get { return _totalEnemies; }
            set { UpdateField(ref _totalEnemies, value); }
        }

        private int _score = 0;
        public int Score
        {
            get { return _score; }
            set { UpdateField(ref _score, value); ScoreChanged?.Invoke(); }
        }

        public event Action ScoreChanged;

        private Stopwatch GameTime;
        //How long the game has been running for, in seconds
        public float GameSeconds { get => GameTime.ElapsedMilliseconds / 1000f; }

        public GameState ()
        {
            GameTime = new Stopwatch();
            GameTime.Start();
        }
    }
}
