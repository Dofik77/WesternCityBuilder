using System;
using DG.Tweening;
using ECS.Game.Components.General;
using ECS.Utils.Extensions;
using Leopotam.Ecs;
using Runtime.Data;
using Runtime.Data.PlayerData.Currency;
using Runtime.Data.PlayerData.Levels;
using Runtime.Game.Ui.Extensions;
using Runtime.Game.Ui.Objects.General;
using Runtime.Game.Ui.Objects.UiObjectives;
using Runtime.Services.AnalyticsService;
using Runtime.Services.AnalyticsService.Impls;
using Runtime.Services.CommonPlayerData;
using Runtime.Services.CommonPlayerData.Data;
using Runtime.Services.DelayService;
using Runtime.Services.MonetizationService;
using Runtime.Services.SceneLoading;
using Runtime.Signals;
using SimpleUi.Abstracts;
using UniRx;
using UnityEngine;
using Utils;
using Zenject;
using Object = UnityEngine.Object;

namespace Runtime.Game.Ui.Windows.LevelComplete
{
    public class LevelCompleteController : UiController<LevelCompleteView>, IInitializable
    {
        [Inject] private IAnalyticsService _analyticsService;
        [Inject] private readonly ICommonPlayerDataService<CommonPlayerData> _commonPlayerData;
        [Inject] private readonly SignalBus _signalBus;
        [Inject] private readonly ISceneLoadingManager _sceneLoadingManager;
        [Inject] private readonly IDelayService _delayService;
        [Inject] private readonly ILevelsData _levelsData;
        [Inject] private readonly ICurrenciesData _currenciesData;
        [Inject] private readonly IMonetizationService _monetizationService;

        private EcsWorld _world;

        private const float _fadeDuration = 0.65f;
        private const float _uiAppearDuration = 1.15f;
        private const float _objectivesDuration = 0.3f;
        private const float _betweenMoveDuration = 0.2f;
        private const float _iconAppearDuration = 0.3f;
        private const float _beforeAppearDuration = 0.4f;
        private const float _barRepaintDuration = 0.3f;
        private const string _vip_mask = "vip_";

        private ECurrency _rewardCurrency;
        private CurrencyBox _currentBox;
        private int _firstObjectiveReward;
        private int _secondObjectiveReward;
        private int _thirdObjectiveReward;
        private int _totalReward;
        private int _currentReward;
        private int _currentTotalReward;
        private int _timer;
        private int _totalTime;
        private bool _isVip;
        private bool _isFirstObjectiveComplete;
        private bool _isSecondObjectiveComplete;
        private bool _isThirdObjectiveComplete;
        private UiBinaryObjective CurrentFirstObjective;
        private UiBinaryObjective CurrentSecondObjective;
        private UiBinaryObjective CurrentThirdObjective;
        private Level _levelData;

        public LevelCompleteController(EcsWorld world) => _world = world;

        public void Initialize()
        {
            View.NextLevel.OnClickAsObservable().Subscribe(x => OnNextLevel()).AddTo(View.NextLevel);
            _signalBus.GetStream<SignalTimerUpdate>().Subscribe(x => _timer = x.Value.ToInt()).AddTo(View);
        }

        private void OnNextLevel()
        {
            _monetizationService.ShowInterstitial("level_complete");
            _sceneLoadingManager.LoadScene(_commonPlayerData.GetData().CurrentLevel);
        }

        public override void OnAfterShow()
        {
            var data = _commonPlayerData.GetData();
            // rewards must be inited before saved
            InitValues(ref data);
            SaveData(ref data);
            InitUiAppear();
            InitTweens(data);
            
            // _analyticsService.SendProgressionEvent(GAProgressionStatus.Complete, _levelData.GetLowerKey());
        }

        public void InitValues(ref CommonPlayerData data)
        {
            ref var levelDataComponent = ref _world.GetEntity<LevelComponent>().Get<LevelDataComponent>();
            _levelData = _levelsData.Get().Get(data.CurrentLevel);
            var property = data.GetCurrentLevel();

            _rewardCurrency = _levelData.GetRewardCurrency();
            _currentBox = _rewardCurrency == ECurrency.Soft ? View.SoftCurrency : View.HardCurrency;

            _firstObjectiveReward = _levelData.GetFirstObjectiveReward();
            _secondObjectiveReward = _levelData.GetSecondObjectiveReward();
            _thirdObjectiveReward = _levelData.GetThirdObjectiveReward();

            _isVip = data.CurrentLevel.StartsWith(_vip_mask);
            if (_isVip)
            {
                _isFirstObjectiveComplete = _timer < _levelData.GetFirstObjective();
                _isSecondObjectiveComplete = _timer < _levelData.GetSecondObjective();
                _isThirdObjectiveComplete = _timer < _levelData.GetThirdObjective();

                CurrentFirstObjective = View.FirstObjective;
                CurrentSecondObjective = View.SecondObjective;
                CurrentThirdObjective = View.ThirdObjective;

                var text = "Less then ";
                var valueDone = "Done";
                var valueFail = "Fail";

                CurrentFirstObjective.Text.text = text + TimeFormat(_levelData.GetFirstObjective());
                CurrentSecondObjective.Text.text = text + TimeFormat(_levelData.GetSecondObjective());
                CurrentThirdObjective.Text.text = text + TimeFormat(_levelData.GetThirdObjective());

                CurrentFirstObjective.Value.text = _isFirstObjectiveComplete ? valueDone : valueFail;
                CurrentSecondObjective.Value.text = _isSecondObjectiveComplete ? valueDone : valueFail;
                CurrentThirdObjective.Value.text = _isThirdObjectiveComplete ? valueDone : valueFail;
                
                _currentTotalReward =
                    (property.IsFirstObjectiveComplete ? 0 : !_isFirstObjectiveComplete ? 0 : _firstObjectiveReward)
                    + (property.IsSecondObjectiveComplete ? 0 : !_isSecondObjectiveComplete ? 0 : _secondObjectiveReward)
                    + (property.IsThirdObjectiveComplete ? 0 : !_isThirdObjectiveComplete ? 0 : _thirdObjectiveReward);
            }
            else
            {
                _isFirstObjectiveComplete = true;
                _isSecondObjectiveComplete = levelDataComponent.UseHint <= 0;
                _isThirdObjectiveComplete = levelDataComponent.UseRollback <= 0;

                CurrentFirstObjective = View.LevelComplete;
                CurrentSecondObjective = View.NoHint;
                CurrentThirdObjective = View.NoRollback;

                CurrentFirstObjective.Value.text = "+" + _firstObjectiveReward;
                CurrentSecondObjective.Value.text = "+" + _secondObjectiveReward;
                CurrentThirdObjective.Value.text = "+" + _thirdObjectiveReward;

                _currentTotalReward = 
                    (property.IsCompleted ? 0 : _firstObjectiveReward) 
                    + (property.IsCompleted || levelDataComponent.UseHint > 0 ? 0 : _secondObjectiveReward)
                    + (property.IsCompleted || levelDataComponent.UseRollback > 0 ? 0 : _thirdObjectiveReward);
            }

            CurrentFirstObjective.gameObject.SetActive(true);
            CurrentSecondObjective.gameObject.SetActive(true);
            CurrentThirdObjective.gameObject.SetActive(true);

            _totalReward = _firstObjectiveReward + _secondObjectiveReward + _thirdObjectiveReward;

            View.LevelN.text = _levelsData.Get().Get(_commonPlayerData.GetData().CurrentLevel).GetName();
            UpdateScore();

            View.LevelComplete.Value.text = "+" + _firstObjectiveReward;
            View.NoHint.Value.text = "+" + _secondObjectiveReward;
            View.NoRollback.Value.text = "+" + _thirdObjectiveReward;
            View.Time.Value.text = TimeFormat(_timer);

            _totalTime = property.TotalTime > 0 && _timer > property.TotalTime
                ? property.TotalTime
                : _timer;

            View.InitCurrencies(ref data.Money, _currenciesData);
            View.Reward.Icon.sprite = _currentBox.Icon.sprite;
            View.Reward.Value.text = "+" + _currentTotalReward;

            string TimeFormat(float value) => TimeSpan.FromSeconds(value).ToString(@"mm\:ss");
        }

        public void SaveData(ref CommonPlayerData data)
        {
            var levelData = _levelsData.Get().Get(data.CurrentLevel);
            var currentLevel = data.Levels.Properties.Get(data.CurrentLevel);
            data.SetCurrentLevelComplete(_totalTime, _isFirstObjectiveComplete,
                currentLevel.IsSecondObjectiveComplete || _isSecondObjectiveComplete,
                currentLevel.IsThirdObjectiveComplete || _isThirdObjectiveComplete);
            if (data.CurrentLevel.StartsWith(_vip_mask))
                data.SetPreviousLevel();
            else
                data.SetNextLevel(levelData.GetNextLevelKey());
            data.Money.Add(_rewardCurrency, _currentTotalReward);
            _commonPlayerData.Save(data);
            
            _analyticsService.SendDesignEvent(AnalyticsState.Get, Enum.GetName(typeof(ECurrency), _rewardCurrency), _currentTotalReward.ToString());
        }

        public void InitUiAppear()
        {
            View.Back.DoAppearColor(_fadeDuration).SetEase(Ease.InQuart);
            View.Top.Init(new Vector2(0, 650f),_uiAppearDuration);
            View.Center.Init(new Vector2(1500f, 0),_uiAppearDuration);
            View.Bottom.Init(new Vector2(0, -800f),_uiAppearDuration);
            View.Top.DoToDefault().SetDelay(_fadeDuration).SetEase(Ease.OutQuart);
            View.Center.DoToDefault().SetDelay(_fadeDuration).SetEase(Ease.OutQuart);
            View.Bottom.DoToDefault().SetDelay(_fadeDuration).SetEase(Ease.OutQuart);
        }

        public void InitTweens(CommonPlayerData data)
        {
            var scorePos = View.ScoreBar.RectTransform.anchoredPosition;

            ObjectiveAppear(CurrentFirstObjective, 1)
                .OnComplete(() =>
                    CurrentFirstObjective.SetComplete(_isFirstObjectiveComplete)
                        .DoAppear(_iconAppearDuration).SetDelay(_beforeAppearDuration).SetEase(Ease.InQuad)
                        .OnComplete(() =>
                        {
                            if (!_isFirstObjectiveComplete)
                                return;
                            _currentReward += _firstObjectiveReward;
                            UpdateScore();
                        }));

            ObjectiveAppear(CurrentSecondObjective, 2)
                .OnComplete(() =>
                    CurrentSecondObjective.SetComplete(_isSecondObjectiveComplete)
                        .DoAppear(_iconAppearDuration).SetDelay(_beforeAppearDuration).SetEase(Ease.InQuad)
                        .OnComplete(() =>
                        {
                            if (!_isSecondObjectiveComplete)
                                return;
                            _currentReward += _secondObjectiveReward;
                            UpdateScore();
                        }));

            ObjectiveAppear(CurrentThirdObjective, 3)
                .OnComplete(() =>
                    CurrentThirdObjective.SetComplete(_isThirdObjectiveComplete)
                        .DoAppear(_iconAppearDuration).SetDelay(_beforeAppearDuration).SetEase(Ease.InQuad)
                        .OnComplete(() =>
                        {
                            if (!_isThirdObjectiveComplete)
                                return;
                            _currentReward += _thirdObjectiveReward;
                            UpdateScore();
                        }));

            ObjectiveAppear(View.Time, 4);
            ObjectiveAppear(View.Reward, 5);

            _delayService.Do(GetTweenDelay(6), () =>
            {
                var currencyRectTrans = _currentBox.Icon.rectTransform;
                var rewardRectTrans = View.Reward.Icon.rectTransform;
                var newIcon = Object.Instantiate(currencyRectTrans,
                    rewardRectTrans.TransformPoint(rewardRectTrans.rect.center), Quaternion.identity, View.transform);
                newIcon.DOMove(currencyRectTrans.TransformPoint(currencyRectTrans.rect.center), 0.5f)
                    .SetEase(Ease.Linear).OnComplete(() =>
                    {
                        _currentBox.Value.text = data.Money.Get(_rewardCurrency).ToString();
                        newIcon.gameObject.SetActive(false);
                    });
            });

            Tweener ObjectiveAppear(CustomUiObject rectTransform, int id)
            {
                rectTransform.Init(scorePos, _objectivesDuration * id);
                return rectTransform.DoToDefault().SetDelay(GetTweenDelay(id)).SetEase(Ease.InCubic);
            }

            float GetTweenDelay(int order) => _fadeDuration + _uiAppearDuration + _betweenMoveDuration * order;
        }

        private void UpdateScore()
        {
            View.ScoreBar.Text.text = $"Reward: {_currentReward} of {_totalReward}";
            View.ScoreBar.Repaint(_totalReward == 0 ? 0 : _currentReward / _totalReward, _barRepaintDuration);
        }
    }
}