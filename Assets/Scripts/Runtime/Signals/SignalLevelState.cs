using ECS.Game.Systems.General.Level;

namespace Runtime.Signals
{
    public struct SignalLevelState
    {
        public ELevelState State;
        
        public SignalLevelState(ELevelState state)
        {
            State = state;
        }
    }
}