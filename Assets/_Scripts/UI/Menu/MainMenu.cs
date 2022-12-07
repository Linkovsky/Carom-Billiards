using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CaromBilliards.MainMenu
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI score;
        [SerializeField] private TextMeshProUGUI shots;
        [SerializeField] private TextMeshProUGUI time;
        [SerializeField] private GameObject lastStats;
        private string _fromJSON;
        private string _path;
        private Load _loadFile;
        private void Start()
        {
            _path = Application.persistentDataPath + "/save.sv";
            LoadFromJSON();
        }
    
        private void LoadFromJSON()
        {
            if (File.Exists(_path))
            {
                lastStats.SetActive(true);
                ReadFromJSON();
                AddValuesFromJSON();
            }
        }
    
        private void AddValuesFromJSON()
        {
            score.text = "Score: " + _loadFile.score;
            shots.text = "Shots: " + _loadFile.shotsMade;
            float timer = _loadFile.timeElapsed;
            float seconds = (int)(timer % 60); // return the remainder of the seconds divide by 60 as an int
            timer /= 60;
            float minutes = (int)(timer % 60); //return the remainder of the minutes divide by 60 as an int
            timer /= 60;
            float hours = (int)(timer % 24); // return the remainder of the hours divided by 60 as an int
            time.text = "Time: " + String.Format("{0}:{1}:{2}", hours.ToString("00"), minutes.ToString("00"),
                seconds.ToString("00"));
        }
    
        private void ReadFromJSON()
        {
            using FileStream fileStream = new FileStream(_path, FileMode.Open);
            using StreamReader streamReader = new StreamReader(fileStream);
            _loadFile = JsonUtility.FromJson<Load>(streamReader.ReadLine());
            streamReader.Close();
            fileStream.Close();
        }

        public void StartNewGame()
        {
            SceneManager.LoadScene(1);
        }
        public void ApplicationExit()
        {
            Application.Quit();
        }
    }
    
    public struct Load
    {
        public int score;
        public int shotsMade;
        public float timeElapsed;
    }
}

