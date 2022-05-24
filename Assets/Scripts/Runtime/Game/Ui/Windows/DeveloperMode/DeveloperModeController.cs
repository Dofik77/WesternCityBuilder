using Leopotam.Ecs;
using Runtime.Game.Ui.Extensions;
using Runtime.Services.CommonPlayerData;
using Runtime.Services.CommonPlayerData.Data;
using Runtime.Services.DelayService;
using Runtime.Services.SceneLoading;
using Runtime.Services.UiData;
using Runtime.Services.UiData.Data;
using Runtime.Signals;
using SimpleUi.Abstracts;
using UniRx;
using Zenject;

namespace Runtime.Game.Ui.Windows.DeveloperMode
{
    public class DeveloperModeController : UiController<DeveloperModeView>, IInitializable
    {
        [Inject] private readonly ICommonPlayerDataService<CommonPlayerData> _commonPlayerData;
        [Inject] private readonly IUiDataService<UiData> _uiData;
        [Inject] private readonly ISceneLoadingManager _sceneLoadingManager;
        [Inject] private readonly SignalBus _signalBus;
        [Inject] private readonly IDelayService _delayService;

        private EcsWorld _world;

        public DeveloperModeController(EcsWorld world) => _world = world;

        public void Initialize()
        {
            _signalBus.GetStream<SignalDeveloperMode>().Subscribe(x => View.UiBox.gameObject.SetActive(!View.UiBox.gameObject.activeSelf))
                .AddTo(View);
            View.DropProgressBtn.OnClickAsObservable().Subscribe(x => OnDropProggress()).AddTo(View);
            
            View.UiBox.gameObject.SetActive(false);
        }

        public void OnDropProggress()
        {
            _commonPlayerData.Save(new CommonPlayerData());
            _uiData.Save(new UiData());
            _sceneLoadingManager.LoadScene(_commonPlayerData.GetData().CurrentLevel);
        }
    }
}