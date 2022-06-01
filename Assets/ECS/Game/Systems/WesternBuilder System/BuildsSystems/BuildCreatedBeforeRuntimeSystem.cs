using ECS.Core.Utils.ReactiveSystem;
using ECS.Core.Utils.ReactiveSystem.Components;
using ECS.Game.Components.Flags;
using ECS.Game.Components.General;
using ECS.Game.Components.WesternBuilder_Component;
using ECS.Views;
using Leopotam.Ecs;
using Runtime.Game.Utils.MonoBehUtils;
using Zenject;

namespace ECS.Game.Systems.WesternBuilder_System.StateMachine
{
    public class BuildCreatedBeforeRuntimeSystem : ReactiveSystem<EventAddComponent<BuildCampFireComponent>>
    {
        private readonly EcsFilter<UnitComponent, LinkComponent> _units;
        protected override EcsFilter<EventAddComponent<BuildCampFireComponent>> ReactiveFilter { get; }
        protected override void Execute(EcsEntity entity)
        {
            foreach (var i in _units)
            {
                //add check on campfire view 
                _units.GetEntity(i).Get<EventUpdatePriorityComponent>();
            }
        }
    }
}