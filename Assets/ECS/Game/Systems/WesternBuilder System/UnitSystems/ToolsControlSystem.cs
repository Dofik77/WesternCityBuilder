using ECS.Core.Utils.ReactiveSystem;
using ECS.Game.Components.General;
using ECS.Game.Components.WesternBuilder_Component;
using ECS.Views.General;
using Leopotam.Ecs;

namespace ECS.Game.Systems.WesternBuilder_System
{
    public class ToolsControlSystem : ReactiveSystem<EventToolsControl>
    {
        protected override EcsFilter<EventToolsControl> ReactiveFilter { get; }
        protected override void Execute(EcsEntity entity)
        {
            var view = entity.Get<LinkComponent>().View as UnitView;
            
            switch (entity.Get<EventToolsControl>().ResourceType)
            {
                case RequiredResourceType.WoodResource :
                    view.Axe.SetActive(true);
                    break;
                
                case RequiredResourceType.RockResource :
                    view.Pick.SetActive(true);
                    break;
            }

            if (entity.Get<EventToolsControl>().Tools == Tools.None)
            {
                view.Axe.SetActive(false);
                view.Pick.SetActive(false);
            }
                
        }
    }


    public struct EventToolsControl
    {
        public RequiredResourceType ResourceType;
        public Tools Tools;
    }

    public enum Tools
    {
        Axe,
        Pick,
        None
    }
    
}