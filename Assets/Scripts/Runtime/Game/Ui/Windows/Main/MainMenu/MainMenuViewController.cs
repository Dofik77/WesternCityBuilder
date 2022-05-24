using Runtime.Game.Ui.Extensions;
using Runtime.Services.CommonPlayerData;
using Runtime.Services.CommonPlayerData.Data;
using Runtime.Services.SceneLoading;
using SimpleUi.Abstracts;
using SimpleUi.Interfaces;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Runtime.Game.Ui.Windows.Main.MainMenu
{
    public class MainMenuViewController : UiController<MainMenuView>, IInitializable, IDefaultSelectable
    {
        [Inject] private readonly ICommonPlayerDataService<CommonPlayerData> _commonPlayerData;
        private readonly SignalBus _signalBus;
        private readonly ISceneLoadingManager _sceneLoadingManager;

        public Selectable DefaultSelectable => View.PlayGame;

        public MainMenuViewController(SignalBus signalBus, ISceneLoadingManager sceneLoadingManager)
        {
            _signalBus = signalBus;
            _sceneLoadingManager = sceneLoadingManager;
        }

        public void Initialize()
        {
            View.PlayGame.OnClickAsObservable().Subscribe(x => OnPlayGame()).AddTo(View.PlayGame);
            View.Exit.OnClickAsObservable().Subscribe(x => Exit()).AddTo(View.Exit);
        }
        private void OnPlayGame()
        {
            _sceneLoadingManager.LoadScene(_commonPlayerData.GetData().CurrentLevel);
        }

        private void Exit() => Application.Quit();
    }
}