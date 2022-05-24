using ECS.Views.General;

namespace ECS.Game.Components.General
{
    public struct LinkComponent
    {
        public ILinkable View;

        public T Get<T>() where T : ILinkable
        {
            return (T) View;
        }
    }
}