using System.Collections.Generic;
using DG.Tweening;
using ECS.Core.Utils.ReactiveSystem;
using ECS.Core.Utils.ReactiveSystem.Components;
using ECS.Game.Components.Flags;
using ECS.Game.Components.General;
using ECS.Game.Components.WesternBuilder_Component;
using ECS.Game.Systems.General.NavMesh;
using ECS.Game.Systems.WesternBuilder_System.ResourceSystem;
using ECS.Game.Systems.WesternBuilder_System.StateMachine;
using ECS.Views;
using ECS.Views.General;
using ECS.Views.WesterBuilderView;
using Leopotam.Ecs;
using Runtime.Data.PlayerData.Recipe;
using SimpleUi.Signals;
using UnityEngine;
using UnityEngine.ProBuilder;
using Zenject;

namespace ECS.Game.Systems.WesternBuilder_System
{
    public class FindClosestResourceSystem : ReactiveSystem<EventFindClosestResourceComponent>
    {
        private readonly EcsFilter<UnitComponent, LinkComponent> _units;
        private readonly EcsFilter<UnitsSkillScoreComponent> _unitSkills;
        
        private readonly EcsFilter<ObjectMiningComponent, LinkComponent>.Exclude<DisableMiningObject> _miningObjects;
        private readonly EcsFilter<WoodLogComponent, LinkComponent> _woodLogResource;
        private readonly EcsFilter<RockComponent, LinkComponent> _rockResource;
        
        private float ClosestResourceMiningPosition = Mathf.Infinity;
        private float ClosestResourcePosition = Mathf.Infinity;
        
        private ObjectMiningView _closestMiningView;
        private ResourceView _closestResourceView;
            
        private UnitView _unitView;
        protected override EcsFilter<EventFindClosestResourceComponent> ReactiveFilter { get; }

        protected override void Execute(EcsEntity entity)
        {
            _unitView = entity.Get<LinkComponent>().View as UnitView;
            var requiredResourceType = entity.Get<UnitPriorityData>().RequiredMining;

            if (CheckResourceOnGround(requiredResourceType))
            {
                
            }
           
            
            foreach (var i in _miningObjects)
            {
                var objectMiningView = _miningObjects.Get2(i).View as ObjectMiningView;

                if (objectMiningView.Entity.Get<RemainingAmountResource>().Value != 0)
                {
                    if (requiredResourceType == objectMiningView.ResourceType)
                    {
                        Vector3 dif = _unitView.transform.position - objectMiningView.transform.position;
                        float curDistance = dif.magnitude;
                    
                        if (curDistance < ClosestResourceMiningPosition)
                        {
                            _closestMiningView = objectMiningView;
                            ClosestResourceMiningPosition = curDistance;
                        }
                    }
                }
            }
            
            var requiredMining =
                _closestMiningView.Entity.Get<RemainingAmountResource>().Value <= _unitView.Entity.Get<UnitPriorityData>().RequiredValueResource
                    ? _closestMiningView.Entity.Get<RemainingAmountResource>().Value
                    : _unitView.Entity.Get<UnitPriorityData>().RequiredValueResource;

            _closestMiningView.Entity.Get<RemainingAmountResource>().Value -= requiredMining;
            
            entity.Get<NextMiningValue>().Value = requiredMining;
            
            entity.Get<FollowAndSetStateComponent>().FeatureState = UnitAction.TakeResource;
            entity.Get<FollowAndSetStateComponent>().SetDistanceView = _closestMiningView;
            entity.Get<FollowAndSetStateComponent>().ControlDistanceView = _closestMiningView;

            entity.Get<EventUnitChangeStateComponent>().State = UnitAction.FollowAndSetState;
            entity.Get<CurrentMiningObjectData>().CurrentMiningObject = _closestMiningView;

            ClosestResourceMiningPosition = Mathf.Infinity;
        }

        public bool CheckResourceOnGround(RequiredResourceType requiredResourceType)
        {
            bool resourceExist = false;

            switch (requiredResourceType)
            {
                case RequiredResourceType.WoodResource :
                    if (!_woodLogResource.IsEmpty())
                    {
                        foreach (var i in _woodLogResource)
                        {
                            var WoodResourceView = _woodLogResource.Get2(i).View as ResourceView;
                            Vector3 dif = _unitView.transform.position - WoodResourceView.transform.position;
                            float curDistance = dif.magnitude;
                            
                            if (curDistance < ClosestResourcePosition)
                            {
                                _closestResourceView = WoodResourceView;
                                ClosestResourceMiningPosition = curDistance;
                            }
                        }
                        resourceExist = true;
                    }
                    break;
                
                case RequiredResourceType.RockResource :
                    if (!_woodLogResource.IsEmpty())
                    {
                        foreach (var i in _woodLogResource)
                        {
                            var RockResourceView = _woodLogResource.Get2(i).View as ResourceView;
                            Vector3 dif = _unitView.transform.position - RockResourceView.transform.position;
                            float curDistance = dif.magnitude;
                            
                            if (curDistance < ClosestResourcePosition)
                            {
                                _closestResourceView = RockResourceView;
                                ClosestResourceMiningPosition = curDistance;
                            }
                        }
                        resourceExist = true;
                    }
                    break;
            }
            
            
            
            return resourceExist;
        }
        
    }
    
    public struct EventFindClosestResourceComponent
    {
        
    }
}