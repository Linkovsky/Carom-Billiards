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
        
        private void AddScore()
        {
            score++;
            if (score == 3) {gameCompleted = true; GameCompleted?.Invoke();}
            _scoreStringBuilder.Remove(7, 1).Insert(7, score);
            _scoreText.SetText(_scoreStringBuilder);
        }
    
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
