using System;
using System.Text;
using TMPro;
using UnityEngine;

namespace CaromBilliards.CoreMechanic.ScoreBoard
{
    public struct Score
    {
        public delegate void GameCompletedHandler();
        public static event GameCompletedHandler GameCompleted;
        public bool isBallMoving { get; private set; }
        public bool gameCompleted { get; private set; }
        public int score { get; private set; }
        public TextMeshProUGUI ScoreText
        {
            set => _scoreText = value;
        }

        public int collidedBallCount;
        
        private StringBuilder _scoreStringBuilder;
        private TextMeshProUGUI _scoreText;
        
        private string _collidedBallName;

        public void StartBuilder()
        {
            _scoreText.SetText(_scoreStringBuilder = new StringBuilder("Score: ").Insert(7, score)); 
        }
        /// <summary>
        /// We increment the score by one check to see if the score is equal to 3 so we can invoke the event saying,
        /// the game has finished.
        /// If not, we change what is shown in the ui text using the string builder
        /// </summary>
        private void AddScore()
        {
            score++;
            if (score == 3) {gameCompleted = true; GameCompleted?.Invoke();}
            _scoreStringBuilder.Remove(7, 1).Insert(7, score);
            _scoreText.SetText(_scoreStringBuilder);
        }
        /// <summary>
        /// We check to see if the ball is moving first to avoid any errors.
        /// We the check in a switch statement to see if the ballcount is 0 or 1
        /// of the switch, if it matches the first one we increment the ball count + one.
        /// After a new call to this function is made we check to know if the name we receive as a parameter is not
        /// the same if not we call the shot to tell the player that a perfect touch with the two balls with the cue
        /// ball was made.
        /// </summary>
        /// <param name="ballName">The name of the ball we collided with</param>
        public void AddCollidedScore(string ballName)
        {
            if (!isBallMoving) return;
            switch (collidedBallCount)
            {
                case 0:
                    collidedBallCount++;
                    _collidedBallName = ballName;
                    break;
                case 1:
                    if (_collidedBallName != ballName)
                    {
                        AddScore();
                        collidedBallCount = 0;
                        _collidedBallName = String.Empty;
                    }
                    break;
            }
        }
    
        public void IsTheBallMoving(Rigidbody playerRigidbody)
        {
            // True being the ball is moving, False not moving
            isBallMoving = playerRigidbody.velocity.sqrMagnitude > 0.01f;
        }
    }
}
