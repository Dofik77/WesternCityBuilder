using ECS.Core.Utils.ReactiveSystem;
using ECS.Game.Components.General;
using ECS.Views.General;
using Leopotam.Ecs;
using Runtime.Data.PlayerData.Skins;

namespace ECS.Game.Systems.View
{
    public class SkinEquipSystem : ReactiveSystem<EventSkinEquipComponent>
    {
        protected override EcsFilter<EventSkinEquipComponent> ReactiveFilter { get; }
        protected override void Execute(EcsEntity entity)
        {
            ref var eventComponent = ref entity.Get<EventSkinEquipComponent>();
            entity.Get<HasSkinComponent>().Key = eventComponent.Value.GetLowerKey();
            entity.Get<LinkComponent>().Get<IHasSkin>().EquipSkin(eventComponent.Value);
        }
    }

    public struct EventSkinEquipComponent
    {
        public Skin Value;
    }

    public struct HasSkinComponent
    {
        public string Key;
    }

    public interface IHasSkin : ILinkable
    {
        void EquipSkin(Skin skin);
    }
}