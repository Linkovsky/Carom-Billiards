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
        public int totalShotsMade { get; private set; }
        public TextMeshProUGUI ShotsText
        {
            set => _shotsText = value;
        }
       
        private StringBuilder _totalShotsStringBuilder;
        private TextMeshProUGUI _shotsText;

        public void StartBuilder()
        {
            _shotsText.SetText( _totalShotsStringBuilder = new StringBuilder("Shots: ").Insert(7, totalShotsMade));
        }
        /// <summary>
        /// we increment the total shots the player made through the game and display it in the UI
        /// </summary>
        public void SetTotalShots()
        {
            int totalNum = FindLengthOfInt(totalShotsMade);
            totalShotsMade++;
            _totalShotsStringBuilder.Remove(7, totalNum).Insert(7, totalShotsMade);
            _shotsText.SetText(_totalShotsStringBuilder);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="number">The number to increment</param>
        /// <returns>The base of the number, then the floor of the number, converted to an int</returns>
        private static int FindLengthOfInt(int number)
        {
            if (number == 0)
                return 1;
            return (int)Mathf.Floor(Mathf.Log10(number)) + 1;
        }
    }
    
}
