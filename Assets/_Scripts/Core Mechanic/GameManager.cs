using System;
using System.Text;
using CaromBilliards.CoreMechanic.Time;
using CaromBilliards.CoreMechanic.ScoreBoard;
using CaromBilliards.CoreMechanic.Shots;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace CaromBilliards.CoreMechanic
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        public Score scoreManager;
        public TotalShots totalShots;
        
        [FormerlySerializedAs("_scoreText")] [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI shotsText;
        [SerializeField] private TextMeshProUGUI timeElapsedText;
        
        private Rigidbody _playerRigidbody;
        private StringBuilder _totalShotsStringBuilder;
        private Timer _playTime;
        private int _totalShotsMade;
        private void Awake()
        {
            if(Instance != null)
                Destroy(this.gameObject);
            else
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
            _playerRigidbody = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
        }

        private void Start()
        {
            scoreManager.ScoreText = scoreText;
            scoreManager.StartBuilder();
            totalShots.ShotsText = shotsText;
            totalShots.StartBuilder();
            _playTime.TimeElapsed = timeElapsedText;
        }

        private void Update()
        {
            if (!scoreManager.IsBallMoving && scoreManager.collidedBallCount == 1)
                scoreManager.collidedBallCount = 0;
            scoreManager.IsTheBallMoving(_playerRigidbody);
            _playTime.HowMuchTimePassedSinceStart();
        }
    }
}