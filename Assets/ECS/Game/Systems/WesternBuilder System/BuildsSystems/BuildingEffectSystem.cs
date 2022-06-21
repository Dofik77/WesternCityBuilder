using ECS.Core.Utils.ReactiveSystem;
using ECS.Game.Components.Flags;
using ECS.Game.Components.General;
using ECS.Game.Systems.WesternBuilder_System.BuildsSystems.BuildEffectSystem;
using ECS.Views;
using Leopotam.Ecs;
using Runtime.Signals;
using Zenject;

namespace ECS.Game.Systems.WesternBuilder_System.BuildsSystems
{
    public class BuildingEffectSystem : ReactiveSystem<BuildingEffect>
    {
        [Inject] private SignalBus _signalBus;
        protected override EcsFilter<BuildingEffect> ReactiveFilter { get; }
        protected override void Execute(EcsEntity entity)
        {
            var buildView = entity.Get<LinkComponent>().View as BuildsView;
            
            switch (buildView.ObjectType)
            {
                case RequiredObjectType.WoodStorage :
                    _signalBus.Fire(new SignalEnableResourceCounter(buildView));
                    break;
                case RequiredObjectType.RockStorage : 
                    _signalBus.Fire(new SignalEnableResourceCounter(buildView));
                    break;
                case RequiredObjectType.FoodStorage : 
                    _signalBus.Fire(new SignalEnableResourceCounter(buildView));
                    break;
                case RequiredObjectType.ToolRecipe :
                    buildView.Entity.Get<IsDestroyedComponent>();
                    break;
                
                case RequiredObjectType.Tower :
                    entity.Get<EventTowerEffect>();
                    break;
                
            }
        }
    }


    public struct BuildingEffect
    {
       
    }
    
}