using ECS.Core.Utils.ReactiveSystem;
using ECS.Game.Components.General;
using ECS.Game.Components.General.Events;
using Leopotam.Ecs;

namespace ECS.Game.Systems.General.Game
{
    public class GameStageSystem : ReactiveSystem<ChangeStageComponent>
    {
        protected override EcsFilter<ChangeStageComponent> ReactiveFilter { get; }
        protected override void Execute(EcsEntity entity)
        {
            ref var stage = ref entity.Get<ChangeStageComponent>().Value;
            entity.Get<GameStageComponent>().Value = stage;
        }
    }
}