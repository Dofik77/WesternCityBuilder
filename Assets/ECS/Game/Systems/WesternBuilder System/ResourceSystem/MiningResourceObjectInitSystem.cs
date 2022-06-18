using ECS.Core.Utils.ReactiveSystem;
using ECS.Core.Utils.ReactiveSystem.Components;
using ECS.Game.Components.General;
using ECS.Game.Components.WesternBuilder_Component;
using ECS.Views;
using Leopotam.Ecs;
using UnityEngine;

namespace ECS.Game.Systems.WesternBuilder_System.ResourceSystem
{
    public class MiningResourceObjectInitSystem : ReactiveSystem<EventAddComponent<ObjectMiningComponent>>
    {
        protected override EcsFilter<EventAddComponent<ObjectMiningComponent>> ReactiveFilter { get; }

        private ObjectMiningView _objectMiningView;
        
        protected override void Execute(EcsEntity entity)
        {
            _objectMiningView = entity.Get<LinkComponent>().View as ObjectMiningView;
            entity.Get<RemainingAmountResource>().Value = _objectMiningView.GetCurrentResourceValue;
            entity.Get<ObjectMiningComponent>().MaxResourceValue = _objectMiningView.GetCurrentResourceValue;

            // switch (_objectMiningView.ResourceType)
            // {
            //     case RequiredResourceType.WoodResource :
            //         entity.Get<TreeObjectComponent>();
            //         break;
            //     
            //     case RequiredResourceType.RockResource :
            //         entity.Get<RockObjectComponent>();
            //         break;
            // }
        }
    }
}