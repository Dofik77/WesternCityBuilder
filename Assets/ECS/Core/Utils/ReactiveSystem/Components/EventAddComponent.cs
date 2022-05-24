using Leopotam.Ecs;

namespace ECS.Core.Utils.ReactiveSystem.Components
{
    public struct EventAddComponent<T> : IEcsIgnoreInFilter, IEventAddComponent where T : struct
    {
        
    }
}