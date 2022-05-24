using ECS.Core.Utils.ElapsedTimeSystem;
using ECS.Game.Components.Flags;
using Leopotam.Ecs;

namespace ECS.Game.Systems.General
{
    public class DelayCleanUpSystem : ElapsedTimeSystem<IsDelayCleanUpComponent>
    {
        protected override EcsFilter<IsDelayCleanUpComponent, ElapsedTimeComponent<IsDelayCleanUpComponent>> ElapsedTimeFilter { get; }
        
        protected override void Execute(EcsEntity entity)
        {
            if (entity.Get<ElapsedTimeComponent<IsDelayCleanUpComponent>>().Value > entity.Get<IsDelayCleanUpComponent>().Delay) 
                entity.Get<IsDestroyedComponent>();
        }
    }
    
    public struct IsDelayCleanUpComponent
    {
        public float Delay;
    }
}