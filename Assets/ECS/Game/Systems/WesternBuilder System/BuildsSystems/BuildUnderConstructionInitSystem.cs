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
            
            entity.Get<ExpectedAmountOfResource>().ExpectedValue = 0;
            entity.Get<BuildUnderConstruction>().CurrentResourceCollected = 0;//???
            entity.Get<BuildUnderConstruction>().RequiredResourceToConstruct = requiredResourceToConstruct;
            
            view.Transform.position = _screenVariables.GetTransformPoint(buildName).position;
            view.Transform.rotation = _screenVariables.GetTransformPoint(buildName).rotation;
            view.UpdateScore(0,requiredResourceToConstruct[0].NeedToConstruct); //TODO for all fields

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
            
            
            foreach (var i in _units)
                _units.GetEntity(i).Get<EventUpdatePriorityComponent>();
        }
    }
}

public struct RecipeComponent
{
    public float WoodCount;
    public float RockCount;

    public Vector3 BuildPoint;
}