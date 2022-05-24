using ECS.Core.Utils.ReactiveSystem;
using ECS.Game.Components.Flags;
using ECS.Game.Components.General;
using Leopotam.Ecs;

namespace ECS.Game.Systems.General
{
    public class CleanUpSystem : ReactiveSystem<IsDestroyedComponent>
    {
        protected override EcsFilter<IsDestroyedComponent> ReactiveFilter { get; }
        protected override bool DeleteEvent => false;
        protected override void Execute(EcsEntity entity)
        {
            if (entity.Has<LinkComponent>())
                entity.Get<LinkComponent>().View.DestroyObject();
            entity.Destroy();
        }
    }
}