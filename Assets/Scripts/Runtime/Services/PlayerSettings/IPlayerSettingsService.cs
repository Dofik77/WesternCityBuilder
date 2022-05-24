namespace Runtime.Services.PlayerSettings
{
    public interface IPlayerSettingsService
    {
        PlayerSettings PlayerSettings { get; }
        void SaveSettings(PlayerSettings settings);
    }
}