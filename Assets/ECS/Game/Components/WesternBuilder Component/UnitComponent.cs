using Leopotam.Ecs;

namespace ECS.Game.Components.Flags
{
    public struct UnitComponent : IEcsIgnoreInFilter
    {
        public int CurrentWood;
        public int CurrentRock;
        
        //вынести в отдельный компонент
    }
    
    
    public struct UnitHasPriority
    {
        
    }
}