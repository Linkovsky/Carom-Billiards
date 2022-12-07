using System;

namespace CaromBilliards.CoreMechanic.SaveSystem
{
    [Serializable]
    public struct SaveManager
    {
        public int score;
        public int shotsMade;
        public float timeElapsed;
    }
}

