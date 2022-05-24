
using ECS.Game.Systems.General;
using ECS.Game.Systems.General.Camera;
using ECS.Game.Systems.General.Game;
using ECS.Game.Systems.General.Level;
using ECS.Game.Systems.General.Player;
using ECS.Game.Systems.View;
using Leopotam.Ecs;
using Runtime.Game.Utils.MonoBehUtils;
using UnityEngine;
using Zenject;

namespace ECS.Installers
{
    public class EcsInstaller : MonoInstaller
    {
        [SerializeField] private ScreenVariables _screenVariables;
        public override void InstallBindings()
        {
            Container.Bind<ScreenVariables>().FromInstance(_screenVariables).AsSingle();
            Container.BindInterfacesAndSelfTo<EcsWorld>().AsSingle().NonLazy();
            BindSystems();
            Container.BindInterfacesTo<EcsMainBootstrap>().AsSingle();
        }

        private void BindSystems()
        {
            Container.BindInterfacesAndSelfTo<GameInitializeSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<InstantiateSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<LevelTimerSystem>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<CameraInitSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<CameraResizeSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<LevelStartSystem>().AsSingle();
            
            
            Container.BindInterfacesAndSelfTo<TransformTranslateSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerInputSystem>().AsSingle();
            
            // Container.BindInterfacesAndSelfTo<NavMeshLinkListener>().AsSingle();
            // Container.BindInterfacesAndSelfTo<NavMeshLinkLerpSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<TriggersDistanceSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<TriggerActivateSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<RaycastSystem>().AsSingle();

            Container.BindInterfacesAndSelfTo<SkinEquipSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<HoverSystem>().AsSingle();

            Container.BindInterfacesAndSelfTo<LevelStateSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerInputCleanUpSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<DelayCleanUpSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<LevelEndSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<GamePauseSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<SaveGameSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameStageSystem>().AsSingle();        //always must been last
            Container.BindInterfacesAndSelfTo<CleanUpSystem>().AsSingle();          //must been latest than last!
        }       
    }
}