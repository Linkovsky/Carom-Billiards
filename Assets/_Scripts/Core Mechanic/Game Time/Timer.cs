using System;
using System.Security.Cryptography.X509Certificates;
using TMPro;

namespace CaromBilliards.CoreMechanic.Time
{
    [Serializable]
    public struct Timer
    {
        public TextMeshProUGUI TimeElapsed
        {
            get => _timeElapsedText;

            set => _timeElapsedText = value; 
        }
        
        
        private TextMeshProUGUI _timeElapsedText;
        
        public float timeElapsedInUnity;
        private float _time;
        private int _seconds;
        private int _minutes;
        private int _hours;
        
        /// <summary>
        /// Updates the time as time goes using deltatime of unity. Increment the variable over time and save it on
        /// another variable for us to be able to make some mathematics do determine the seconds, minutes and hours.
        /// Then we assign the values to the textmeshpro formatting the string correctly. 
        /// </summary>
        public void HowMuchTimePassedSinceStart()
        {
            timeElapsedInUnity += UnityEngine.Time.deltaTime;
            _time = timeElapsedInUnity;
            _seconds = (int)(_time % 60); // return the remainder of the seconds divide by 60 as an int
            _time /= 60;
            _minutes = (int)(_time % 60); //return the remainder of the minutes divide by 60 as an int
            _time /= 60;
            _hours = (int)(_time % 24); // return the remainder of the hours divided by 60 as an int
            _time /= 24;
            _timeElapsedText.text = String.Format("{0}:{1}:{2}", _hours.ToString("00"), _minutes.ToString("00"),
                _seconds.ToString("00"));
        }
    }
}
