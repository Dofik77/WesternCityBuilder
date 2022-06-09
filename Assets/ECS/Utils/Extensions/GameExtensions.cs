using ECS.Core.Utils.ReactiveSystem.Components;
using ECS.Game.Components;
using ECS.Game.Components.Flags;
using ECS.Game.Components.General;
using ECS.Game.Components.WesternBuilder_Component;
using ECS.Game.Systems.General;
using ECS.Game.Systems.General.Level;
using ECS.Game.Systems.WesternBuilder_System;
using ECS.Game.Systems.WesternBuilder_System.StateMachine;
using ECS.Views;
using ECS.Views.General;
using Leopotam.Ecs;
using Runtime.Data.PlayerData.Recipe;
using Runtime.Game.Ui.Windows.TouchPad;
using Runtime.Services.Uid;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ECS.Utils.Extensions
{
    public static class GameExtensions
    {
        public static void CreateEcsEntities(this EcsWorld world)
        {
            world.CreateInput();
            world.CreateLevelEntity();
            world.CreateCamera();
            world.CreateDistanceTriggers();
            world.CreateNavMeshLinks();
            
            world.CreateUnit();
            world.CreateUnit();
            world.CreateUnit();

            world.CreateUnitSkillScoresEntity();
            world.CreateResourceMining();
            world.CreateCampFire();
        }
        
        //разделить на два класса?
        //один вызывает все методы при инициализации игры
        //другой вызывает методы в runtime

        public static EcsEntity GetInput(this EcsWorld world)
        {
            return world.GetEntity<InputComponent>();
        }

        public static void CreateInput(this EcsWorld world)
        {
            var entity = world.NewEntity();
            entity.Get<InputComponent>();
        }

        public static void CreateLevelEntity(this EcsWorld world)
        {
            var entity = world.NewEntity();
            entity.Get<LevelComponent>();
            entity.Get<LevelStateComponent>().State = ELevelState.Start;
            entity.Get<LevelDataComponent>();
        }

        public static void CreateCamera(this EcsWorld world)
        {
            var entity = world.NewEntity();
            entity.Get<UIdComponent>().Value = UidGenerator.Next();
            entity.GetAndFire<CameraComponent>();
            entity.GetAndFire<PrefabComponent>().Value = "Camera";
        }

        public static void CreateUnitSkillScoresEntity(this EcsWorld world)
        {
            var entity = world.NewEntity();
            entity.GetAndFire<UnitsSkillScoreComponent>();
        }

        public static void CreateUnit(this EcsWorld world)
        {
            //TODO - skin, posOfStart, etc...
            var entity = world.NewEntity();
            entity.Get<UIdComponent>().Value = UidGenerator.Next();
            entity.GetAndFire<UnitComponent>();
            entity.GetAndFire<PrefabComponent>().Value = "Player";
            entity.Get<EventUnitChangeStateComponent>();
        }
        
        public static void CreateResourceMining(this EcsWorld world)
        {
            var views = Object.FindObjectsOfType<ObjectMiningView>();

            foreach (var view in views)
            {
                var entity = world.NewEntity();
                entity.GetAndFire<ObjectMiningComponent>();
                entity.Get<UIdComponent>().Value = UidGenerator.Next();
                entity.LinkView(view);
            }
        }
        
        public static void CreateCampFire(this EcsWorld world)
        {
            var views = Object.FindObjectsOfType<BuildsView>(true); // сделать под костёр уникальную View

            foreach (var view in views)
            {
                var entity = world.NewEntity();
                entity.GetAndFire<BuildCampFireComponent>();
                entity.Get<UIdComponent>().Value = UidGenerator.Next();
                entity.LinkView(view);
            }
        }
        
        public static void CreateResourceType(this EcsWorld world, Transform point, RequiredResourceType resourceType)
        {
            var entity = world.NewEntity();
            entity.Get<UIdComponent>().Value = UidGenerator.Next();
            entity.GetAndFire<WoodLogComponent>();
            entity.GetAndFire<PrefabComponent>().Value = resourceType.ToString();
            entity.Get<EventMakeObjectAsChild>().Parent = point;
        }

        public static void CreateRequestRecipe(this EcsWorld world, Recipe recipe)
        {
            var entity = world.NewEntity();
            entity.Get<UIdComponent>().Value = UidGenerator.Next();
            entity.GetAndFire<PrefabComponent>().Value = recipe.GetName();
            entity.GetAndFire<BuildUnderConstruction>().Recipe = recipe;
        }

        public static void UpdateRequestRecipe(this EcsWorld world, Recipe recipe)
        {
            
        }
        
        public static void CreateDistanceTriggers(this EcsWorld world)
        {
            var views = Object.FindObjectsOfType<DistanceTriggerView>(true);
            foreach (var view in views)
            {
                var entity = world.NewEntity();
                entity.Get<UIdComponent>().Value = UidGenerator.Next();
                entity.Get<DistanceTriggerComponent>();
                entity.LinkView(view);
            }
        }

        public static void CreateNavMeshLinks(this EcsWorld world)
        {
            var views = Object.FindObjectsOfType<NavMeshLinkView>(true);
            foreach (var view in views)
            {
                var entity = world.NewEntity();
                entity.Get<UIdComponent>().Value = UidGenerator.Next();
                entity.LinkView(view);
            }
        }
    }
}

// public static void CreateWoodStorage(this EcsWorld world)
// {
//     var entity = world.NewEntity();
//     entity.Get<UIdComponent>().Value = UidGenerator.Next();
//     //entity.GetAndFire<BuildComponent>().Value = "Storage";
//     entity.GetAndFire<PrefabComponent>().Value = "Storage";
//             
//     entity.Get<BuildWoodStorageComponent>();
//
//     //единый метод что сохранит enum
//     entity.GetAndFire<BuildStorageComponent>();
//     entity.Get<BuildStorageComponent>().MaxResource = 10;
//     entity.Get<BuildStorageComponent>().CurrentResource = 0;
// }
//         
// public static void CreateRockStorage(this EcsWorld world)
// {
//     var entity = world.NewEntity();
//     entity.Get<UIdComponent>().Value = UidGenerator.Next();
//     //entity.GetAndFire<BuildComponent>().Value = "RockStorage";
//     entity.GetAndFire<PrefabComponent>().Value = "RockStorage";
//             
//     entity.Get<BuildRockStorageComponent>();
//             
//     entity.GetAndFire<BuildStorageComponent>();
//     entity.Get<BuildStorageComponent>().MaxResource = 5;
//     entity.Get<BuildStorageComponent>().CurrentResource = 0;
// }