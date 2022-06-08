using ECS.Views;

namespace ECS.Game.Components.WesternBuilder_Component
{
    public struct UnitPriorityData
    {
        public int CurrentMining;
        
        public BuildsView TargetBuildsView;

        public int RequiredValueResource;
        public RequiredResourceType RequiredMining;
    }

    public struct NextMiningValue
    {
        public int Value; 
    }

    public struct UnitCurrentResource
    {
        public int Value;
    }

}