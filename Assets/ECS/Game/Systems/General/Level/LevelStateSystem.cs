using ECS.Core.Utils.ReactiveSystem;
using ECS.Utils.Extensions;
using Leopotam.Ecs;
using Runtime.Data.Base.Game;
using Runtime.Game.Utils.MonoBehUtils;
using Runtime.Services.VibrationService;
using UnityEngine;
using Zenject;

namespace ECS.Game.Systems.General.Level
{
    public class LevelStateSystem : ReactiveSystem<EventChangeLevelStateComponent>
    {
        [Inject] private SignalBus _signalBus;
        [Inject] private readonly ScreenVariables _screenVariables;
        [Inject] private readonly IVibrationService _vibrationService;

        private readonly EcsWorld _world;

     
        protected override EcsFilter<EventChangeLevelStateComponent> ReactiveFilter { get; }
        protected override bool DeleteEvent => true;

        private Transform _cacheLetter;

        protected override void Execute(EcsEntity entity)
        {
            entity.Get<LevelStateComponent>().State = entity.Get<EventChangeLevelStateComponent>().State;

            switch (entity.Get<LevelStateComponent>().State)
            {
                case ELevelState.End:
                    _world.SetStage(EGameStage.Complete);
                    break;
            }
        }
    }

    public struct EventChangeLevelStateComponent
    {
        public ELevelState State;
    }

    public struct LevelStateComponent
    {
        public ELevelState State;
    }

    public enum ELevelState
    {
        Start,
        End
    }
}