using ECS.Core.Utils.ReactiveSystem;
using ECS.Core.Utils.ReactiveSystem.Components;
using ECS.Game.Components.General;
using ECS.Views.General;
using Leopotam.Ecs;

namespace ECS.Game.Systems.View
{
    public class HoverSystem : ReactiveSystem<EventAddComponent<HoverComponent>, EventRemoveComponent<HoverComponent>>
    {
        protected override EcsFilter<EventAddComponent<HoverComponent>> ReactiveFilter { get; }
        protected override EcsFilter<EventRemoveComponent<HoverComponent>> ReactiveFilter2 { get; }

        private IHover _view;

        protected override void Execute(EcsEntity entity, bool added)
        {
            _view = entity.Get<LinkComponent>().Get<IHover>();
            if (added)
                _view.SetHover();
            else
                _view.ClearHover();
        }
    }

    public struct HoverComponent : IEcsIgnoreInFilter
    {
    }
    
    public interface IHover : ILinkable
    {
        void SetHover();

        void ClearHover();
    }
}