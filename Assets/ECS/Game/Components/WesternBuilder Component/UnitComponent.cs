using Leopotam.Ecs;

namespace ECS.Game.Components.Flags
{
    public struct UnitComponent : IEcsIgnoreInFilter
    {
        public bool AxeAvailable;
        public bool PickAvailable;
    }
    
    
    public struct UnitHasPriority
    {
        
    }
}