using ECS.Core.Utils.SystemInterfaces;
using ECS.Game.Components.General;
using Leopotam.Ecs;
using UnityEngine;

namespace ECS.Game.Systems.General
{
    public class TransformTranslateSystem : IEcsUpdateSystem
    {
        private readonly EcsFilter<LinkComponent, EventSetPositionComponent> _viewsPos;
        private readonly EcsFilter<LinkComponent, EventSetRotationComponent> _viewsRot;
        public void Run()
        {
            foreach (var i in _viewsPos)
            {
                _viewsPos.Get1(i).View.Transform.position = _viewsPos.Get2(i).Value;
                _viewsPos.GetEntity(i).Del<EventSetPositionComponent>();
            }
            foreach (var i in _viewsRot)
            {
                _viewsRot.Get1(i).View.Transform.rotation = _viewsRot.Get2(i).Value;
                _viewsRot.GetEntity(i).Del<EventSetRotationComponent>();
            }
        }
    }

    public struct EventSetPositionComponent
    {
        public Vector3 Value;
    }
    
    public struct EventSetRotationComponent
    {
        public Quaternion Value;
    }
}