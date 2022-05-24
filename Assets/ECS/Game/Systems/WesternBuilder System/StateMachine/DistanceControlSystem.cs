using ECS.Core.Utils.ReactiveSystem;
using ECS.Core.Utils.SystemInterfaces;
using ECS.Game.Components.General;
using ECS.Game.Components.WesternBuilder_Component;
using ECS.Views;
using ECS.Views.General;
using ECS.Views.WesterBuilderView.Interfaces;
using Leopotam.Ecs;
using UnityEngine;

namespace ECS.Game.Systems.WesternBuilder_System.StateMachine
{
    public class DistanceControlSystem : IEcsUpdateSystem
    {
        private readonly EcsFilter<LinkComponent, EventControlDistanceToSetState> _distanceControlToUpdateState;

        private UnitView _unitView;
        private LinkableView _destanationView;
        private IHasStopDistance _iDestanationView;

        private Vector3 _distanceObjectPosition;
        private float _stopDistance;
        public void Run()
        {
            foreach (var i in _distanceControlToUpdateState)
            {
                _unitView = _distanceControlToUpdateState.Get1(i).View as UnitView;
                
                _destanationView = _distanceControlToUpdateState.Get2(i).DistanceObjectView;
                _iDestanationView= _destanationView as IHasStopDistance;
                
                _distanceObjectPosition = _destanationView.transform.position;
                _stopDistance = _iDestanationView.GetStopDistance();
                
                if (Vector3.Distance(_unitView.transform.position, _distanceObjectPosition) < _stopDistance) 
                {
                    _unitView.Entity.Get<EventUnitChangeStateComponent>().State = _distanceControlToUpdateState.Get2(i).FeatureState;
                    _unitView.Entity.Get<EventSetAnimationComponent>().Value = 0;
                    _unitView.Entity.Get<EventSetAnimationComponent>().StageOfAnim = "Stage";
                    _unitView.Entity.Del<EventControlDistanceToSetState>();
                }
            }
        }
    }
    
    public struct EventControlDistanceToSetState
    {
        public LinkableView DistanceObjectView;
        public UnitAction FeatureState;
    }
}