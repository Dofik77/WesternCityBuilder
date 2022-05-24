using Leopotam.Ecs;
using UnityEngine;

namespace ECS.Views.General
{
    public class NavMeshLinkView : LinkableView
    {
        [SerializeField] private float _duration;
        public ref AnimationCurve GetPath() => ref _path;
        [SerializeField] private AnimationCurve _path = new AnimationCurve();

        public override void Link(EcsEntity entity)
        {
            base.Link(entity);
            entity.Get<NavMeshLinkComponent>().Duration = _duration;
        }
    }

    public struct NavMeshLinkComponent
    {
        public float Duration;
    }
}