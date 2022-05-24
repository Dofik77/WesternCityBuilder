using ECS.Views;

namespace ECS.Game.Components.WesternBuilder_Component
{
    public struct UnitPriorityData
    {
        public BuildsView TargetBuildsView;

        public int RequiredValueResource;
        public int CurrentValueResource;
        public RequiredResourceType RequiredMining;
    }

    public struct UnitDataMainValue
    {
        public int CurrentMainResourceValue; 
    }

}