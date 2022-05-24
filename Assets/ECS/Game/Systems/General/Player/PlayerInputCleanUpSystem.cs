using ECS.Core.Utils.SystemInterfaces;
using Leopotam.Ecs;

namespace ECS.Game.Systems.General.Player
{
    public class PlayerInputCleanUpSystem : IEcsUpdateSystem
    {
#pragma warning disable
        private readonly EcsFilter<EventInputDownComponent> _eventDown;
        private readonly EcsFilter<EventInputHoldAndDragComponent> _eventHoldAndDrag;
        private readonly EcsFilter<EventInputUpComponent> _eventUp;
#pragma warning restore 649

        public void Run()
        {
            foreach (var i in _eventDown)
                _eventDown.GetEntity(i).Del<EventInputDownComponent>();
            foreach (var i in _eventHoldAndDrag)
                _eventHoldAndDrag.GetEntity(i).Del<EventInputHoldAndDragComponent>();
            foreach (var i in _eventUp)
                _eventUp.GetEntity(i).Del<EventInputUpComponent>();
        }
    }
}