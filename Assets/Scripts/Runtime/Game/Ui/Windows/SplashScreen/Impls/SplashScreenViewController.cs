using Runtime.Services.CommonPlayerData;
using Runtime.Services.CommonPlayerData.Data;
using Runtime.Services.SceneLoading;
using Runtime.Services.UiData;
using Runtime.Services.UiData.Data;
using SimpleUi.Abstracts;
using Zenject;

namespace Runtime.Game.Ui.Windows.SplashScreen.Impls
{
    public class SplashScreenViewController : UiController<SplashScreenView>, IInitializable
    {
        [Inject] private readonly ICommonPlayerDataService<CommonPlayerData> _commonPlayerData;
        [Inject] private readonly IUiDataService<UiData> _uiData;
        private readonly ISceneLoadingManager _sceneLoadingManager;

        public SplashScreenViewController(ISceneLoadingManager sceneLoadingManager)
        {
            _sceneLoadingManager = sceneLoadingManager;
        }
        
        public void Initialize()
        {
            // _commonPlayerData.Save(new CommonPlayerData());
            // _uiData.Save(new UiData());
            _sceneLoadingManager.LoadScene(_commonPlayerData.GetData().CurrentLevel);
        }
    }
}