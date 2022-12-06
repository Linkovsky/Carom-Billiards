using System;
using TMPro;

namespace CaromBilliards.CoreMechanic.Time
{
    public struct Timer
    {
        public TextMeshProUGUI TimeElapsed
        {
            set => _timeElapsedText = value;
        }
        
        private TextMeshProUGUI _timeElapsedText;
        
        private float _timeElapsed;
        private float _time;
        private int _seconds;
        private int _minutes;
        private int _hours;
        
        public void HowMuchTimePassedSinceStart()
        {
            _timeElapsed += UnityEngine.Time.deltaTime;
            _time = _timeElapsed;
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
