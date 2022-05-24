using ECS.Game.Components.General;

namespace Runtime.Signals
{
    public struct SignalLevelDataUpdate
    {
        public LevelDataComponent Value;
        
        public SignalLevelDataUpdate(LevelDataComponent value)
        {
            Value = value;
        }
    }
}