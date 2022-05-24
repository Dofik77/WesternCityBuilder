using DataBase.Timer;

namespace Runtime.Signals
{
    public struct SignalTimerUpdate
    {
        public Timer Value;
        
        public SignalTimerUpdate(Timer value)
        {
            Value = value;
        }
    }
}