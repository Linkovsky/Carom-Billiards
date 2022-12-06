using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

namespace CaromBilliards.CoreMechanic.ScoreBoard
{
    
    public struct Score
    {
        public bool IsBallMoving { get; private set; }
        
        public TextMeshProUGUI ScoreText
        {
            set => _scoreText = value;
        }

        public int collidedBallCount;
        
        private StringBuilder _scoreStringBuilder;
        private TextMeshProUGUI _scoreText;
        private int _score;
        private string _collidedBallName;

        public void StartBuilder()
        {
            _scoreText.SetText(_scoreStringBuilder = new StringBuilder("Score: ").Insert(7, _score)); 
        }
        
        private void AddScore()
        {
            if (_score == 3) return;
            _score++;
            _scoreStringBuilder.Remove(7, 1).Insert(7, _score);
            _scoreText.SetText(_scoreStringBuilder);
        }
    
    
        public void AddCollidedScore(string ballName)
        {
            if (!IsBallMoving) return;
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
            IsBallMoving = playerRigidbody.velocity.sqrMagnitude > 0.01f;
        }
    }
}
