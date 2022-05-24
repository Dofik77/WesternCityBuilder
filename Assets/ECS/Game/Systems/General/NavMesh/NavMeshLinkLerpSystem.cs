using ECS.Core.Utils.ElapsedTimeSystem;
using ECS.Game.Components.General;
using ECS.Utils.Extensions;
using ECS.Views.General;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.AI;

namespace ECS.Game.Systems.General.NavMesh
{
    public class NavMeshLinkLerpSystem : ElapsedTimeSystem<EventOnNavMeshLinkComponent>
    {
        protected override EcsFilter<EventOnNavMeshLinkComponent, ElapsedTimeComponent<EventOnNavMeshLinkComponent>>
            ElapsedTimeFilter { get; }

        private float _elapsedTime;
        private float _normalizedTime;
        private IHasNavMeshAgent _view;
        private NavMeshAgent _agent;

        protected override void Execute(EcsEntity entity)
        {
            _view = entity.Get<LinkComponent>().Get<IHasNavMeshAgent>();

            ref var eventLink = ref entity.Get<EventOnNavMeshLinkComponent>();
            
            _agent = _view.GetAgent();
            _elapsedTime = entity.Get<ElapsedTimeComponent<EventOnNavMeshLinkComponent>>().Value;
            _normalizedTime = _elapsedTime / eventLink.LinkView.Entity.Get<NavMeshLinkComponent>().Duration;

            _view.Transform.position =
                Vector3.Lerp(eventLink.From, eventLink.To + Vector3.up * _agent.baseOffset, _normalizedTime) +
                eventLink.LinkView.GetPath().Evaluate(_normalizedTime) * Vector3.up;

            if (_normalizedTime >= 1.0f)
            {
                entity.DelTimer<EventOnNavMeshLinkComponent>();
                _agent.CompleteOffMeshLink();
            }
        }
    }

    public interface IHasNavMeshAgent : ILinkable
    {
        ref NavMeshAgent GetAgent();
    }
    
    public struct EventOnNavMeshLinkComponent
    {
        public NavMeshLinkView LinkView;
        public Vector3 From;
        public Vector3 To;
    }
}