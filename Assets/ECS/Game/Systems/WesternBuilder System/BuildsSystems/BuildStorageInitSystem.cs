using ECS.Core.Utils.ReactiveSystem;
using ECS.Core.Utils.ReactiveSystem.Components;
using ECS.Game.Components.General;
using ECS.Game.Components.WesternBuilder_Component;
using ECS.Views;
using Leopotam.Ecs;

namespace ECS.Game.Systems.WesternBuilder_System.BuildsSystems
{
    public class BuildStorageInitSystem : ReactiveSystem<EventAddComponent<BuildStorageComponent>>
    {
        protected override EcsFilter<EventAddComponent<BuildStorageComponent>> ReactiveFilter { get; }
        protected override void Execute(EcsEntity entity)
        {
            entity.Get<BuildUnderConstruction>();
            entity.Get<ExpectedAmountOfResource>().ExpectedValue = 0;

            var view = entity.Get<LinkComponent>().View as BuildsView;
            
            var maxResource = entity.Get<BuildStorageComponent>().MaxResource;
            view.UpdateScore(0,maxResource);
        }
    }
}