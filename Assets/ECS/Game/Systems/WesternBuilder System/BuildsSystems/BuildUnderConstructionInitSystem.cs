using ECS.Core.Utils.ReactiveSystem;
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
    public class BuildUnderConstructionInitSystem : ReactiveSystem<BuildUnderConstruction>
    {
        [Inject] private ScreenVariables _screenVariables;
        private readonly EcsFilter<UnitComponent, LinkComponent>.Exclude<UnitHasPriority> _units;
        protected override EcsFilter<BuildUnderConstruction> ReactiveFilter { get; }
       
        protected override void Execute(EcsEntity entity)
        {
            Recipe currentRecipe = entity.Get<BuildUnderConstruction>().Recipe;
            
            var buildStatus = currentRecipe.GetIsStorageOffData();
            var requiredResourceToConstruct = currentRecipe.GetResourceCount();
            var buildName = currentRecipe.GetName();
            var view = entity.Get<LinkComponent>().View as BuildsView;

            entity.Get<BuildUnderConstruction>().BuildsView = view;
            entity.Get<BuildUnderConstruction>().CurrentResourceCollected = 0;
            entity.Get<BuildUnderConstruction>().RequiredResourceToConstruct = requiredResourceToConstruct;
            
            view.Transform.position = _screenVariables.GetTransformPoint(buildName).position;
            view.Transform.rotation = _screenVariables.GetTransformPoint(buildName).rotation;

            InitBuildUI(view, requiredResourceToConstruct);
            InitExpectedResources(entity, requiredResourceToConstruct);
            CheckBuildOnStorage(entity, buildStatus, currentRecipe);
            
            foreach (var i in _units)
                _units.GetEntity(i).Get<EventUpdatePriorityComponent>();
        }

        private void InitBuildUI(BuildsView view, RequiredResourceCount[] requiredResourceToConstruct)
        {
            view.UpdateScore(0,requiredResourceToConstruct[0].NeedToConstruct); //TODO for all fields
        }//TODO 
        
        private void InitExpectedResources(EcsEntity entity, RequiredResourceCount[] requiredResourceToConstruct)
        {
            foreach (var resource in requiredResourceToConstruct)
            {
                entity.Get<ExpectedTypeAndValueResource>().ResourceTypeValuePair =
                    new ResourceTypeValuePair(resource.Key, resource.NeedToConstruct);
            }
        }

        private void CheckBuildOnStorage(EcsEntity entity, IsStorageOff buildStatus, Recipe currentRecipe)
        {
            if (buildStatus != IsStorageOff.None)
            {
                entity.Get<BuildStorageComponent>().MaxResource = currentRecipe.GetMaxResourceStorage();

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
