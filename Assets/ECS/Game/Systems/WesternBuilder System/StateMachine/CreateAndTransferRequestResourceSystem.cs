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
using UnityEngine;
using Zenject;

namespace ECS.Game.Systems.WesternBuilder_System.StateMachine
{
    public class CreateAndTransferRequestResourceSystem : ReactiveSystem<RequestResourceComponent>
    {
        [Inject] private IDelayService _delayService;
        protected override EcsFilter<RequestResourceComponent> ReactiveFilter { get; }

        private readonly EcsFilter<UnitsSkillScoreComponent> _skillsEntity;

        private EcsWorld _world;

        private UnitView _unitView;
        private int _extractTime;
        private int _requiredMining;
        
        //убрать, скилы нужны будут только в Prioryti
        private int _unitCurrentLogValue;
        private int _requiredValueForUnit;
        private int _unitWoodSpeedMain;
        private int _unitMaxRock;
        //
        
        private EcsEntity _treeMiningWoodEntity;
        private ObjectMiningView _objectMiningView;
        private int _currentRemainingResourceInObject;

        private EcsEntity _woodEntity;
        private ResourceView _resourceView;

        protected override void Execute(EcsEntity entity)
        {
            foreach (var i in _skillsEntity)
            {
                _skillsEntity.GetEntity(i);
                _unitWoodSpeedMain = _skillsEntity.Get1(i).SkillsOfMine.Get(entity.Get<UnitPriorityData>().RequiredMining).Skill;
            }
            
            var objectMiningView = entity.Get<CurrentMiningObjectData>().CurrentMiningObject as ObjectMiningView;
            ResourceExtraction(entity, objectMiningView);
            
        }

        private void ResourceExtraction(EcsEntity unitEntity, ObjectMiningView objectMiningView)
        {
            var unitView = unitEntity.Get<LinkComponent>().View as UnitView;
            var reqMainValue = unitEntity.Get<UnitMainingValue>().CurrentMainResourceValue;
            var extractTime = ExtractTime(reqMainValue, _unitWoodSpeedMain);

            unitView.Entity.Get<EventSetAnimationComponent>().Value = 3;
            unitView.Entity.Get<EventSetAnimationComponent>().StageOfAnim = "Stage";

            for (int i = 1; i < reqMainValue + 1; i++)
            {
                _delayService.Do(_unitWoodSpeedMain * i, () =>
                {
                    _world.CreateResourceType(unitView.GetTransformPoint(), unitEntity.Get<UnitPriorityData>().RequiredMining);
                    objectMiningView.GetCurrentResourceValue--;
                });
            }
            
            _delayService.Do(extractTime + 0.1f, () =>
            {
                var requiredValueForUnit = unitEntity.Get<UnitPriorityData>().RequiredValueResource - unitEntity.Get<UnitMainingValue>().CurrentMainResourceValue;

                if (objectMiningView.Entity.IsAlive() && objectMiningView.GetCurrentResourceValue == 0)
                    objectMiningView.Entity.Get<IsDestroyedComponent>();

                if (requiredValueForUnit != 0)
                {
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

        private float ExtractTime(int reqValue, int unitWoodSpeedMain)
        {
            float extractTime = 0;
            for (int i = 1; i < reqValue+1; i++)
                extractTime = i * unitWoodSpeedMain;

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