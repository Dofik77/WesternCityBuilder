using System.Diagnostics.CodeAnalysis;
using ECS.Core.Utils.ReactiveSystem;
using ECS.Game.Components.Flags;
using ECS.Game.Components.General;
using ECS.Utils.Extensions;
using ECS.Views.General;
using Leopotam.Ecs;
using Runtime.Data.Base.Game;
using Runtime.Data.PlayerData.Levels;
using Runtime.Data.PlayerData.Skins;
using Runtime.Game.Ui.Windows;
using Runtime.Game.Utils.MonoBehUtils;
using Runtime.Services.AnalyticsService;
using Runtime.Services.CommonPlayerData;
using Runtime.Services.CommonPlayerData.Data;
using SimpleUi.Signals;
using UniRx;
using Zenject;

namespace ECS.Game.Systems.General.Level
{
    [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
    public class LevelStartSystem : ReactiveSystem<EventLevelStartComponent>
    {
        [Inject] private IAnalyticsService _analyticsService;
        [Inject] private SignalBus _signalBus;
        [Inject] private ScreenVariables _screenVariables;
        [Inject] private ISkinsData _skinsData;
        [Inject] private ILevelsData _levelsData;
        [Inject] private ICommonPlayerDataService<CommonPlayerData> _commonPlayerData;
        private const string PLAYER_START = "PlayerStart";
        private const string PLAYER_FINISH = "PlayerFinish";
        private const string VIP_MASK = "vip_";

        private readonly EcsWorld _world;
        private readonly EcsFilter<CameraComponent, LinkComponent> _cameraF;
        private readonly EcsFilter<PlayerComponent, LinkComponent> _player;
        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        private bool started = false;
        private CameraView _cameraView;

        // ReSharper disable once UnassignedGetOnlyAutoProperty
        protected override EcsFilter<EventLevelStartComponent> ReactiveFilter { get; }
        protected override bool DeleteEvent => true;

        protected override void Execute(EcsEntity entity)
        {
            if (started)
                return;

            started = true;
            _world.SetStage(EGameStage.Play);
            _signalBus.OpenWindow<GameHudWindow>();
            

            InitCameraTween();

            foreach (var i in _player)
            {
            }
            
            
        }
        
        
        private void InitCameraTween()
        {
        }
    }
    
    public struct EventLevelStartComponent : IEcsIgnoreInFilter
    {
    }
}