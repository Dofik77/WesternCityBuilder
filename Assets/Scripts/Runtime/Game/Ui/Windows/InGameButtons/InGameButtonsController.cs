using DG.Tweening;
using ECS.Game.Components.WesternBuilder_Component;
using ECS.Views;
using Leopotam.Ecs;
using Runtime.Data;
using Runtime.Data.PlayerData.Levels;
using Runtime.Data.PlayerData.Recipe;
using Runtime.Game.Ui.Extensions;
using Runtime.Game.Ui.Impls;
using Runtime.Game.Ui.Windows.InGameMenu;
using Runtime.Game.Ui.Windows.Levels;
using Runtime.Game.Ui.Windows.Store;
using Runtime.Signals;
using SimpleUi.Signals;
using TMPro;
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
            
            _signalBus.GetStream<SignalStorageUpdate>().Subscribe(x => UpdateResourceCounter(x.BuildsView));
            _signalBus.GetStream<SignalEnableResourceCounter>().Subscribe(x => EnableResourceCounter(x.BuildsView));
            
            View.Top.Init(_topHidePos, _appearDuration);
            View.Bottom.Init(_bottomHidePos, _appearDuration);
            
            BeforeHideEvent = new BeforeActionEvent(OnBeforeHide, _appearDuration);

            var data = _commonPlayerData.GetData();
            if (data.CurrentLevel.StartsWith("vip_"))
            {
                View.AdsOff.gameObject.SetActive(false);
                View.Store.gameObject.SetActive(false);
                View.Timer.gameObject.SetActive(true);
            }
        }

        public void EnableResourceCounter(BuildsView buildView)
        {
            switch (buildView.ObjectType)
            {
                case RequiredObjectType.WoodStorage:
                    View.WoodReserveData.gameObject.SetActive(true);
                    break;
                    
                case RequiredObjectType.RockStorage :
                    View.RockReserveData.gameObject.SetActive(true);
                    break;
                    
                case RequiredObjectType.FoodStorage :
                    View.FoodReserveData.gameObject.SetActive(true);
                    break;
            }

            UpdateResourceCounter(buildView);
        }
        
        public void UpdateResourceCounter(BuildsView buildsView)
        {
            if (buildsView.Entity.Has<BuildStorageComponent>())
            {
                var updateValue = buildsView.Entity.Get<BuildStorageComponent>().CurrentResourceInStorage;
                var maxInStorage = buildsView.Entity.Get<BuildStorageComponent>().MaxResource;
                
                switch (@buildsView.ObjectType)
                {
                    case RequiredObjectType.WoodStorage:
                        View.WoodReserveData.CounterText.text = updateValue.ToString() + "/" + maxInStorage.ToString();
                        break;
                    
                    case RequiredObjectType.RockStorage :
                        View.RockReserveData.CounterText.text = updateValue.ToString() + "/" + maxInStorage.ToString();
                        break;
                    
                    case RequiredObjectType.FoodStorage :
                        View.FoodReserveData.CounterText.text = updateValue.ToString() + "/" + maxInStorage.ToString();
                        break;
                }
            }
            
        }

        public override void OnAfterShow()
        {
            // View.LevelN.text = Enum.GetName(typeof(EScene), _commonPlayerData.GetData().Level)?.Replace("_", " ");
            View.LevelN.text = _levelsData.Get().Get(_commonPlayerData.GetData().CurrentLevel.ToLower()).GetName();
            View.Top.DoToDefault().SetEase(Ease.OutCubic);
            View.Bottom.DoToDefault().SetEase(Ease.OutCubic);
            _delayService.Do(_appearDuration, () => EnableInput(true));
        }
        
        public override void OnBeforeHide()
        {
            //TODO
            EnableInput(false);
            View.Top.DoOutOfBounds().SetEase(Ease.InCubic);
            View.Bottom.DoOutOfBounds().SetEase(Ease.InCubic);
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