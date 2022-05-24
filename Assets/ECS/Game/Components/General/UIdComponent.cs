using PdUtils;

namespace ECS.Game.Components.General
{
    public struct UIdComponent
    {
        public Uid Value;
        public override string ToString() => Value.ToString();
    }
}