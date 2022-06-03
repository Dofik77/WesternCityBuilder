﻿using ECS.Core.Utils.ReactiveSystem;
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
        //при спавне постройки давать компонент ( активная постройка ) и искать по фильтру активные постройки, и вытаскивать от туда данные
        private readonly EcsFilter<UnitsSkillScoreComponent> _unitSkills;
        private readonly EcsFilter<BuildCampFireComponent, LinkComponent> _campfire;
        
        private readonly EcsFilter<BuildUnderConstruction, LinkComponent> _buildConstruction;
        private readonly EcsFilter<BuildWoodStorageComponent, LinkComponent> _woodStorage;
        private readonly EcsFilter<BuildRockStorageComponent, LinkComponent> _rockStorage;
        
        //exclude заполненые склады по компоненту, exclude isBuildingNow, потому что склад сначало строиться,
        //а потом являеться складом
        //заменить на единый storageComponent.enum
        
        //2ой компонент - под recipe ( сколько дерева? сколько еды? сколько камня? ) 
        

        protected override EcsFilter<EventUpdatePriorityComponent> ReactiveFilter { get; }

        private EcsEntity _campFireEntity;
        private EcsEntity _woodStorageEntity;
        private EcsEntity _rockStorageEntity;
        private EcsEntity _buildUnderConstructionEntity;
        private EcsEntity _unitSkillsEntity;
       

        private int _maxWoodTakeUnitResource;


        private Priority _priority;
        
        //idea 
        //система приоритетов будет выставлять enum для того, что нужно сделать 
        //и лишь после этого по enum выбирать действия

        protected override void Execute(EcsEntity entity)
        {
            CheckingActivities();
            ChoosePriority();
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
        private void ChoosePriority()
        {
            if (!_buildUnderConstructionEntity.IsNull())
                _priority = Priority.Reciepe;
            else if 
                (!_woodStorageEntity.IsNull() || !_rockStorageEntity.IsNull())
            {
                if (!_woodStorageEntity.IsNull() && !_rockStorageEntity.IsNull())
                {
                    var maxWoodInStorage = _woodStorageEntity.Get<BuildStorageComponent>().MaxResource;
                    var expectedAmountWoodStorageOfResource = _woodStorageEntity.Get<ExpectedTypeAndValueResource>().ExpectedValue;
                    
                    var maxRockInStorage = _rockStorageEntity.Get<BuildStorageComponent>().MaxResource;
                    var expectedAmountRockStorageOfResource = _rockStorageEntity.Get<ExpectedTypeAndValueResource>().ExpectedValue;

                    if (expectedAmountRockStorageOfResource < maxRockInStorage && !(expectedAmountWoodStorageOfResource < maxWoodInStorage))
                        _priority = Priority.RockStorage;
                    else if (expectedAmountWoodStorageOfResource < maxWoodInStorage)
                        _priority = Priority.WoodStorage;
                    else
                        _priority = Priority.Await;
                }
                
                else if (!_woodStorageEntity.IsNull())
                {
                    var maxWoodInStorage = _woodStorageEntity.Get<BuildStorageComponent>().MaxResource;
                    var expectedAmountWoodStorageOfResource = _woodStorageEntity.Get<ExpectedTypeAndValueResource>().ExpectedValue;
                    
                    if (expectedAmountWoodStorageOfResource < maxWoodInStorage)
                        _priority = Priority.WoodStorage;
                    else
                        _priority = Priority.Await;
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
                    WoodStorageUpdate(entity);
                    break;
                
                case Priority.RockStorage :
                    RockStorageUpdate(entity);
                    break;
                case Priority.Reciepe :
                    RecipeUpdate(entity);
                    break;
            }
        }
        
        private void RecipeUpdate(EcsEntity entity)
        {
            //может быть ситуация, когда в рецепте уже 0, но в постройке ещё не присены все ресрусы
            //для этого, перед каждый началом работы RecipeUpdate, нужно проверять, есть ли в Recipe[key,value]==0?
            //если да, то отправляем его ждать в BuildUnderConstruct
            var requiredResourceToConstruct = 
                _buildUnderConstructionEntity.Get<BuildUnderConstruction>().RequiredResourceToConstruct;
            var currentResourceCollected = 
                _buildUnderConstructionEntity.Get<BuildUnderConstruction>().CurrentResourceCollected;
            var targetView = entity.Get<BuildUnderConstruction>().BuildsView;

            foreach (var resource in requiredResourceToConstruct)
            {
                var resourceType = resource.Key;
                var resourceCount = resource.NeedToConstruct;

                entity.Get<UnitPriorityData>().RequiredMining = resourceType;
                entity.Get<UnitPriorityData>().TargetBuildsView = targetView;
                
                // var maxWoodTakeUnitResource = _unitSkillsEntity.Get<UnitsSkillScoreComponent>().SkillOfPortability.Get(resourceType).Skill;
                // var requiredWoodForUnit = maxWoodInStorage - expectedAmountOfResource;
                // var requiredValueForUnit = requiredWoodForUnit > maxWoodTakeUnitResource
                //     ? maxWoodTakeUnitResource : requiredWoodForUnit;
                
                
            }

        }

        private void WoodStorageUpdate(EcsEntity entity)
        {
            foreach (var i in _unitSkills)
                _unitSkillsEntity = _unitSkills.GetEntity(i);
            
            var maxWoodInStorage = _woodStorageEntity.Get<BuildStorageComponent>().MaxResource;
            var expectedAmountOfResource = _woodStorageEntity.Get<ExpectedTypeAndValueResource>().ExpectedValue;
            
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
            var expectedAmountOfResource = _rockStorageEntity.Get<ExpectedTypeAndValueResource>().ExpectedValue;
            
            var RequiredMining = RequiredResourceType.RockResource;
                
            entity.Get<UnitPriorityData>().RequiredMining = RequiredMining;
            entity.Get<UnitPriorityData>().TargetBuildsView = _rockStorageEntity.Get<LinkComponent>().View as BuildsView;
    
            var maxRockTakeUnitResource = _unitSkillsEntity.Get<UnitsSkillScoreComponent>().SkillOfPortability.Get(RequiredMining).Skill;
            var requiredRock = maxRockInStorage - expectedAmountOfResource;
            var requiredValueForUnit = requiredRock > maxRockTakeUnitResource ? maxRockTakeUnitResource : requiredRock;
                
            entity.Get<UnitPriorityData>().RequiredValueResource = requiredValueForUnit;
            entity.Get<EventUnitChangeStateComponent>().State = UnitAction.FetchResource;
           
        }

        // private void StoragesUpdate(EcsEntity entity)
        // {
        //     if (!_rockStorageEntity.IsNull())
        //     {
        //         var maxRockInStorage = _rockStorageEntity.Get<BuildStorageComponent>().MaxResource;
        //         var expectedAmountOfResource = _rockStorageEntity.Get<ExpectedAmountOfResource>().ExpectedValue;
        //
        //         if (expectedAmountOfResource < maxRockInStorage)
        //         {
        //             var RequiredMining = RequiredResourceType.RockResourceType;
        //             
        //             entity.Get<UnitPriorityData>().RequiredMining = RequiredMining;
        //             entity.Get<UnitPriorityData>().TargetBuildsView = _rockStorageEntity.Get<LinkComponent>().View as BuildsView;
        //
        //             var maxRockTakeUnitResource = _unitSkillsEntity.Get<UnitsSkillScoreComponent>().SkillOfPortability.Get(RequiredMining).Skill;
        //             var requiredRock = maxRockInStorage - expectedAmountOfResource;
        //             var requiredValueForUnit = requiredRock > maxRockTakeUnitResource ? maxRockTakeUnitResource : requiredRock;
        //             
        //             entity.Get<UnitPriorityData>().RequiredValueResource = requiredValueForUnit;
        //             entity.Get<EventUnitChangeStateComponent>().State = UnitAction.FetchResource;
        //         }
        //         else 
        //             entity.Get<EventUnitChangeStateComponent>().State = UnitAction.AwaitNearCampFire;
        //     }
        //    
        // }
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
    
    Reciepe
}


    