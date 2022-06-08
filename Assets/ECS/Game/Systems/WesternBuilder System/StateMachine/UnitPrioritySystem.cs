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
        
        //exclude заполненые склады по компоненту, exclude isBuildingNow, потому что склад сначало строиться,
        //а потом являеться складом
        //заменить на единый storageComponent.enum
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
            //проверять юнита на действия и бездействия ( UnitInAction/UnitHasPriority ) - будет давать и удаляться по факту необходимости
            //- если поступил рецепт, а они че то добывают
            //должны добыть до конца, а потом идти делать рецепт
            
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
            bool recipeNotVoid = true;
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

                    entity.Get<UnitCurrentResource>().Value = 0;
                    entity.Get<UnitPriorityData>().RequiredValueResource = requiredValueForUnit;
                    entity.Get<EventUnitChangeStateComponent>().State = UnitAction.FetchResource;
                    
                    recipeNotVoid = false;
                    break;
                }
            }
            
            if (recipeNotVoid)
                entity.Get<EventUnitChangeStateComponent>().State = UnitAction.AwaitNearСonstruction;
            
            //может быть ситуация, когда в рецепте уже 0, но в постройке ещё не присены все ресрусы
            //для этого, перед каждый началом работы RecipeUpdate, нужно проверять, есть ли в Recipe[key,value]==0?
            //если да, то отправляем его ждать в BuildUnderConstruct
        }

        private void WoodStorageUpdate(EcsEntity entity)
        {
            foreach (var i in _unitSkills)
                _unitSkillsEntity = _unitSkills.GetEntity(i);
            
            var maxWoodInStorage = _woodStorageEntity.Get<BuildStorageComponent>().MaxResource;
            var expectedAmountOfResource = _woodStorageEntity.Get<ExpectedValueResource>().ExpectedValue;
            
            var RequiredMining = RequiredResourceType.WoodResource;
                
            entity.Get<UnitPriorityData>().RequiredMining = RequiredMining;
            entity.Get<UnitPriorityData>().TargetBuildsView = _woodStorageEntity.Get<LinkComponent>().View as BuildsView;
        
            var maxWoodTakeUnitResource = _unitSkillsEntity.Get<UnitsSkillScoreComponent>().SkillOfPortability.Get(RequiredMining).Skill;
            var requiredWood = maxWoodInStorage - expectedAmountOfResource;
            var requiredValueForUnit = requiredWood > maxWoodTakeUnitResource ? maxWoodTakeUnitResource : requiredWood;
                
            entity.Get<UnitPriorityData>().RequiredValueResource = requiredValueForUnit;
            entity.Get<EventUnitChangeStateComponent>().State = UnitAction.FetchResource;
        }

        private void RockStorageUpdate(EcsEntity entity)
        {
            foreach (var i in _unitSkills)
                _unitSkillsEntity = _unitSkills.GetEntity(i);
            
            var maxRockInStorage = _rockStorageEntity.Get<BuildStorageComponent>().MaxResource;
            var expectedAmountOfResource = _rockStorageEntity.Get<ExpectedValueResource>().ExpectedValue;
            
            var RequiredMining = RequiredResourceType.RockResource;
                
            entity.Get<UnitPriorityData>().RequiredMining = RequiredMining;
            entity.Get<UnitPriorityData>().TargetBuildsView = _rockStorageEntity.Get<LinkComponent>().View as BuildsView;
    
            var maxRockTakeUnitResource = _unitSkillsEntity.Get<UnitsSkillScoreComponent>().SkillOfPortability.Get(RequiredMining).Skill;
            var requiredRock = maxRockInStorage - expectedAmountOfResource;
            var requiredValueForUnit = requiredRock > maxRockTakeUnitResource ? maxRockTakeUnitResource : requiredRock;
                
            entity.Get<UnitPriorityData>().RequiredValueResource = requiredValueForUnit;
            entity.Get<EventUnitChangeStateComponent>().State = UnitAction.FetchResource;
           
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
            
            entity.Get<UnitPriorityData>().RequiredValueResource = requiredValueForUnit;
            entity.Get<EventUnitChangeStateComponent>().State = UnitAction.FetchResource;
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


    