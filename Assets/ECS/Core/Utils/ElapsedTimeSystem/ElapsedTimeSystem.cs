using ECS.Core.Utils.SystemInterfaces;
using Leopotam.Ecs;
using UnityEngine;

namespace ECS.Core.Utils.ElapsedTimeSystem
{
    public abstract class ElapsedTimeSystem<T> : IEcsUpdateSystem where T : struct
    {
        protected abstract EcsFilter<T, ElapsedTimeComponent<T>> ElapsedTimeFilter { get; }
        public void Run()
        {
            foreach (var i in ElapsedTimeFilter)
            {
                ElapsedTimeFilter.Get2(i).Value += Time.deltaTime;
                Execute(ElapsedTimeFilter.GetEntity(i));
            }
        }
        protected abstract void Execute(EcsEntity entity);
    }
    
    public struct ElapsedTimeComponent<T>
    {
        public float Value;
    }
}