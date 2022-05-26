
using ECS.Game.Systems.General;
using ECS.Game.Systems.General.Camera;
using ECS.Game.Systems.General.Game;
using ECS.Game.Systems.General.Level;
using ECS.Game.Systems.General.NavMesh;
using ECS.Game.Systems.General.Player;
using ECS.Game.Systems.View;
using ECS.Game.Systems.WesternBuilder_System;
using ECS.Game.Systems.WesternBuilder_System.BuildsSystems;
using ECS.Game.Systems.WesternBuilder_System.ResourceSystem;
using ECS.Game.Systems.WesternBuilder_System.StateMachine;
using ECS.Game.Systems.WesternBuilder_System.Stats;
using ECS.Game.Systems.WesternBuilder_System.Stats.StateMachine;
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


            //West Isle Builder Systems 
            Container.BindInterfacesAndSelfTo<SetAnimationSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<BuildingsInitSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<BuildStorageInitSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<MiningResourceObjectInitSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<UnitInitSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<SetBaseValuesOfUnitSkill>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<UnitPrioritySystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<UnitStateMachine>().AsSingle();
            Container.BindInterfacesAndSelfTo<AssignmentOfTasksForUnitsSystem>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<FindClosestResourceObjectSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<CreateAndTransferRequestResourceSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<DistanceControlSystem>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<BuildsStatusSystem>().AsSingle();
            //West Isle Builder Systems 
            
            Container.BindInterfacesAndSelfTo<CameraInitSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<CameraResizeSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<LevelStartSystem>().AsSingle();
            
            
            Container.BindInterfacesAndSelfTo<TransformTranslateSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<MakeObjectChildSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<NavMeshDestanationSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerInputSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<TriggersDistanceSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<TriggerActivateSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<RaycastSystem>().AsSingle();
            
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