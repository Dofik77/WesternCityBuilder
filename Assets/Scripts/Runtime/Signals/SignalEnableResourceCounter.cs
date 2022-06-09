using ECS.Views;

namespace Runtime.Signals
{
    public struct SignalEnableResourceCounter
    {
        public BuildsView BuildsView;
        
        public SignalEnableResourceCounter(BuildsView buildsView)
        {
            BuildsView = buildsView;
        }
    }
}