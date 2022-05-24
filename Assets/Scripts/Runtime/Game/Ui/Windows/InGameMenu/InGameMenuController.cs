using DG.Tweening;
using ECS.Game.Components.Flags;
using ECS.Game.Components.General;
using ECS.Utils.Extensions;
using ECS.Views.General;
using Leopotam.Ecs;
using Lofelt.NiceVibrations;
using Runtime.Game.Ui.Extensions;
using Runtime.Game.Ui.Impls;
using Runtime.Services.PauseService;
using Runtime.Services.PlayerSettings;
using Runtime.Services.SceneLoading;
using SimpleUi.Signals;
using UniRx;
using UnityEngine;
using Utils;
using Zenject;

namespace Runtime.Game.Ui.Windows.InGameMenu
{
    public class InGameMenuController : UiControllerExtended<InGameMenuView>, IInitializable
    {
        [Inject] private readonly ISceneLoadingManager _sceneLoadingManager;
        [Inject] private readonly IPlayerSettingsService _playerSettings;
        [Inject] private readonly SignalBus _signalBus;
        [Inject] private readonly IPauseService _pauseService;
        private readonly EcsWorld _world;

        private const float _fadeDuration = 0.45f;
        private const float _appearDuration = 0.6f;
        private Vector2 _bottomHidePos = new Vector2(0, 1500);

        public InGameMenuController(EcsWorld world) => _world = world;

        public void Initialize()
        {
            View.Back.OnClickAsObservable().Subscribe(x => OnBack()).AddTo(View.Back);
            View.PrivacyPolicy.OnClickAsObservable().Subscribe(x => Application.OpenURL(View.PrivacyPolicyURL))
                .AddTo(View.Back);
            View.TermsOfUse.OnClickAsObservable().Subscribe(x => Application.OpenURL(View.TermsOfUseURL))
                .AddTo(View.Back);
            View.VibrationOn.OnClickAsObservable().Subscribe(x => OnVibration(false)).AddTo(View.Back);
            View.VibrationOff.OnClickAsObservable().Subscribe(x => OnVibration(true)).AddTo(View.Back);

            VibrationSwitch(_playerSettings.PlayerSettings.UseVibration);
            
            BeforeHideEvent = new BeforeActionEvent(OnBeforeHide, _appearDuration);
        }

        public override void OnAfterShow()
        {
            _pauseService.PauseGame(true);
            View.Back.gameObject.SetActive(true);
            View.Center.gameObject.SetActive(true);
            View.Background.DoAppearColor(_fadeDuration).SetEase(Ease.InQuart);
            View.Center.DoFromPosition(new Vector2(0f, 1500), _appearDuration).SetDelay(_fadeDuration)
                .SetEase(Ease.OutCubic);
            _delayService.Do(_appearDuration, () => EnableInput(true));
        }

        public override void OnBeforeHide()
        {
            View.Background.DoDisappearColor(_appearDuration, () => View.Back.gameObject.SetActive(false))
                .SetEase(Ease.OutQuart);
        }
        
        public override void EnableInput(bool value)
        {
            View.Back.interactable = value;
            View.PrivacyPolicy.interactable = value;
            View.TermsOfUse.interactable = value;
            View.VibrationOff.interactable = value;
            View.VibrationOn.interactable = value;
        }

        private void OnBack()
        {
            EnableInput(false);
            View.Center.DoToPosition(_bottomHidePos, _appearDuration, () =>
            {
                View.Center.gameObject.SetActive(false);
                _signalBus.OpenWindow<GameHudWindow>();
                _pauseService.PauseGame(false);
            }).SetEase(Ease.InCubic);
        }

        private void OnVibration(bool value)
        {
            VibrationSwitch(value);
            
            var playerSettings = _playerSettings.PlayerSettings;
            playerSettings.UseVibration = value;
            _playerSettings.SaveSettings(playerSettings);
            
           //_world.GetEntity<CameraComponent>().Get<LinkComponent>().Get<CameraView>().HapticReceiver.hapticsEnabled = value;
            
            if (value)
                HapticPatterns.PlayPreset(HapticPatterns.PresetType.SoftImpact);
        }

        private void VibrationSwitch(bool value)
        {
            View.VibrationOn.gameObject.SetActive(value);
            View.VibrationOff.gameObject.SetActive(!value);
        }
    }
}