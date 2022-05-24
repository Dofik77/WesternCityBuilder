using System.Diagnostics.CodeAnalysis;
using ECS.Core.Utils.SystemInterfaces;
using ECS.Game.Components.Flags;
using ECS.Game.Components.General;
using ECS.Game.Systems.General.Level;
using ECS.Views.General;
using Leopotam.Ecs;

namespace ECS.Game.Systems.General
{
    public class TriggerActivateSystem : IEcsUpdateSystem
    {
        private readonly EcsFilter<EventTriggerActivateComponent, LinkComponent> _events;
        private readonly EcsFilter<PlayerComponent, LinkComponent> _player;
        private readonly EcsFilter<LevelStateComponent> _levelState;

        private EcsEntity _eventEntity;
        private PlayerView _playerView;
        private bool cond = false;

        [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
        public void Run()
        {
            
        }
    }
    
    public struct EventTriggerActivateComponent : IEcsIgnoreInFilter
    {
    }
    public struct ActivatedComponent : IEcsIgnoreInFilter
    {
    }
}