using DG.Tweening;
using ECS.Utils.Extensions;
using Leopotam.Ecs;
using Lofelt.NiceVibrations;
using Runtime.Data;
using Runtime.Data.PlayerData.Currency;
using Runtime.Data.PlayerData.Recipe;
using Runtime.Data.PlayerData.Skins;
using Runtime.Game.Ui.Extensions;
using Runtime.Game.Ui.Impls;
using Runtime.Game.Ui.Objects;
using Runtime.Game.Ui.Objects.Layouts;
using Runtime.Services.UiData.Data;
using SimpleUi.Signals;
using UniRx;
using UnityEngine;
using Utils;
using Zenject;

namespace Runtime.Game.Ui.Windows.Store
{
    public class StoreController : UiControllerExtended<StoreView>, IInitializable
    {
        [Inject] private readonly ISkinsData _skinsData;
        [Inject] private readonly ICurrenciesData _currenciesData;
        [Inject] private readonly IRecipeData _recipeData;

        private EcsWorld _world;

        private const float _fadeDuration = 0.45f;
        private const float _appearDuration = 0.6f;
        private Vector2 _windowHidePos = new Vector2(0, 3000);

        public StoreController(EcsWorld world) => _world = world;

        public void Initialize()
        {
            View.BackBtn.OnClickAsObservable().Subscribe(x => OnBack()).AddTo(View.BackBtn);
            BeforeHideEvent = new BeforeActionEvent(OnBeforeHide, _appearDuration);
            InitValues();
            InitUiData();
        }
        
        public override void InitUiData()
        {
            var uiData = _uiData.GetData();
            if (uiData.StoreUiData.Positions.Length != View.LayoutContainer.LayoutGroups.Length)
            {
                uiData.StoreUiData = new UiData.LayoutContainerData(View.LayoutContainer.LayoutGroups.Length);
                _uiData.Save(uiData);
            }
            
            View.RebuildUiData(uiData.StoreUiData);
            
        }
        
        public void InitValues()
        {
            var data = _commonPlayerData.GetData();
            
            View.InitCurrencies(ref data.Money, _currenciesData);
            //для 2 и 3го key дописать логику ( рецепт закрыт по уровню и т.д) 
            View.LayoutContainer.Init(_recipeData.Get(), data, OnButton,
                key => key.StartsWith("cube_locked_") || key.StartsWith("background_locked_"),
                null, 
                key => data.CurrentCubeSkinKey == key || data.CurrentBackgroundSkinKey == key);
            
            void OnButton(UiGeneratedEntity entity, UiGeneratedLayout layout)
            {
                Recipe recipe = _recipeData.Get().Get(entity.Key);
                _world.CreateRequestRecipe(recipe);
                //тут клон передавать
                //layout - вся страничка
            }

            void SelectImpact(UiGeneratedEntity entity, HapticPatterns.PresetType type, float duration)
            {
                HapticPatterns.PlayPreset(type);
                entity.CurrentStateObjective.SetSelected(true);
                _delayService.Do(duration, () => entity.CurrentStateObjective.SetSelected(false));
            }

            // void SpawnParticle(UiLayoutObjective objective)
            // {
            //     var go = _uiSpawnService.Spawn("uiStars");
            //     go.transform.SetParent(objective.transform);
            // }
        }

        public override void OnAfterShow()
        {
            var data = _commonPlayerData.GetData();
            View.InitCurrencies(ref data.Money, _currenciesData);
            View.LayoutContainer.UpdatePurchaseStates(_skinsData.Get(), ref _commonPlayerData.GetData().Money.CurrencyValues);
            
            _pauseService.PauseGame(true);
            View.BackBtn.gameObject.SetActive(true);
            View.UiBox.gameObject.SetActive(true);
            View.Background.DoAppearColor(_fadeDuration).SetEase(Ease.InQuart);
            View.UiBox.DoFromPosition(_windowHidePos, _appearDuration).SetDelay(_fadeDuration).SetEase(Ease.OutCubic);
            _delayService.Do(_appearDuration, () => EnableInput(true));
        }

        public override void OnBeforeHide()
        {
            View.Background.DoDisappearColor(_appearDuration, () => View.BackBtn.gameObject.SetActive(false))
                .SetEase(Ease.OutQuart);
            SaveUiData();
        }
        
        public override void EnableInput(bool value)
        {
            View.BackBtn.interactable = value;
        }

        private void OnBack()
        {
            EnableInput(false);
            View.UiBox.DoToPosition(_windowHidePos, _appearDuration, () =>
            {
                View.UiBox.gameObject.SetActive(false);
                _signalBus.OpenWindow<GameHudWindow>();
                _pauseService.PauseGame(false);
            }).SetEase(Ease.InCubic);
        }

        private void SaveUiData()
        {
            var uiData = _uiData.GetData();
            uiData.StoreUiData.Set(View.LayoutContainer);
            _uiData.Save(uiData);
        }
    }
}