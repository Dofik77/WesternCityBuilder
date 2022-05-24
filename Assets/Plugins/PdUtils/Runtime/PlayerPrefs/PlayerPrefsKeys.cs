namespace PdUtils.PlayerPrefs
{
    public enum PlayerPrefsKeys
    {
        MusicOn, SoundFxOn, VibrationOn
    }
    
    public static class PlayerPrefsKeysExtension
    {
        public static string Value(this PlayerPrefsKeys key)
        {
            return key.ToString().ToLower();
        }
    }

}