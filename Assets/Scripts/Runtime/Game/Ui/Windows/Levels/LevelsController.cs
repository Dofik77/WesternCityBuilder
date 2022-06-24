using System;
using DG.Tweening;
using Leopotam.Ecs;
using Lofelt.NiceVibrations;
using Runtime.Data;
using Runtime.Data.PlayerData.Currency;
using Runtime.Data.PlayerData.Levels;
using Runtime.Game.Ui.Extensions;
using Runtime.Game.Ui.Impls;
using Runtime.Game.Ui.Objects;
using Runtime.Game.Ui.Objects.Layouts;
using Runtime.Services.AnalyticsService.Impls;
using Runtime.Services.UiData.Data;
using SimpleUi.Signals;
using UniRx;
using UnityEngine;
using Utils;
using Zenject;

namespace Runtime.Game.Ui.Windows.Levels
{
    public class LevelsController : UiControllerExtended<LevelsView>, IInitializable
    {
        [Inject] private readonly ILevelsData _levelsData;
        [Inject] private readonly ICurrenciesData _currenciesData;

        private EcsWorld _world;

        private const float _fadeDuration = 0.45f;
        private const float _appearDuration = 0.6f;
        private const string _vip_mask = "vip_";
        private Vector2 _windowHidePos = new Vector2(0, 3000);

        public LevelsController(EcsWorld world) => _world = world;

        public void Initialize()
        {
            View.BackBtn.OnClickAsObservable().Subscribe(x => OnBack()).AddTo(View.BackBtn);
            BeforeHideEvent = new BeforeActionEvent(OnBeforeHide, _appearDuration);
            
            View.UiBox.Init(_windowHidePos, _appearDuration);
            
            InitValues();
            InitUiData();
        }

        public override void InitUiData()
        {
            var uiData = _uiData.GetData();
            if (uiData.LevelsUiData.Positions.Length != View.LayoutContainer.LayoutGroups.Length)
            {
                uiData.LevelsUiData = new UiData.LayoutContainerData(View.LayoutContainer.LayoutGroups.Length);
                _uiData.Save(uiData);
            }
            
            View.RebuildUiData(uiData.LevelsUiData);
        }
        
        public void InitValues()
        {
            var data = _commonPlayerData.GetData();
            
            View.InitCurrencies(ref data.Money, _currenciesData);
            View.LayoutContainer.Init(_levelsData.Get(), data, OnButton,
                key => !data.Levels.Contain(key) && !key.StartsWith(_vip_mask),
                key => !data.Levels.Contain(key) && key.StartsWith(_vip_mask),
                key => data.CurrentLevel == key);
            
            foreach (var group in View.LayoutContainer.LayoutGroups)
                if (group.GeneratedLayout.DataKeyMask.StartsWith(_vip_mask))
                    group.GeneratedLayout.DoWithEachEntity(InitStars);
            
            UiLevelEntity levelEntity;
            Services.CommonPlayerData.Data.Levels.LevelProperty property;
            Level level;
            void InitStars(UiGeneratedEntity entity)
            {
                levelEntity = (UiLevelEntity) entity;
                if (!data.Levels.Contain(levelEntity.Key))
                    return;
                property = data.Levels.Properties.Get(levelEntity.Key);
                level = _levelsData.Get().Get(levelEntity.Key);

                if (property.TotalTime < 0)
                    return;
                levelEntity.Star1.Switch(property.IsFirstObjectiveComplete);
                levelEntity.Star2.Switch(property.IsSecondObjectiveComplete);
                levelEntity.Star3.Switch(property.IsThirdObjectiveComplete);
            }

            void OnButton(UiGeneratedEntity entity, UiGeneratedLayout layout)
            {
                Level level = _levelsData.Get().Get(entity.Key);
                var data = _commonPlayerData.GetData();
                Action sceneLoadAction = null;
                switch (entity.CurrentUiState)
                {
                    case EUiEntityState.Available:
                        HapticPatterns.PlayPreset(HapticPatterns.PresetType.RigidImpact);
                        data.Levels.Add(entity.Key);
                        data.Money.Add(level.GetCurrency(), -level.GetCost());
                        View.GetCurrencyBox(level.GetCurrency()).Value.text = data.Money.Get(level.GetCurrency()).ToString();
                        entity.SetState(EUiEntityState.Action);
                        View.LayoutContainer.UpdatePurchaseStates(_levelsData.Get(), ref _commonPlayerData.GetData().Money.CurrencyValues);
                        _analyticsService.SendDesignEvent(AnalyticsState.Get, "VipLevel", level.GetLowerKey());
                        _analyticsService.SendDesignEvent(AnalyticsState.Spent, Enum.GetName(typeof(ECurrency), level.GetCurrency()), level.GetCost().ToString());
                        break;

                    case EUiEntityState.Unavailable:
                        SelectImpact(entity, HapticPatterns.PresetType.SoftImpact, 0.3f);
                        break;

                    case EUiEntityState.Action:
                        HapticPatterns.PlayPreset(HapticPatterns.PresetType.RigidImpact);
                        layout.CurrentGreyEntity?.SetState(EUiEntityState.Action);
                        entity.SetState(EUiEntityState.Grey);
                        layout.CurrentGreyEntity = entity;

                        data.SetNextLevel(entity.Key);
                        SaveUiData();
                        sceneLoadAction = () => _sceneLoadingManager.LoadScene(data.CurrentLevel);

                        break;
                    case EUiEntityState.Grey:
                        break;
                }

                _commonPlayerData.Save(data);
                sceneLoadAction?.Invoke();
            }

            void SelectImpact(UiGeneratedEntity entity, HapticPatterns.PresetType type, float duration)
            {
                HapticPatterns.PlayPreset(type);
                entity.CurrentStateObjective.SetSelected(true);
                _delayService.Do(duration, () => entity.CurrentStateObjective.SetSelected(false));
            }
        }

        public override void OnAfterShow()
        {
            var data = _commonPlayerData.GetData();
            View.InitCurrencies(ref data.Money, _currenciesData);
            View.LayoutContainer.UpdateGeneratedLayoutData(_levelsData.Get(), data);
            
            _pauseService.PauseGame(true);
            View.Background.DoAppearColor(_fadeDuration).SetEase(Ease.InQuart);
            View.UiBox.DoToDefault().SetDelay(_fadeDuration).SetEase(Ease.OutCubic);
            _delayService.Do(_appearDuration, () => EnableInput(true));
        }

        public override void OnBeforeHide()
        {
            View.Background.DoDisappearColor(_appearDuration).SetEase(Ease.OutQuart);
            SaveUiData();
        }
        
        public override void EnableInput(bool value)
        {
            View.BackBtn.interactable = value;
        }
        
        

        public void OnBack()
        {
            EnableInput(false);
            View.UiBox.DoOutOfBounds().OnComplete(() =>
            {
                _signalBus.OpenWindow<GameHudWindow>();
                _pauseService.PauseGame(false);
            }).SetEase(Ease.InCubic);
        }
        
        private void SaveUiData()
        {
            var uiData = _uiData.GetData();
            uiData.LevelsUiData.Set(View.LayoutContainer);
            _uiData.Save(uiData);
        }
    }
}