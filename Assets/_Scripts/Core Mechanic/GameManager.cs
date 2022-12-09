using System.IO;
using System.Text;
using CaromBilliards.CoreMechanic.SaveSystem;
using CaromBilliards.CoreMechanic.ScoreBoard;
using CaromBilliards.CoreMechanic.Shots;
using CaromBilliards.CoreMechanic.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Timer = CaromBilliards.CoreMechanic.Time.Timer;

namespace CaromBilliards.CoreMechanic
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        
        [FormerlySerializedAs("_scoreText")] [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI shotsText;
        [SerializeField] private TextMeshProUGUI timeElapsedText;
        
        public Score scoreManager;
        public TotalShots totalShotsManager;
        public UIManager uiManager;
        private Rigidbody _playerRigidbody;
        private StringBuilder _totalShotsStringBuilder;
        private SaveManager _saveGame;
        private Timer _playTimeManager;
        private int _totalShotsMade;
        private string _toJSON;
        private string _filePath;

        private void OnEnable() => Score.GameCompleted += GameCompleted;
        private void OnDisable() => Score.GameCompleted -= GameCompleted;
        
        /// <summary>
        /// The subscription of the event, if the game is completed we call the receivevalues to then call the
        /// createjsonfile. In the end we fire the end screen of the ui.
        /// </summary>
        private void GameCompleted()
        {
            ReceiveValues();
            CreateJsonAndFile();
            uiManager.DrawEndScreen(shotsText.text, scoreText.text, timeElapsedText.text);
        }

        private void Awake()
        {
            Instance = this;
            _playerRigidbody = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
        }

        private void Start()
        {
            _filePath = Application.persistentDataPath + "/save.sv";
            scoreManager.ScoreText = scoreText;
            scoreManager.StartBuilder();
            totalShotsManager.ShotsText = shotsText;
            totalShotsManager.StartBuilder();
            _playTimeManager.TimeElapsed = timeElapsedText;
        }
        
        private void Update()
        {
            if (scoreManager.gameCompleted) return;
            if (!scoreManager.isBallMoving && scoreManager.collidedBallCount == 1)
                scoreManager.collidedBallCount = 0;
            scoreManager.IsTheBallMoving(_playerRigidbody);
            _playTimeManager.HowMuchTimePassedSinceStart();
        }
        
        /// <summary>
        /// We create the json string we want to save and assign it to a string, then using the filestream we open
        /// the path where we want to save the data and using streamwriter we write to the file the string we got
        /// from the jsonutility
        /// </summary>
        private void CreateJsonAndFile()
        {
            _toJSON = JsonUtility.ToJson(_saveGame);
            using FileStream fs = new FileStream(_filePath, FileMode.Create);
            using StreamWriter streamWriter = new StreamWriter(fs);
            streamWriter.Write(_toJSON);
            streamWriter.Close();
            fs.Close();
        }
        
        /// <summary>
        /// We receive the values from the ui manager that tell us what are the en dresults
        /// </summary>
        private void ReceiveValues()
        {
            _saveGame.score = scoreManager.score;
            _saveGame.shotsMade = totalShotsManager.totalShotsMade;
            _saveGame.timeElapsed = _playTimeManager.timeElapsedInUnity;
        }

        public void ExitGame()
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}