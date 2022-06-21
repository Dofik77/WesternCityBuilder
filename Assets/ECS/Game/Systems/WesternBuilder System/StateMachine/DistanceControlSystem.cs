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
        private readonly EcsFilter<LinkComponent, EventControlDistanceToSetState> _event;

        private UnitView _unitView;
        private LinkableView _destanationView;
        private IHasStopDistance _iDestanationView;

        private Vector3 _destanationPosition;
        private float _stopDistance;
        public void Run()
        {
            foreach (var i in _event)
            {
                _unitView = _event.Get1(i).View as UnitView;
                
                _destanationView = _event.Get2(i).DistanceObjectView;
                _iDestanationView = _destanationView as IHasStopDistance;
                
                _destanationPosition = _destanationView.transform.position;
                _stopDistance = _iDestanationView.GetStopDistance();
                
                if (Vector3.Distance(_unitView.transform.position, _destanationPosition) <= _stopDistance)
                {
                    //_unitView.GetAgent().isStopped = true;
                    _unitView.transform.LookAt(_destanationView.transform);
                    _unitView.Entity.Get<EventUnitChangeStateComponent>().State = _event.Get2(i).FeatureState;
                    _unitView.Entity.Get<EventSetAnimationComponent>().Value = 0;

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