using PdUtils.Dao;

namespace Runtime.Services.PlayerSettings.Impl
{
    public class PlayerSettingsService : IPlayerSettingsService
    {
        private readonly IDao<PlayerSettings> _dao;
        private PlayerSettings _playerSettings;
        
        public PlayerSettingsService(IDao<PlayerSettings> dao)
        {
            _dao = dao;
        }
        
        public PlayerSettings PlayerSettings => _playerSettings ??= _dao.Load() ?? new PlayerSettings();

        public void SaveSettings(PlayerSettings settings)
        {
            _playerSettings = settings;
            _dao.Save(settings);
        }
    }
}