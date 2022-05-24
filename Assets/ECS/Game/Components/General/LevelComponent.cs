using Leopotam.Ecs;

namespace ECS.Game.Components.General
{
    public struct LevelComponent : IEcsIgnoreInFilter
    {
    }

    public struct LevelDataComponent
    {
        public int UseRollback;
        public int UseHint;
    }
}