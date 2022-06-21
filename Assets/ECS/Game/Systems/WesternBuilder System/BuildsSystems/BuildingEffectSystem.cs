using ECS.Core.Utils.ReactiveSystem;
using ECS.Game.Components.General;
using ECS.Views;
using Leopotam.Ecs;

namespace ECS.Game.Systems.WesternBuilder_System.BuildsSystems
{
    public class BuildingEffectSystem : ReactiveSystem<BuildingEffect>
    {
        protected override EcsFilter<BuildingEffect> ReactiveFilter { get; }
        protected override void Execute(EcsEntity entity)
        {
            var view = entity.Get<LinkComponent>().View as BuildsView;
            
            switch (view.ObjectType)
            {
                
            }
        }
    }


    public struct BuildingEffect
    {
        private BuildsView BuildsView;
    }
    
}