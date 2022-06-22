using ECS.Core.Utils.ReactiveSystem;
using ECS.Game.Components.Flags;
using ECS.Game.Components.General;
using ECS.Game.Components.WesternBuilder_Component;
using ECS.Game.Systems.General.NavMesh;
using ECS.Game.Systems.WesternBuilder_System.StateMachine;
using ECS.Utils.Extensions;
using ECS.Views;
using ECS.Views.General;
using Leopotam.Ecs;

namespace ECS.Game.Systems.WesternBuilder_System.BuildsSystems.BuildEffectSystem
{
    public class TowerEffectSystem : ReactiveSystem<EventTowerEffect>
    {
        private readonly EcsFilter<CameraComponent, LinkComponent> _camera;
        private readonly EcsFilter<BuildCampFireComponent, LinkComponent> _campFire;
        protected override EcsFilter<EventTowerEffect> ReactiveFilter { get; }

        private EcsWorld _world;
        private EcsEntity _unitEntity;

        private BuildsView _campFireView;
        protected override void Execute(EcsEntity entity)
        {
            ChangeCameraView();
            CreateUnit();
        }

        private void ChangeCameraView()
        {
            foreach (var camera in _camera)
            {
                var cameraView = _camera.Get2(camera).View as CameraView;
                cameraView.LerpCamera();
            }
        }

        private void CreateUnit()
        {
            _unitEntity = _world.CreateUnit();

            foreach (var i in _campFire)
                _campFireView = _campFire.Get2(i).View as BuildsView;
            
            _unitEntity.Get<EventSetDestinationComponent>().DistanceControlObjectView = _campFireView;
            _unitEntity.Get<EventControlDistanceToSetState>().DistanceObjectView = _campFireView;
        }
    }

    public struct EventTowerEffect
    {
        
    }
    
    
}