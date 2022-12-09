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
            // If the game is completed the button should be hidden away from the player
            if(GameManager.Instance.scoreManager.gameCompleted)
            {
                replayButton.SetActive(false);
                return;
            }
            // The button is active or not based on the conditions if the player can shoot and it's not using playback
            if(GameManager.Instance.totalShotsManager.totalShotsMade > 0)
                replayButton.SetActive(_ballController.CanPlay && !_replay.Playback);
        }
        /// <summary>
        /// Assigns all the values receive to the strings of the ui and plays the animation
        /// </summary>
        /// <param name="shotsText">The incoming result of the shots</param>
        /// <param name="scoreText">The incoming result of the score</param>
        /// <param name="timeText">The incoming result of the time</param>
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