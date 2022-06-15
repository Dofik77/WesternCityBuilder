namespace ECS.Game.Components.WesternBuilder_Component.dotween_components
{
    public struct MoveTweenEventComponent
    {
        public ETweenEventType EventType;
    }
    
    public enum ETweenEventType
    {
        ResourcePickUp,
        ResourceDelivery
    }
}