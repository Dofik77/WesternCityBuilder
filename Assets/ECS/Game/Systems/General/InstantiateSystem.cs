using ECS.Core.Utils.ReactiveSystem;
using ECS.Core.Utils.ReactiveSystem.Components;
using ECS.Game.Components.General;
using ECS.Utils;
using ECS.Views.General;
using Leopotam.Ecs;
using Zenject;

namespace ECS.Game.Systems.General
{
    public class InstantiateSystem : ReactiveSystem<EventAddComponent<PrefabComponent>>
    {
        [Inject] private readonly ISpawnService<EcsEntity, ILinkable> _spawnService;
        protected override EcsFilter<EventAddComponent<PrefabComponent>> ReactiveFilter { get; }
        protected override void Execute(EcsEntity entity)
        {
            var linkable = _spawnService.SpawnPrefab(entity);
            linkable?.Link(entity);
            entity.Get<LinkComponent>().View = linkable;
        }
    }
}