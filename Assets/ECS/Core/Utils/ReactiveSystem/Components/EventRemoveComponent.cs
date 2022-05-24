using Leopotam.Ecs;

namespace ECS.Core.Utils.ReactiveSystem.Components
{
    public struct EventRemoveComponent<T> : IEcsIgnoreInFilter, IEventRemoveComponent where T : struct
    {
        
    }
}