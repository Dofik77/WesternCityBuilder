using System;

namespace DataBase.Timer
{
    [Serializable]
    public struct Timer
    {
        public int Hours;
        public int Minutes;
        public int Seconds;


        public Timer(int minutes, int seconds, int hours)
        {
            Minutes = minutes;
            Seconds = seconds;
            Hours = hours;
        }
        
        public float ToFloat()
        {
            return Seconds + Minutes * 60 + Hours * 60 * 60;
        }
        public void Increment(float timer)
        {
            Seconds = TimeSpan.FromSeconds(timer).Seconds;
            Minutes = TimeSpan.FromSeconds(timer).Minutes;
            Hours = TimeSpan.FromSeconds(timer).Hours;
        }
        /// <summary>
        /// Returns value in seconds
        /// </summary>
        /// <returns></returns>
        ///
        public int ToInt() => Minutes * 60 + Seconds; 
        public override string ToString() => /*Hours.ToString("00") + ":" + */ Minutes.ToString("00") + ":" + Seconds.ToString("00");
        public bool Equals(Timer other) => Minutes == other.Minutes && Seconds == other.Seconds;
        public static Timer Zero() => new Timer(0,0,0);
        public static Timer Max() => new Timer(int.MaxValue,int.MaxValue,int.MaxValue);
    }
}