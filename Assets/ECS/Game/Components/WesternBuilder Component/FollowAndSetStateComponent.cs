using ECS.Views.General;

namespace ECS.Game.Components.WesternBuilder_Component
{
    public struct FollowAndSetStateComponent
    {
        public UnitAction FeatureState;
        public LinkableView SetDistanceView;
        public LinkableView ControlDistanceView;
    }
}