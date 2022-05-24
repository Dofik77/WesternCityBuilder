using DG.Tweening;
using Leopotam.Ecs;
using Runtime.Data;
using Runtime.Data.PlayerData.Levels;
using Runtime.Game.Ui.Extensions;
using Runtime.Game.Ui.Impls;
using Runtime.Game.Ui.Windows.InGameMenu;
using Runtime.Game.Ui.Windows.Levels;
using Runtime.Game.Ui.Windows.Store;
using Runtime.Signals;
using SimpleUi.Signals;
using UniRx;
using UnityEngine;
using Utils;
using Zenject;

namespace Runtime.Game.Ui.Windows.InGameButtons
{
    public class InGameButtonsController : UiControllerExtended<InGameButtonsView>, IInitializable
    {
        [Inject] private readonly ILevelsData _levelsData;
        private EcsWorld _world;

        private int _devModeCount = 0;
        private int _devModeUseBtn = 10;

        private Vector2 _topHidePos = new Vector2(0, 400);
        private Vector2 _bottomHidePos = new Vector2(0, -400);
        private float _appearDuration = 0.4f;
        private int _shakeEveryNRollbacks = 2;

        public InGameButtonsController(EcsWorld world) => _world = world;

        public void Initialize()
        {
            View.DeveloperModeBtn.OnClickAsObservable().Subscribe(x => OnDevMode()).AddTo(View);
            View.InGameMenuBtn.OnClickAsObservable().Subscribe(x => _signalBus.OpenWindow<InGameMenuWindow>()).AddTo(View);
            View.Store.OnClickAsObservable().Subscribe(x => _signalBus.OpenWindow<StoreWindow>()).AddTo(View);
            View.Levels.OnClickAsObservable().Subscribe(x => _signalBus.OpenWindow<LevelsWindow>()).AddTo(View);

            _signalBus.GetStream<SignalLevelDataUpdate>().Subscribe(x => TryShakeHintButton(ref x.Value.UseRollback))
                .AddTo(View);
            _signalBus.GetStream<SignalTimerUpdate>().Subscribe(x => View.Timer.Value.text = x.Value.ToString()).AddTo(View);
            
            InitTapOnStart();
            BeforeHideEvent = new BeforeActionEvent(OnBeforeHide, _appearDuration);

            var data = _commonPlayerData.GetData();
            if (data.CurrentLevel.StartsWith("vip_"))
            {
                View.AdsOff.gameObject.SetActive(false);
                View.Store.gameObject.SetActive(false);
                View.Timer.gameObject.SetActive(true);
            }
        }

        public void InitTapOnStart()
        {
            if (View.StartToPlayBtn.gameObject.activeSelf)
            {
                View.StartToPlayBtn.OnClickAsObservable().Subscribe(x => OnStartToPlay()).AddTo(View.StartToPlayBtn);
                View.UiBox.gameObject.SetActive(false);
                View.StartToPlayBtn.transform.DOScale(0.1f, 1.5f).SetRelative(true).SetLoops(-1, LoopType.Yoyo);
            }

            void OnStartToPlay()
            {
                View.UiBox.gameObject.SetActive(true);
                View.StartToPlayBtn.gameObject.SetActive(false);
            }
        }

        public override void OnAfterShow()
        {
            // View.LevelN.text = Enum.GetName(typeof(EScene), _commonPlayerData.GetData().Level)?.Replace("_", " ");
            View.LevelN.text = _levelsData.Get().Get(_commonPlayerData.GetData().CurrentLevel.ToLower()).GetName();
            View.Top.gameObject.SetActive(true);
            View.Bottom.gameObject.SetActive(true);
            View.Top.DoFromPosition(_topHidePos, _appearDuration).SetEase(Ease.OutCubic);
            View.Bottom.DoFromPosition(_bottomHidePos, _appearDuration).SetEase(Ease.OutCubic);
            _delayService.Do(_appearDuration, () => EnableInput(true));
        }

        public override void OnBeforeHide()
        {
            //TODO
            EnableInput(false);
            View.Top.DoToPosition(_topHidePos, _appearDuration, () => View.Top.gameObject.SetActive(false)).SetEase(Ease.InCubic);
            View.Bottom.DoToPosition(_bottomHidePos, _appearDuration, () => View.Bottom.gameObject.SetActive(false)).SetEase(Ease.InCubic);
        }
        
        public override void EnableInput(bool value)
        {
            View.DeveloperModeBtn.interactable = value;
            View.InGameMenuBtn.interactable = value;
            View.Store.interactable = value;
            View.Levels.interactable = value;
            View.HintBtn.interactable = value;
            View.AdsOff.interactable = value;
        }

        private void TryShakeHintButton(ref int useRollback)
        {
            if (useRollback % _shakeEveryNRollbacks != 0)
                return;

            View.HintBtn.rectTransform.DOShakeAnchorPos(0.3f, Vector2.one * 30, 4).SetDelay(1.5f).SetEase(Ease.InCubic);
        }

        private void OnDevMode()
        {
            _devModeCount++;
            if (_devModeCount >= _devModeUseBtn)
            {
                _devModeCount = 0;
                _signalBus.Fire(new SignalDeveloperMode());
            }
        }
    }
}