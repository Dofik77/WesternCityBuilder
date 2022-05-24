using System.Collections.Generic;
using ECS.Core.Utils.ReactiveSystem;
using ECS.Game.Components.Flags;
using ECS.Game.Components.General;
using ECS.Game.Components.WesternBuilder_Component;
using ECS.Views;
using ECS.Views.General;
using ECS.Views.WesterBuilderView;
using Leopotam.Ecs;
using UnityEngine;
using Zenject;

namespace ECS.Game.Systems.WesternBuilder_System.BuildsSystems
{
    public class BuildsStatusSystem : ReactiveSystem<EventBuildUpdate>
    {
        [Inject] private SignalBus _signalBus;
        protected override EcsFilter<EventBuildUpdate> ReactiveFilter { get; }

        private readonly EcsFilter<BuildComponent, LinkComponent> _builds;

        private BuildsView _buildsView;
        private UnitView _unitView;
        private LinkableView _resourceView;

        private int _currentValue;
        
        
        protected override void Execute(EcsEntity entity)
        {
            _buildsView = entity.Get<UnitPriorityData>().TargetBuildsView;
            var unitView = entity.Get<LinkComponent>().View as UnitView;

            foreach (var i in _builds)
            {
                var buildsView = _builds.Get2(i).View as BuildsView;
                
                if (buildsView.ObjectType == _buildsView.ObjectType)
                {
                    var currentResource = buildsView.CurrentResource;
                    var resourceCount = unitView.GetTransformPoint().childCount; //размер коллекции
                    var maxStorage = buildsView.Entity.Get<BuildStorageComponent>().MaxResource;
                    
                    for (int j = 0; j < resourceCount; j++)
                    {
                        unitView.GetTransformPoint().GetChild(j).gameObject.GetComponent<LinkableView>().Entity
                            .Get<IsDestroyedComponent>(); // коллекцию во вьюшке
                    }

                    currentResource += resourceCount;
                    buildsView.CurrentResource = currentResource;
                    buildsView.UpdateScore(buildsView.CurrentResource, maxStorage);
                }
            }
            
            entity.Get<EventUpdatePriorityComponent>();
            
            //UI который будет привязан к постройке 
            //обновлять данные в постройке 
            //визуализация тоже в постройке 
        }
    }

    public struct EventBuildUpdate
    {
    }
}