using System;
using System.Collections.Generic;
using ECS.Core.Utils.SystemInterfaces;
using ECS.Game.Components.Flags;
using ECS.Game.Components.General;
using ECS.Game.Systems.View;
using ECS.Utils.Extensions;
using Leopotam.Ecs;
using Runtime.Game.Ui.Windows.TouchPad;
using Zenject;

namespace ECS
{
    public class EcsMainBootstrap : IInitializable, ITickable, IDisposable
    {
        private readonly EcsWorld _world;
        private readonly EcsSystems _initUpdateSystems;
        public EcsMainBootstrap(EcsWorld world,
            IList<IEcsUpdateSystem> updateSystems,
            IList<IEcsInitSystem> initSystems)
        {
            _world = world;
            _initUpdateSystems = new EcsSystems(_world);

            if (initSystems.Count > 0)
                _initUpdateSystems.AddRange(initSystems);
            
            if (updateSystems.Count > 0)
                _initUpdateSystems.AddRange(updateSystems);
            
            _initUpdateSystems.DeclareOneFrameEvents();
        }

        public void Initialize()
        {
#if UNITY_EDITOR
            Leopotam.Ecs.UnityIntegration.EcsSystemsObserver.Create (_initUpdateSystems);
            Leopotam.Ecs.UnityIntegration.EcsWorldObserver.Create (_world);
#endif
            //Create filters for use out of systems
            _world.GetFilter(typeof(EcsFilter<UIdComponent>));
            _world.GetFilter(typeof(EcsFilter<InputComponent>));
            _world.GetFilter(typeof(EcsFilter<CameraComponent>));
            _world.GetFilter(typeof(EcsFilter<LevelComponent>));
            _world.GetFilter(typeof(EcsFilter<PlayerComponent>));
            _world.GetFilter(typeof(EcsFilter<HasSkinComponent>));

            _initUpdateSystems?.Init();
        }

        public void Tick()
        {
            _initUpdateSystems?.Run();
        }

        public void Dispose()
        {
            _initUpdateSystems?.Destroy();
            _world?.Destroy();
        }
    }
}
