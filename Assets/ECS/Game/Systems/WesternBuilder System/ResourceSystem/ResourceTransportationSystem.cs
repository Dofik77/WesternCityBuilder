using DG.Tweening;
using ECS.Core.Utils.ReactiveSystem;
using ECS.Core.Utils.SystemInterfaces;
using ECS.Game.Components.Flags;
using ECS.Game.Components.General;
using ECS.Game.Components.WesternBuilder_Component.dotween_components;
using ECS.Views.WesterBuilderView;
using Leopotam.Ecs;

namespace ECS.Game.Systems.WesternBuilder_System.ResourceSystem
{
    public class ResourceTransportationSystem : ReactiveSystem<ResourceTravel>
    {
        protected override EcsFilter<ResourceTravel> ReactiveFilter { get; }
        protected override void Execute(EcsEntity entity)
        {
            var unitView = entity.Get<ResourceTravel>().UnitView;
            var resourceView = entity.Get<LinkComponent>().View as ResourceView;
            
            resourceView.transform.DOKill();
            
            resourceView.transform.SetParent(unitView.GetResourceStack());
            
            resourceView.Transform.DOLocalMove(entity.Get<Vector3Component<MoveTweenEventComponent>>().Value,
                unitView.GetInteractionDuration()).SetEase(Ease.Linear);

            resourceView.transform.rotation = unitView.GetResourceStack().transform.rotation;


        }
    }
}