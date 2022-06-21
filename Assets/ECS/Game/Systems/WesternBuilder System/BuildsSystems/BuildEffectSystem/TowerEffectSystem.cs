using ECS.Core.Utils.ReactiveSystem;
using ECS.Game.Components.Flags;
using ECS.Game.Components.General;
using ECS.Utils.Extensions;
using ECS.Views.General;
using Leopotam.Ecs;

namespace ECS.Game.Systems.WesternBuilder_System.BuildsSystems.BuildEffectSystem
{
    public class TowerEffectSystem : ReactiveSystem<EventTowerEffect>
    {
        private readonly EcsFilter<CameraComponent, LinkComponent> _camera;
        protected override EcsFilter<EventTowerEffect> ReactiveFilter { get; }

        private EcsWorld _world;
        protected override void Execute(EcsEntity entity)
        {
            foreach (var camera in _camera)
            {
                var cameraView = _camera.Get2(camera).View as CameraView;
                cameraView.LerpCamera();
            }

            _world.CreateUnit();
        }
    }

    public struct EventTowerEffect
    {
        
    }
    
    
}