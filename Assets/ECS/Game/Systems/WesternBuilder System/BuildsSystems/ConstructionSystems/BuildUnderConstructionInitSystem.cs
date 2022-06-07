using System.Linq;
using DG.Tweening;
using ECS.Core.Utils.ReactiveSystem;
using ECS.Core.Utils.ReactiveSystem.Components;
using ECS.Game.Components.Flags;
using ECS.Game.Components.General;
using ECS.Game.Components.WesternBuilder_Component;
using ECS.Views;
using Leopotam.Ecs;
using Runtime.Data.PlayerData.Recipe;
using Runtime.Game.Utils.MonoBehUtils;
using UnityEngine;
using Zenject;

namespace ECS.Game.Systems.WesternBuilder_System.BuildsSystems
{
    public class BuildUnderConstructionInitSystem : ReactiveSystem<EventAddComponent<BuildUnderConstruction>>
    {
        [Inject] private ScreenVariables _screenVariables;
        private readonly EcsFilter<UnitComponent, LinkComponent>.Exclude<UnitHasPriority> _units;
        protected override EcsFilter <EventAddComponent<BuildUnderConstruction>> ReactiveFilter { get; }
       
        protected override void Execute(EcsEntity entity)
        {
            Recipe currentRecipe = entity.Get<BuildUnderConstruction>().Recipe;
            
            var buildStatus = currentRecipe.GetIsStorageOffData();
            var requiredResourceToConstruct = currentRecipe.GetResourceCount();
            var buildName = currentRecipe.GetName();
            var view = entity.Get<LinkComponent>().View as BuildsView;

            entity.Get<BuildUnderConstruction>().BuildsView = view;
            
            entity.Get<BuildUnderConstruction>().RequiredRecipeResource = 
                (RequiredResourceCount[])requiredResourceToConstruct.Clone();
            entity.Get<BuildUnderConstruction>().RequiredResourceToConstruct = 
                (RequiredResourceCount[])requiredResourceToConstruct.Clone();

            //проверку на лвл
            view.Transform.position = _screenVariables.GetTransformPoint(buildName).position;
            view.Transform.rotation = _screenVariables.GetTransformPoint(buildName).rotation;

            ChangeViewObject(view);
            //InitBuildUI(view, requiredResourceToConstruct);
            CheckBuildOnStorage(entity, buildStatus, currentRecipe);
            
            
            foreach (var i in _units)
                _units.GetEntity(i).Get<EventUpdatePriorityComponent>();
        }

        private void ChangeViewObject(BuildsView view)
        {
            //проверка на левел
            view.BaseObject.SetActive(true);
            view.ConstructedObject.SetActive(false);
        }

        private void InitBuildUI(BuildsView view, RequiredResourceCount[] requiredResourceToConstruct)
        {
            view.UpdateScore(0,10); //TODO for all fields
        }
        
        private void CheckBuildOnStorage(EcsEntity entity, IsStorageOff buildStatus, Recipe currentRecipe)
        {
            if (buildStatus != IsStorageOff.None)
            {
                entity.Get<BuildStorageComponent>().MaxResource = currentRecipe.GetMaxResourceStorage();
                entity.Get<BuildStorageComponent>().IsStorageOff = currentRecipe.GetIsStorageOffData();
                
                entity.Get<BuildStorageComponent>().LeftToCollectResourceCount = 
                    currentRecipe.GetMaxResourceStorage() - entity.Get<BuildStorageComponent>().CurrentResourceInStorage;
                
                switch (currentRecipe.GetIsStorageOffData())
                {
                    case IsStorageOff.Wood :
                        entity.Get<BuildWoodStorageComponent>();
                        break;
                    
                    case IsStorageOff.Rock :
                        entity.Get<BuildRockStorageComponent>();
                        break;
                    
                    case IsStorageOff.Food:
                        break;
                    
                    case IsStorageOff.Ore:
                        break;
                }
            }
        }
    }
}
