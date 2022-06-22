using ECS.Core.Utils.SystemInterfaces;
using ECS.Game.Components.General;
using Leopotam.Ecs;
using UnityEngine;

namespace ECS.Game.Systems.WesternBuilder_System
{
    public class MakeObjectChildSystem : IEcsUpdateSystem
    {
        private readonly EcsFilter<EventMakeObjectAsChild, LinkComponent> _child;
        
        public void Run()
        {
            foreach (var i in _child)
            {
                
                _child.Get2(i).View.Transform.SetParent(_child.Get1(i).Parent);
                
                _child.Get2(i).View.Transform.position = _child.Get1(i).Parent.position;
                _child.Get2(i).View.Transform.rotation = _child.Get1(i).Parent.rotation;
                
                
                _child.GetEntity(i).Del<EventMakeObjectAsChild>();
            }
        }
    }

    public struct EventMakeObjectAsChild
    {
        public Transform Parent;
    }
}