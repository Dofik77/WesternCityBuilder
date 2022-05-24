using System;

namespace Runtime.Services.PlayerSettings
{
    [Serializable]
    public class PlayerSettings
    {
        public int LanguageIndex;
        public int FontSizeIndex;
        public int ResolutionIndex;
        public bool WindowedMode;
        public float VolumeValue;
        
        public bool UseVibration = true;
    }
}