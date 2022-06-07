using ECS.Views;

namespace ECS.Game.Components.WesternBuilder_Component
{
    public struct UnitPriorityData
    {
        public BuildsView TargetBuildsView;

        public int RequiredValueResource;
        public RequiredResourceType RequiredMining;
    }

    public struct UnitMainingValue
    {
        public int CurrentMainResourceValue; 
    }

}