using System;
using System.Linq;
using DG.Tweening;
using Leopotam.Ecs;
using Runtime.Data.PlayerData.Currency;
using Runtime.Data.PlayerData.Levels;
using Runtime.Data.PlayerData.Skins;
using Runtime.Game.Ui.Extensions;
using Runtime.Game.Ui.Impls;
using Runtime.Game.Ui.Objects.Purchase;
using Runtime.Services.AnalyticsService.Impls;
using Runtime.Services.CommonPlayerData.Data;
using SimpleUi.Signals;
using UniRx;
using UnityEngine;
using Utils;
using Zenject;

namespace Runtime.Game.Ui.Windows.Purchases
{
    public class PurchasesController : UiControllerExtended<PurchasesView>, IInitializable
    {
        [Inject] private readonly ICurrenciesData _currenciesData;
        [Inject] private readonly ISkinsData _skinsData;
        [Inject] private readonly ILevelsData _levelsData;

        private EcsWorld _world;

        private const float _fadeDuration = 0.45f;
        private const float _appearDuration = 0.6f;
        private Vector2 _windowHidePos = new Vector2(0, 3000);

        public PurchasesController(EcsWorld world) => _world = world;

        public void Initialize()
        {
            View.BackBtn.OnClickAsObservable().Subscribe(x => OnBack()).AddTo(View.BackBtn);
            BeforeHideEvent = new BeforeActionEvent(OnBeforeHide, _appearDuration);
            
            View.UiBox.Init(_windowHidePos, _appearDuration);
            
            InitValues();
            InitUiData();
        }

        public void InitValues()
        {
            foreach (var purchaseEntity in View.CurrencyPurchaseEntities)
                purchaseEntity.Init();
            View.Part1VipUnlock.Init(Part1VipUnlockAction);
            View.LootboxReward.Init(LootboxRewardAction);
            View.VipLevelReward.Init(VipLevelRewardAction);

            void Part1VipUnlockAction()
            {
                var data = _commonPlayerData.GetData();
                foreach (var level in _levelsData.Get())
                {
                    if (!level.GetLowerKey().StartsWith("vip_part_1"))
                        continue;
                    if (data.Levels.Contain(level.GetLowerKey()))
                        continue;
                    data.Levels.Add(level.GetLowerKey());
                    _analyticsService.SendDesignEvent(AnalyticsState.Get, "VipLevel", level.GetLowerKey());
                }

                _commonPlayerData.Save(data);
            }

            void LootboxRewardAction()
            {
                var data = _commonPlayerData.GetData();
                foreach (var skin in _skinsData.Get().OrderBy(x => x.GetCost()).ToList())
                {
                    if (skin.GetCurrency() != ECurrency.Soft)
                        continue;
                    if (skin.GetCost() == 0)
                        continue;
                    if (data.Skins.Contain(skin.GetLowerKey()))
                        continue;
                    data.Skins.Add(skin.GetLowerKey());
                    _commonPlayerData.Save(data);
                    _analyticsService.SendDesignEvent(AnalyticsState.Get, "CasualSkin", skin.GetLowerKey());
                    return;
                }
            }

            void VipLevelRewardAction()
            {
                var data = _commonPlayerData.GetData();
                foreach (var level in _levelsData.Get())
                {
                    if (!level.GetLowerKey().StartsWith("vip_"))
                        continue;
                    if (data.Levels.Contain(level.GetLowerKey()))
                        continue;
                    data.Levels.Add(level.GetLowerKey());
                    _commonPlayerData.Save(data);
                    _analyticsService.SendDesignEvent(AnalyticsState.Get, "VipLevel", level.GetLowerKey());
                    return;
                }
            }
        }

        public override void OnAfterShow()
        {
            var data = _commonPlayerData.GetData();
            _pauseService.PauseGame(true);
            View.Background.DoAppearColor(_fadeDuration).SetEase(Ease.InQuart);
            View.UiBox.DoToDefault().SetDelay(_fadeDuration).SetEase(Ease.OutCubic);
            _delayService.Do(_appearDuration, () => EnableInput(true));
        }

        public override void OnBeforeHide()
        {
            View.Background.DoDisappearColor(_appearDuration).SetEase(Ease.OutQuart);
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
    }
}