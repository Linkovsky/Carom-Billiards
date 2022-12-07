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

        private void Start()
        {
            if (GameManager.Instance != null)
                GameManager.Instance.uiManager = this;
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

