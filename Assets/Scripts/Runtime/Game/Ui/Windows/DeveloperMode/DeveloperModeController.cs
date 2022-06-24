using Leopotam.Ecs;
using Runtime.Game.Ui.Extensions;
using Runtime.Game.Ui.Impls;
using Runtime.Services.CommonPlayerData.Data;
using Runtime.Services.MonetizationService;
using Runtime.Services.UiData.Data;
using Runtime.Signals;
using UniRx;
using Zenject;

namespace Runtime.Game.Ui.Windows.DeveloperMode
{
    public class DeveloperModeController : UiControllerExtended<DeveloperModeView>, IInitializable
    {
        [Inject] private readonly IMonetizationService _monetizationService;

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
#if  UNITY_EDITOR
            _monetizationService.SetAdsRemoved(false);
#endif
            _commonPlayerData.Save(new CommonPlayerData());
            _uiData.Save(new UiData());
            _sceneLoadingManager.LoadScene(_commonPlayerData.GetData().CurrentLevel);
        }
    }
}