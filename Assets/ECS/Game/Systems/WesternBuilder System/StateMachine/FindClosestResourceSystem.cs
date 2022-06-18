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
using Zenject.ReflectionBaking.Mono.Cecil;

namespace ECS.Game.Systems.WesternBuilder_System
{
    public class FindClosestResourceSystem : ReactiveSystem<EventFindClosestResourceComponent>
    {
        private readonly EcsFilter<UnitComponent, LinkComponent> _units;
        private readonly EcsFilter<UnitsSkillScoreComponent> _unitSkills;
        
        private readonly EcsFilter<ObjectMiningComponent, LinkComponent>.Exclude<DisableMiningObject> _miningObjects;
        private readonly EcsFilter<WoodResourceComponent, LinkComponent>.Exclude<ResourceObjectUnavailable> _woodLogResource;
        private readonly EcsFilter<RockResourceComponent, LinkComponent>.Exclude<ResourceObjectUnavailable> _rockResource;
        
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
                DelegateTask(entity, _closestResourceView, 1);
            else
                FindClosestMiningObject(entity, requiredResourceType);
        }
        
        private bool CheckResourceOnGround(RequiredResourceType requiredResourceType)
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
                                ClosestResourcePosition = curDistance;
                            }
                        }
                        resourceExist = true;
                    }
                    break;
                
                case RequiredResourceType.RockResource :
                    if (!_rockResource.IsEmpty())
                    {
                        foreach (var i in _woodLogResource)
                        {
                            var RockResourceView = _rockResource.Get2(i).View as ResourceView;
                            Vector3 dif = _unitView.transform.position - RockResourceView.transform.position;
                            float curDistance = dif.magnitude;
                            
                            if (curDistance < ClosestResourcePosition)
                            {
                                _closestResourceView = RockResourceView;
                                ClosestResourcePosition = curDistance;
                            }
                        }
                        resourceExist = true;
                    }
                    break;
            }
            
            ClosestResourcePosition = Mathf.Infinity;
            
            if(resourceExist)
                _closestResourceView.Entity.Get<ResourceObjectUnavailable>();
            
            return resourceExist;
        }
        private void FindClosestMiningObject(EcsEntity entity, RequiredResourceType requiredResourceType)
        {
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
            
            DelegateTask(entity, _closestMiningView, requiredMining);
            
            ClosestResourceMiningPosition = Mathf.Infinity;
        }
        private void DelegateTask(EcsEntity entity, LinkableView targetView, int NextMiningValue)
        {
            entity.Get<NextMiningValue>().Value = NextMiningValue;
                
            entity.Get<FollowAndSetStateComponent>().FeatureState = UnitAction.TakeResource;
            entity.Get<FollowAndSetStateComponent>().SetDistanceView = targetView;
            entity.Get<FollowAndSetStateComponent>().ControlDistanceView = targetView;
                
            entity.Get<EventUnitChangeStateComponent>().State = UnitAction.FollowAndSetState;
            entity.Get<CurrentMiningObjectData>().CurrentMiningObject = targetView;

            ClosestResourcePosition = Mathf.Infinity;
        }
    }
    
    public struct EventFindClosestResourceComponent
    {
        
    }

    public struct ResourceObjectUnavailable
    {
        
    }
}