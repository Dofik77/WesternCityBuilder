using System;
using System.Diagnostics.CodeAnalysis;
using ECS.Core.Utils.ReactiveSystem.Components;
using ECS.Core.Utils.SystemInterfaces;
using Leopotam.Ecs;

namespace ECS.Core.Utils.ReactiveSystem
{
    public abstract class ReactiveSystem<T> : IEcsUpdateSystem where T : struct
    {
        protected abstract EcsFilter<T> ReactiveFilter { get; }
        protected virtual bool EntityFilter(EcsEntity entity) => true;
        protected virtual bool DeleteEvent => true;
        public void Run()
        {
            foreach (var i in ReactiveFilter)
            {
                var entity = ReactiveFilter.GetEntity(i);
                if (EntityFilter(entity))
                    Execute(entity);
                if(DeleteEvent)
                    entity.Del<T>();
            }
        }
        protected abstract void Execute(EcsEntity entity);
    }
    
    public abstract class ReactiveSystem<T1, T2> : IEcsUpdateSystem 
        where T1 : struct, IEventAddComponent
        where T2 : struct, IEventRemoveComponent
    {
        protected abstract EcsFilter<T1> ReactiveFilter { get; }
        protected abstract EcsFilter<T2> ReactiveFilter2 { get; }
        protected virtual bool EntityFilter(EcsEntity entity) => true;
        protected virtual bool DeleteEvent => true;
        public void Run()
        {
            foreach (var i in ReactiveFilter)
            {
                var entity = ReactiveFilter.GetEntity(i);
                if (EntityFilter(entity))
                    Execute(entity, true);
                if(DeleteEvent)
                    entity.Del<T1>();
            }
            
            foreach (var i in ReactiveFilter2)
            {
                var entity = ReactiveFilter2.GetEntity(i);
                if (EntityFilter(entity))
                    Execute(entity, false);
                if(DeleteEvent)
                    entity.Del<T2>();
            }
        }
        protected abstract void Execute(EcsEntity entity, bool added);
    }
}