using System;
using DG.Tweening;
using ECS.Core.Utils.ReactiveSystem;
using ECS.Game.Components.Flags;
using ECS.Game.Components.General;
using ECS.Game.Components.WesternBuilder_Component;
using ECS.Game.Systems.General;
using ECS.Game.Systems.General.NavMesh;
using ECS.Game.Systems.WesternBuilder_System.ResourceSystem;
using ECS.Game.Systems.WesternBuilder_System.Stats;
using ECS.Utils.Extensions;
using ECS.Views;
using ECS.Views.General;
using ECS.Views.WesterBuilderView;
using Leopotam.Ecs;
using Runtime.Data;
using Runtime.Services.DelayService;
using Runtime.Services.DelayService.Impls;
using Runtime.Signals;
using UnityEngine;
using Zenject;

namespace ECS.Game.Systems.WesternBuilder_System.StateMachine
{
    public class CreateAndTransferRequestResourceSystem : ReactiveSystem<RequestResourceComponent>
    {
        [Inject] private IDelayService _delayService;
        [Inject] private SignalBus _signalBus;
        protected override EcsFilter<RequestResourceComponent> ReactiveFilter { get; }

        private readonly EcsFilter<UnitsSkillScoreComponent> _skillsEntity;

        private EcsWorld _world;
        
        private int _extractTime;
        private int _requiredMining;
        private int _unitSpeedMain;
        private int _currentRemainingResourceInObject;
        
        private EcsEntity _treeMiningWoodEntity;
        private EcsEntity _woodEntity;
        private UnitView _unitView;
        private ObjectMiningView _objectMiningView;
        private ResourceView _resourceView;

        protected override void Execute(EcsEntity entity)
        {
            foreach (var i in _skillsEntity)
                _unitSpeedMain = _skillsEntity.Get1(i).SkillsOfMine.Get(entity.Get<UnitPriorityData>().RequiredMining).Skill;
            
            if (entity.Get<CurrentMiningObjectData>().CurrentMiningObject.Entity.Has<ObjectMiningComponent>())
            {
                var objectMiningView = entity.Get<CurrentMiningObjectData>().CurrentMiningObject as ObjectMiningView;
                ResourceExtraction(entity, objectMiningView);
            }
            else if (entity.Get<CurrentMiningObjectData>().CurrentMiningObject.Entity.Has<BuildStorageComponent>())
            {
                var objectMiningView = entity.Get<CurrentMiningObjectData>().CurrentMiningObject as BuildsView;
                ResourceExtraction(entity, objectMiningView);
            }
            else if (entity.Get<CurrentMiningObjectData>().CurrentMiningObject.Entity.Has<ResourceComponent>())
            {
                var objectMiningView = entity.Get<CurrentMiningObjectData>().CurrentMiningObject as ResourceView;
                ResourceExtraction(entity, objectMiningView);
            }
        }
        
        private void ResourceExtraction(EcsEntity unitEntity, ResourceView resourceView)
        {
            var unitView = unitEntity.Get<LinkComponent>().View as UnitView;
            var reqMainValue = unitEntity.Get<NextMiningValue>().Value;
            var extractTime = ExtractTime(reqMainValue, 1);
            
            unitView.Entity.Get<EventSetAnimationComponent>().Value = resourceView.MiningAnimationStage;
            unitEntity.Get<UnitCurrentResource>().Value++;
            var minedInCycle = 1;
            
            _delayService.Do(extractTime + 0.1f, () =>
            {
                unitEntity.Get<CurrentMiningObjectData>().CurrentMiningObject.Entity.Get<EventMakeObjectAsChild>()
                        .Parent =
                    unitView.GetResourceStack();
                
                var requiredValueForUnit =
                    unitEntity.Get<UnitPriorityData>().RequiredValueResource -
                    minedInCycle;
                
                if (requiredValueForUnit > 0)
                {
                    unitEntity.Get<UnitPriorityData>().RequiredValueResource = requiredValueForUnit;
                    unitEntity.Get<EventUnitChangeStateComponent>().State = UnitAction.FetchResource;
                }
                else
                {
                    unitEntity.Get<EventUnitChangeStateComponent>().State = UnitAction.FollowAndSetState;

                    unitEntity.Get<FollowAndSetStateComponent>().FeatureState = UnitAction.PutResource;
                    unitEntity.Get<FollowAndSetStateComponent>().SetDistanceView =
                        unitEntity.Get<UnitPriorityData>().TargetBuildsView;
                    unitEntity.Get<FollowAndSetStateComponent>().ControlDistanceView =
                        unitEntity.Get<UnitPriorityData>().TargetBuildsView;
                }
            });
            
        }
        
        private void ResourceExtraction(EcsEntity unitEntity, ObjectMiningView objectMiningView)
        {
            var unitView = unitEntity.Get<LinkComponent>().View as UnitView;
            var reqMainValue = unitEntity.Get<NextMiningValue>().Value;
            var extractTime = ExtractTime(reqMainValue, _unitSpeedMain);

            var minedInCycle = 0;
            
            unitView.Entity.Get<EventSetAnimationComponent>().Value = objectMiningView.MiningAnimationStage;
            
            for (int i = 1; i < reqMainValue + 1; i++)
            {
                _delayService.Do(_unitSpeedMain * i, () =>
                {
                    _world.CreateResourceType(unitView.GetResourceStack(), unitEntity.Get<UnitPriorityData>().RequiredMining);
                    objectMiningView.GetCurrentResourceValue--;
                    unitEntity.Get<UnitCurrentResource>().Value++;
                    minedInCycle++;
                });
            }

            //minedInCycle += reqMainValue;
            
            _delayService.Do(extractTime + 0.1f, () =>
            {
                var requiredValueForUnit =
                    unitEntity.Get<UnitPriorityData>().RequiredValueResource -
                    minedInCycle;
                
                if (requiredValueForUnit > 0)
                {
                    unitEntity.Get<UnitPriorityData>().RequiredValueResource = requiredValueForUnit;
                    unitEntity.Get<EventUnitChangeStateComponent>().State = UnitAction.FetchResource;
                }
                else
                {
                    unitEntity.Get<EventUnitChangeStateComponent>().State = UnitAction.FollowAndSetState;

                    unitEntity.Get<FollowAndSetStateComponent>().FeatureState = UnitAction.PutResource;
                    unitEntity.Get<FollowAndSetStateComponent>().SetDistanceView =
                        unitEntity.Get<UnitPriorityData>().TargetBuildsView;
                    unitEntity.Get<FollowAndSetStateComponent>().ControlDistanceView =
                        unitEntity.Get<UnitPriorityData>().TargetBuildsView;
                }
                
                if (objectMiningView.Entity.IsAlive() && objectMiningView.GetCurrentResourceValue == 0)
                    objectMiningView.Entity.Get<DisableMiningObject>();
            });
        }

        private void ResourceExtraction(EcsEntity unitEntity, BuildsView storageView)
        {
            var unitView = unitEntity.Get<LinkComponent>().View as UnitView;
            var reqMainValue = unitEntity.Get<NextMiningValue>().Value;
            var extractTime = ExtractTime(reqMainValue, 1);
            var minedInCycle = 0;

            //unitView.Entity.Get<EventSetAnimationComponent>().Value = 2;
            
            for (int i = 1; i < reqMainValue + 1; i++)
            {
                _delayService.Do(1 * i, () =>
                {
                    _world.CreateResourceType(unitView.GetResourceStack(), unitEntity.Get<UnitPriorityData>().RequiredMining);
                    storageView.Entity.Get<BuildStorageComponent>().CurrentResourceInStorage--;
                    storageView.Entity.Get<BuildStorageComponent>().LeftToCollectResourceCount++;
                    _signalBus.Fire(new SignalStorageUpdate(storageView));
                    
                    unitEntity.Get<UnitCurrentResource>().Value++;
                    minedInCycle++;
                });
            }
            
            _delayService.Do(extractTime + 0.1f, () =>
            {
                var requiredValueForUnit =
                    unitEntity.Get<UnitPriorityData>().RequiredValueResource -
                    minedInCycle;
                
                if (requiredValueForUnit > 0)
                {
                    unitEntity.Get<UnitPriorityData>().RequiredValueResource = requiredValueForUnit;
                    unitEntity.Get<EventUnitChangeStateComponent>().State = UnitAction.FetchResource;
                }
                else
                {
                    unitEntity.Get<EventUnitChangeStateComponent>().State = UnitAction.FollowAndSetState;

                    unitEntity.Get<FollowAndSetStateComponent>().FeatureState = UnitAction.PutResource;
                    unitEntity.Get<FollowAndSetStateComponent>().SetDistanceView =
                        unitEntity.Get<UnitPriorityData>().TargetBuildsView;
                    unitEntity.Get<FollowAndSetStateComponent>().ControlDistanceView =
                        unitEntity.Get<UnitPriorityData>().TargetBuildsView;
                }
            });
            
        }
        
        private float ExtractTime(int reqValue, int unitSpeedMain)
        {
            float extractTime = 0;
            for (int i = 1; i < reqValue+1; i++)
                extractTime = i * unitSpeedMain;

            Debug.Log(extractTime);
            return extractTime;
            
            //in mid time of dev change logic
            //( because the more time unitWoodSpeedMain, the most time that he spend on mining ( have to be else ) 
        }
    }


    public struct RequestResourceComponent
    {
        public RequestedResource RequestedResource;
        public LinkableView TargetView;
    }

    public enum RequestedResource
    {
        WoodRequest,
        RockRequest
    }
}