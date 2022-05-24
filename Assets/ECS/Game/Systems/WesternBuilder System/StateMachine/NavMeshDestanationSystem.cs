using ECS.Core.Utils.SystemInterfaces;
using ECS.Game.Components.General;
using ECS.Game.Components.WesternBuilder_Component;
using ECS.Game.Systems.WesternBuilder_System;
using ECS.Views.General;
using ECS.Views.WesterBuilderView.Interfaces;
using Leopotam.Ecs;
using Runtime.Game.Utils.MonoBehUtils;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace ECS.Game.Systems.General.NavMesh
{
    public class NavMeshDestanationSystem : IEcsUpdateSystem
    {
        [Inject] private ScreenVariables _screenVariables;
        
        private readonly EcsFilter<LinkComponent, EventSetDestinationComponent> _views;
        
        private UnitView _unitView;
        private LinkableView _destanationView;
        private IHasStopDistance _iDestanationView;
        private float _stopDistanceOffset;
        
        public void Run()
        {
            foreach (var i in _views)
            {
                _unitView = _views.Get1(i).View as UnitView;
                _destanationView = _views.Get2(i).DistanceControlObjectView;
                _iDestanationView = _destanationView as IHasStopDistance;

                _unitView.GetAgent().SetDestination(_destanationView.transform.position);
                _unitView.GetAgent().stoppingDistance = _iDestanationView.GetStopDistance();
                
                
                _views.GetEntity(i).Del<EventSetDestinationComponent>();
                
                _unitView.Entity.Get<EventSetAnimationComponent>().Value = 1;
                _unitView.Entity.Get<EventSetAnimationComponent>().StageOfAnim = "Stage"; 
            }
        }
    }
    
    public struct EventSetDestinationComponent
    {
        public LinkableView DistanceControlObjectView;
    }
}