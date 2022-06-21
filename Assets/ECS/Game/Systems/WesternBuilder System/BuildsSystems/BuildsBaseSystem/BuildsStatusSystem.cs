using System.Collections.Generic;
using ECS.Core.Utils.ReactiveSystem;
using ECS.Game.Components.Flags;
using ECS.Game.Components.General;
using ECS.Game.Components.WesternBuilder_Component;
using ECS.Views;
using ECS.Views.General;
using ECS.Views.WesterBuilderView;
using Leopotam.Ecs;
using Runtime.Signals;
using UnityEngine;
using Zenject;

namespace ECS.Game.Systems.WesternBuilder_System.BuildsSystems
{
    public class BuildsStatusSystem : ReactiveSystem<EventBuildUpdate>
    {
        [Inject] private SignalBus _signalBus;
        
        protected override EcsFilter<EventBuildUpdate> ReactiveFilter { get; }

        private readonly EcsFilter<BuildComponent, LinkComponent> _builds;
        private readonly EcsFilter<UnitComponent, LinkComponent> _units;

        private BuildsView _buildsView;
      
        private LinkableView _resourceView;

        private int _currentValue;
        
        protected override void Execute(EcsEntity entity)
        {
            _buildsView = entity.Get<UnitPriorityData>().TargetBuildsView;
            var unitView = entity.Get<LinkComponent>().View as UnitView;
            var buildsView = entity.Get<UnitPriorityData>().TargetBuildsView;
            
            var resourceCount = unitView.GetResourceStack().childCount; //размер коллекции

            if (buildsView.Entity.Has<BuildUnderConstruction>())
            {
                PutResourcesFromUnit(resourceCount, unitView);
                UpdateConstructionData(buildsView, entity);
                CheckConstructionStatus(buildsView.Entity, entity);
            }
            
            else if (buildsView.Entity.Has<BuildStorageComponent>())
            {
                PutResourcesFromUnit(resourceCount, unitView);
                UpdateStorageData(buildsView, entity);
                entity.Del<UnitHasPriority>();
                entity.Get<EventUpdatePriorityComponent>();
            }
        }
        
        private void PutResourcesFromUnit(int resourceCount, UnitView unitView)
        {
            for (int j = 0; j < resourceCount; j++)
            {
                unitView.GetResourceStack().GetChild(j).gameObject.GetComponent<LinkableView>().Entity
                    .Get<IsDestroyedComponent>();
                // коллекцию во вьюшке
            }
        }

        private void UpdateConstructionData(BuildsView build, EcsEntity unitEntity)
        {
            var requiredResourceToConstruct = build.Entity.Get<BuildUnderConstruction>().RequiredResourceToConstruct;
            for (int i = 0; i < requiredResourceToConstruct.Length; i++)
            {
                if (build.Entity.Get<BuildUnderConstruction>().RequiredResourceToConstruct[i].Key 
                    == unitEntity.Get<UnitPriorityData>().RequiredMining)
                {
                    build.Entity.Get<BuildUnderConstruction>().RequiredResourceToConstruct[i].NeedToConstruct 
                        -= unitEntity.Get<UnitCurrentResource>().Value;
                    UpdateProgressUI(build, unitEntity.Get<UnitCurrentResource>().Value);
                    unitEntity.Get<UnitCurrentResource>().Value = 0;
                }
            }
        }
        
        private void UpdateProgressUI(BuildsView build, int unitBrought)
        {
            build.UpdateProgressBar(unitBrought);
        }

        private void UpdateStorageData(BuildsView build, EcsEntity unitEntity)
        {
            var value = unitEntity.Get<UnitCurrentResource>().Value;
            unitEntity.Get<UnitCurrentResource>().Value = 0;
            build.Entity.Get<BuildStorageComponent>().CurrentResourceInStorage += value;
            build.Entity.Get<BuildStorageComponent>().PromisingResourceValue += value;
            _signalBus.Fire(new SignalStorageUpdate(build));
        }

        private void CheckConstructionStatus(EcsEntity buildEntity, EcsEntity unitEntity)
        {
            bool constructionIsDone = false;
            
            var requiredResourceToConstruct 
                = buildEntity.Get<BuildUnderConstruction>().RequiredResourceToConstruct;

            for (int i = 0; i < requiredResourceToConstruct.Length; i++)
            {
                if (requiredResourceToConstruct[i].NeedToConstruct == 0)
                    constructionIsDone = true;
                else
                {
                    constructionIsDone = false;
                    break;
                }
            }

            if (!constructionIsDone)
                unitEntity.Get<EventUpdatePriorityComponent>();
            else
            {
                buildEntity.Get<BuildConstruction>();

                foreach (var i in _units)
                    _units.GetEntity(i).Get<EventUnitChangeStateComponent>().State = UnitAction.Сonstruction;
            }
               


        }
    }

    public struct EventBuildUpdate
    {
    }
}