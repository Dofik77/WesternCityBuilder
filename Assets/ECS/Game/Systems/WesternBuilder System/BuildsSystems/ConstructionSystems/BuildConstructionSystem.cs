using ECS.Core.Utils.ReactiveSystem;
using ECS.Game.Components.Flags;
using ECS.Game.Components.General;
using ECS.Game.Components.WesternBuilder_Component;
using ECS.Views;
using Leopotam.Ecs;
using Runtime.Data.PlayerData.Recipe;
using Runtime.Services.DelayService;
using Zenject;

namespace ECS.Game.Systems.WesternBuilder_System.BuildsSystems
{
    public class BuildConstructionSystem : ReactiveSystem<BuildConstruction>
    {
        [Inject] private IDelayService _delayService;
        
        private readonly EcsFilter<UnitComponent, LinkComponent> _units;
        protected override EcsFilter<BuildConstruction> ReactiveFilter { get; }
        protected override void Execute(EcsEntity entity)
        {
            var buildView = entity.Get<LinkComponent>().View as BuildsView;
            var constructionValue = entity.Get<BuildUnderConstruction>().Recipe.GetResourceCount();
            var delayToConstruct = CalculatedTimeForConstruct(constructionValue);
            
            //UpdateConstructUI(buildView); - in another delay
            _delayService.Do(delayToConstruct, () =>
            {
                entity.Del<BuildUnderConstruction>();
                entity.Get<BuildComponent>();

                buildView.BaseObject.SetActive(false);
                buildView.ConstructedObject.SetActive(true);
                

                foreach (var i in _units)
                    _units.GetEntity(i).Get<EventUpdatePriorityComponent>();
            });


            //постройка = кол-во ресурсов необходимых для постройки ( уже принесенных ).секунд
            //по завершению удаляем BuildUnderConstruction, даём BuildComp, и обновляем приоритет
            //( обычно идут чилить / заполнять склады ) 
        }

        private int CalculatedTimeForConstruct(RequiredResourceCount[] resourceCounts)
        {
            int TimeForBuild = 0;
            
            foreach (var resource in resourceCounts)
            {
                TimeForBuild += resource.NeedToConstruct;
            }
            return TimeForBuild;
        }

        private void UpdateConstructUI(BuildsView build)
        {
            
        }
    }


    public struct BuildConstruction
    {
        
    }
}