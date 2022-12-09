using System;
using CaromBilliards.CoreMechanic.ReplayState;
using CaromBilliards.Player_Input;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CaromBilliards.CoreMechanic.UI
{
    public class UIManager : MonoBehaviour
    {
        public GameObject ReplayButton
        {
            get => replayButton;
        }
        [SerializeField] private Animation endScreenAnim;
        [SerializeField] private TextMeshProUGUI shots;
        [SerializeField] private TextMeshProUGUI score;
        [SerializeField] private TextMeshProUGUI time;
        [SerializeField] private GameObject replayButton;
        private BallController _ballController;
        private Replay _replay;
        private void Awake()
        {
            _ballController = GameObject.FindGameObjectWithTag("Player").GetComponent<BallController>();
            _replay = FindObjectOfType<Replay>();
        }

        private void Start()
        {
            if (GameManager.Instance != null)
                GameManager.Instance.uiManager = this;
        }

        private void Update()
        {
            if(GameManager.Instance.totalShotsManager.totalShotsMade > 0)
                replayButton.SetActive(_ballController.CanPlay && !_replay.Playback);
        }

        public void DrawEndScreen(string shotsText, string scoreText, string timeText)
        {
            if (GameManager.Instance != null)
            {
                shots.text = shotsText;
                score.text = scoreText;
                time.text = timeText;
            }
            endScreenAnim.Play();
        }

        public void RestartButton()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}

