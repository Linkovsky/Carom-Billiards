using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

namespace CaromBilliards.CoreMechanic.Shots
{
    public struct TotalShots
    {
        public TextMeshProUGUI ShotsText
        {
            set => _shotsText = value;
        }
       
        private StringBuilder _totalShotsStringBuilder;
        private TextMeshProUGUI _shotsText;
        
        private int _totalShotsMade;
        
        public void StartBuilder()
        {
            _shotsText.SetText( _totalShotsStringBuilder = new StringBuilder("Shots: ").Insert(7, _totalShotsMade));
        }
        public void SetTotalShots()
        {
            int totalNum = FindLengthOfInt(_totalShotsMade);
            _totalShotsMade++;
            _totalShotsStringBuilder.Remove(7, totalNum).Insert(7, _totalShotsMade);
            _shotsText.SetText(_totalShotsStringBuilder);
        }
        private static int FindLengthOfInt(int number)
        {
            if (number == 0)
                return 1;
            return (int)Math.Floor(Math.Log10(number)) + 1;
        }
    }
    
}
