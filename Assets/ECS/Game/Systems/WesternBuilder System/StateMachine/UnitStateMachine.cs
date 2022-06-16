using ECS.Core.Utils.ReactiveSystem;
using ECS.Core.Utils.ReactiveSystem.Components;
using ECS.Game.Components.General;
using ECS.Game.Components.WesternBuilder_Component;
using ECS.Game.Components.WesternBuilder_Component.MiningComponents;
using ECS.Game.Systems.General.NavMesh;
using ECS.Game.Systems.WesternBuilder_System.BuildsSystems;
using ECS.Game.Systems.WesternBuilder_System.StateMachine;
using ECS.Utils.Extensions;
using ECS.Views;
using Leopotam.Ecs;
using Runtime.Game.Utils.MonoBehUtils;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace ECS.Game.Systems.WesternBuilder_System.Stats.StateMachine
{
    public class UnitStateMachine : ReactiveSystem<EventUnitChangeStateComponent>
    {
        [Inject] private ScreenVariables _screenVariables;
        protected override EcsFilter<EventUnitChangeStateComponent> ReactiveFilter { get; }
        private readonly EcsFilter<BuildCampFireComponent, LinkComponent> _campFire;

        private BuildsView _campfire;
        
        protected override void Execute(EcsEntity entity)
        {
            foreach (var i in _campFire)
                _campfire = _campFire.Get2(i).View as BuildsView;
            
            switch (entity.Get<EventUnitChangeStateComponent>().State)
            {
                case UnitAction.AwaitNearCampFire :
                    entity.Get<EventSetDestinationComponent>().DistanceControlObjectView = _campfire;
                    entity.Get<EventControlDistanceToSetState>().DistanceObjectView = _campfire;
                    Debug.Log("AwaitNearCampFire");
                    break;
                
                case UnitAction.FetchResource :
                    entity.Get<EventFindClosestResourceComponent>();
                    Debug.Log("FindClosestObject");
                    break;
                
                case UnitAction.TakeResource :
                    entity.Get<RequestResourceComponent>();
                    Debug.Log("TakeResource");
                    break;
                
                case UnitAction.PutResource : 
                    Debug.Log("PutResource");
                    entity.Get<EventBuildUpdate>();
                    break;
             
                case UnitAction.FollowAndSetState :
                    Debug.Log("FollowAndSetState");
                    entity.Get<EventControlDistanceToSetState>().FeatureState 
                        = entity.Get<FollowAndSetStateComponent>().FeatureState;
                    entity.Get<EventControlDistanceToSetState>().DistanceObjectView 
                        = entity.Get<FollowAndSetStateComponent>().ControlDistanceView;
                    entity.Get<EventSetDestinationComponent>().DistanceControlObjectView 
                        = entity.Get<FollowAndSetStateComponent>().SetDistanceView;
                    break;
                
                case UnitAction.AwaitNearСonstruction :
                    Debug.Log("AwaitNearСonstruction");
                    break;
                
                case UnitAction.Сonstruction :
                    Debug.Log("Construction");
                    break;
            }
        }
    }
}