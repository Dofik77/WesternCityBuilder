using ECS.Core.Utils.ReactiveSystem;
using ECS.Game.Components.Flags;
using ECS.Game.Components.General;
using ECS.Game.Components.WesternBuilder_Component;
using ECS.Views;
using ECS.Views.General;
using ECS.Views.WesterBuilderView;
using Leopotam.Ecs;
using Runtime.Data;
using UnityEngine;
using UnityEngine.Events;

namespace ECS.Game.Systems.WesternBuilder_System.StateMachine
{
    public class UnitPrioritySystem : ReactiveSystem<EventUpdatePriorityComponent>
    {
        private readonly EcsFilter<UnitsSkillScoreComponent> _unitSkills;
        private readonly EcsFilter<BuildCampFireComponent, LinkComponent> _campfire;
        
        private readonly EcsFilter<BuildUnderConstruction, LinkComponent> _buildConstruction;
        private readonly EcsFilter<BuildWoodStorageComponent, LinkComponent>.Exclude<BuildUnderConstruction> _woodStorage;
        private readonly EcsFilter<BuildRockStorageComponent, LinkComponent>.Exclude<BuildUnderConstruction> _rockStorage;
        
        protected override EcsFilter<EventUpdatePriorityComponent> ReactiveFilter { get; }

        private EcsEntity _campFireEntity;
        private EcsEntity _woodStorageEntity;
        private EcsEntity _rockStorageEntity;
        private EcsEntity _buildUnderConstructionEntity;
        private EcsEntity _unitSkillsEntity;
        
        private Priority _priority;

        private int _maxWoodTakeUnitResource;
        
        protected override void Execute(EcsEntity entity)
        {
            CheckingActivities();
            ChoosePriority(entity);
            TransferPriority(_priority, entity);
        }
        
        private void CheckingActivities()
        {
            foreach (var i in _campfire)
                _campFireEntity = _campfire.GetEntity(i);

            foreach (var i in _buildConstruction)
                _buildUnderConstructionEntity = _buildConstruction.GetEntity(i);
            
            foreach (var i in _woodStorage)
                _woodStorageEntity = _woodStorage.GetEntity(i);

            foreach (var i in _rockStorage)
                _rockStorageEntity = _rockStorage.GetEntity(i);
            
            foreach (var i in _unitSkills)
                _unitSkillsEntity = _unitSkills.GetEntity(i);
        }
        private void ChoosePriority(EcsEntity unitEntity)
        {
            if (!_buildConstruction.IsEmpty()) 
                _priority = Priority.Reciepe;
            else if (!_woodStorage.IsEmpty() || !_rockStorage.IsEmpty()) 
            {
                if (!_woodStorageEntity.IsNull() && !_rockStorageEntity.IsNull())
                {
                    var needWoodResource = _woodStorageEntity.Get<BuildStorageComponent>().LeftToCollectResourceCount;
                    var needRockResource = _rockStorageEntity.Get<BuildStorageComponent>().LeftToCollectResourceCount;

                    if (needWoodResource > 0)
                        _priority = Priority.WoodStorage;
                    else if (needRockResource > 0)
                        _priority = Priority.RockStorage;
                    else
                        _priority = Priority.Await;
                }
                
                else if (!_woodStorageEntity.IsNull())
                {
                    var needWoodResource = _woodStorageEntity.Get<BuildStorageComponent>().LeftToCollectResourceCount;

                    if (needWoodResource > 0)
                        _priority = Priority.WoodStorage;
                    else
                        _priority = Priority.Await;
                    
                    //декомпазировать на методы
                }
            }
            else
                _priority = Priority.Await;
        }
        
        private void TransferPriority(Priority priority, EcsEntity entity)
        {
            switch (priority)
            {
                case Priority.Await :
                    entity.Get<EventUnitChangeStateComponent>().State = UnitAction.AwaitNearCampFire;
                    break;
                
                case Priority.WoodStorage :
                    StoragesUpdate(entity, _woodStorageEntity, RequiredResourceType.WoodResource);
                    break;
                
                case Priority.RockStorage :
                    StoragesUpdate(entity, _rockStorageEntity, RequiredResourceType.RockResource);
                    break;
                
                case Priority.Reciepe :
                    RecipeUpdate(entity);
                    break;
            }
        }
        
        private void RecipeUpdate(EcsEntity entity)
        {
            bool recipeComplete = true;
            var requiredResourceToConstruct = 
                _buildUnderConstructionEntity.Get<BuildUnderConstruction>().RequiredRecipeResource;

            for (int i = 0; i < requiredResourceToConstruct.Length; i++)
            {
                if (requiredResourceToConstruct[i].NeedToConstruct > 0)
                {
                    var resourceType = requiredResourceToConstruct[i].Key;
                    var resourceCount = requiredResourceToConstruct[i].NeedToConstruct;
                    var targetView = _buildUnderConstructionEntity.Get<BuildUnderConstruction>().BuildsView;
                    
                    entity.Get<UnitPriorityData>().RequiredMining = resourceType;
                    entity.Get<UnitPriorityData>().TargetBuildsView = targetView;
                    
                    var maxTakeUnitResource = _unitSkillsEntity.Get<UnitsSkillScoreComponent>().SkillOfPortability.Get(resourceType).Skill;
                    var requiredValueForUnit = resourceCount > maxTakeUnitResource ? maxTakeUnitResource : resourceCount;
                    
                    _buildUnderConstructionEntity.Get<BuildUnderConstruction>().RequiredRecipeResource[i].NeedToConstruct
                        -= requiredValueForUnit;

                    entity.Get<UnitHasPriority>();

                    entity.Get<UnitCurrentResource>().Value = 0;
                    entity.Get<UnitPriorityData>().RequiredValueResource = requiredValueForUnit;

                    if (!ResourceInStoragesAvailable(entity, requiredValueForUnit, resourceType))
                        entity.Get<EventUnitChangeStateComponent>().State = UnitAction.FetchResource;
                    
                    recipeComplete = false;
                    break;
                }
            }
            
            if (recipeComplete)
                entity.Get<EventUnitChangeStateComponent>().State = UnitAction.AwaitNearСonstruction;
        }
        
        private void StoragesUpdate(EcsEntity entity, EcsEntity storage, RequiredResourceType resourceType)
        {
            var needValueResource = storage.Get<BuildStorageComponent>().LeftToCollectResourceCount;
            var targetView = storage.Get<LinkComponent>().View as BuildsView;
        
            entity.Get<UnitPriorityData>().RequiredMining = resourceType;
            entity.Get<UnitPriorityData>().TargetBuildsView = targetView;
        
            var maxTakeUnitResource = _unitSkillsEntity.Get<UnitsSkillScoreComponent>().SkillOfPortability.Get(resourceType).Skill;
            var requiredValueForUnit = needValueResource > maxTakeUnitResource ? maxTakeUnitResource : needValueResource;

            storage.Get<BuildStorageComponent>().LeftToCollectResourceCount -= requiredValueForUnit;
            
            entity.Get<UnitHasPriority>();
            entity.Get<UnitPriorityData>().RequiredValueResource = requiredValueForUnit;
            entity.Get<EventUnitChangeStateComponent>().State = UnitAction.FetchResource;
        }

        private bool ResourceInStoragesAvailable(EcsEntity unitEntity, int requiredValueForUnit, RequiredResourceType requiredResourceType)
        {
            bool StorageNotNull = false;

            switch (requiredResourceType)
            {
                case RequiredResourceType.WoodResource :
                    if (!_woodStorage.IsEmpty())
                    {
                        var storageView = _woodStorageEntity.Get<LinkComponent>().View as BuildsView;
                        if (_woodStorageEntity.Get<BuildStorageComponent>().PromisingResourceValue > 0)
                        {
                            var resourceInStorage = _woodStorageEntity.Get<BuildStorageComponent>().PromisingResourceValue;
                            
                            requiredValueForUnit = resourceInStorage > requiredValueForUnit
                                ? requiredValueForUnit
                                : resourceInStorage;
                            
                            unitEntity.Get<NextMiningValue>().Value = requiredValueForUnit;
                            _woodStorageEntity.Get<BuildStorageComponent>().PromisingResourceValue -=
                                requiredValueForUnit;
                            
                            //for LeftToCollect can be evalute by dif Current-PromisingResourceValue ( 10 - 7 = 3 )
                            
                            unitEntity.Get<FollowAndSetStateComponent>().FeatureState = UnitAction.TakeResource;
                            unitEntity.Get<FollowAndSetStateComponent>().SetDistanceView = _woodStorageEntity.Get<LinkComponent>().View as BuildsView;
                            unitEntity.Get<FollowAndSetStateComponent>().ControlDistanceView = storageView;
                            
                            unitEntity.Get<EventUnitChangeStateComponent>().State = UnitAction.FollowAndSetState;
                            unitEntity.Get<CurrentMiningObjectData>().CurrentMiningObject = storageView;

                            StorageNotNull = true;
                        }
                    }
                    break;
                
                case RequiredResourceType.RockResource :
                    if (!_rockStorage.IsEmpty())
                    {
                        var storageView = _rockStorageEntity.Get<LinkComponent>().View as BuildsView;
                        if (_rockStorageEntity.Get<BuildStorageComponent>().PromisingResourceValue > 0)
                        {
                            var resourceInStorage = _woodStorageEntity.Get<BuildStorageComponent>().PromisingResourceValue;
                            
                            requiredValueForUnit = resourceInStorage > requiredValueForUnit
                                ? requiredValueForUnit
                                : resourceInStorage;
                            
                            unitEntity.Get<NextMiningValue>().Value = requiredValueForUnit;
                            _rockStorageEntity.Get<BuildStorageComponent>().PromisingResourceValue -=
                                requiredValueForUnit;
                            
                            //for LeftToCollect can be evalute by dif Current-PromisingResourceValue ( 10 - 7 = 3 )
                            
                            unitEntity.Get<FollowAndSetStateComponent>().FeatureState = UnitAction.TakeResource;
                            unitEntity.Get<FollowAndSetStateComponent>().SetDistanceView = _woodStorageEntity.Get<LinkComponent>().View as BuildsView;
                            unitEntity.Get<FollowAndSetStateComponent>().ControlDistanceView = storageView;
                            
                            unitEntity.Get<EventUnitChangeStateComponent>().State = UnitAction.FollowAndSetState;
                            unitEntity.Get<CurrentMiningObjectData>().CurrentMiningObject = storageView;

                            StorageNotNull = true;
                        }
                    }
                    break;
            }
            return StorageNotNull;
        }
    }
}


public struct EventUpdatePriorityComponent
{
    
}


public enum Priority
{
    Await,
    
    WoodStorage,
    RockStorage,
    FoodStorage,
    
    Reciepe
}


    