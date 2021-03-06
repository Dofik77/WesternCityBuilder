using ECS.Core.Utils.ReactiveSystem;
using ECS.Game.Components.Flags;
using ECS.Game.Components.General;
using ECS.Game.Components.WesternBuilder_Component;
using ECS.Views;
using Leopotam.Ecs;
using Runtime.Data.PlayerData.Recipe;
using Runtime.Services.DelayService;
using Runtime.Signals;
using Zenject;

namespace ECS.Game.Systems.WesternBuilder_System.BuildsSystems
{
    public class BuildConstructionSystem : ReactiveSystem<BuildConstruction>
    {
        [Inject] private IDelayService _delayService;
        [Inject] private SignalBus _signalBus;
        
        private readonly EcsFilter<UnitComponent, LinkComponent> _units;
        protected override EcsFilter<BuildConstruction> ReactiveFilter { get; }
        protected override void Execute(EcsEntity entity)
        {
            var buildView = entity.Get<LinkComponent>().View as BuildsView;
            var constructionValue = entity.Get<BuildUnderConstruction>().Recipe.GetResourceCount();
            var delayToConstruct = CalculatedTimeForConstruct(constructionValue);

            foreach (var unit in _units)
                _units.GetEntity(unit).Get<EventSetAnimationComponent>().Value = 5;

            buildView.ResourceCountToConstructionProgressBar.gameObject.SetActive(false);
            buildView.CounctractProgressBar.gameObject.SetActive(true); 
            buildView.CounctractProgressBar.RepaintConstruction(1, delayToConstruct);
            
            _delayService.Do(delayToConstruct, () =>
            {
                entity.Get<BuildComponent>();
                entity.Get<BuildingEffect>();
                
                ChangeObjectStage(buildView);
                
                foreach (var i in _units)
                {
                    _units.GetEntity(i).Del<UnitHasPriority>();
                    _units.GetEntity(i).Get<EventUpdatePriorityComponent>();
                }

                var key = entity.Get<BuildUnderConstruction>().Recipe.GetLowerKey();
                
                _signalBus.Fire(new SignalRecipeUpdate(key));
                
                entity.Del<BuildUnderConstruction>();
            });
        }
        
        //give true status on some tools
        private void ChangeObjectStage(BuildsView buildView)
        {
            buildView.BaseObject.SetActive(false);
            buildView.CounctractProgressBar.gameObject.SetActive(false);

            if (buildView.ObjectType != RequiredObjectType.ToolRecipe)
            {
                buildView.ConstructedObject.SetActive(true);
                buildView.StopDistance *= 1.4f;
            }
        }

        private int CalculatedTimeForConstruct(RequiredResourceCount[] resourceCounts)
        {
            int TimeForBuild = 0;
            
            foreach (var resource in resourceCounts)
            {
                TimeForBuild += resource.NeedToConstruct;
            }
            return TimeForBuild;
        }
    }


    public struct BuildConstruction
    {
        
    }
}