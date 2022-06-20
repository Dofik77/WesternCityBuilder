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
using ECS.Views.WesterBuilderView;
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
            world.CreateResourceOnGround();
            world.CreateCampFire();
        }
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
            var views = Object.FindObjectsOfType<BuildsView>(true);

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
            entity.Get<ResourceComponent>();
            entity.Get<ResourceObjectUnavailable>();
            
            switch (resourceType)
            {
                case RequiredResourceType.WoodResource :
                    entity.Get<WoodResourceComponent>();
                    break;
                
                case RequiredResourceType.RockResource :
                    entity.Get<RockResourceComponent>();
                    break;
                
                case RequiredResourceType.FoodResource : 
                    break;
            }
           
            entity.GetAndFire<PrefabComponent>().Value = resourceType.ToString();
            entity.Get<EventMakeObjectAsChild>().Parent = point;
        }

        public static void CreateResourceOnGround(this EcsWorld world)
        {
            var views = Object.FindObjectsOfType<ResourceView>(true);

            foreach (var view in views)
            {
                var entity = world.NewEntity();
                entity.Get<UIdComponent>().Value = UidGenerator.Next();
                entity.Get<ResourceComponent>();

                switch (view.ResourceType)
                {
                    case RequiredResourceType.WoodResource :
                        entity.Get<WoodResourceComponent>();
                        break;
                    
                    case RequiredResourceType.RockResource :
                        entity.Get<RockResourceComponent>();
                        break;
                }
                entity.LinkView(view);
            }
        }

        public static void CreateRequestRecipe(this EcsWorld world, Recipe recipe)
        {
            var entity = world.NewEntity();
            entity.Get<UIdComponent>().Value = UidGenerator.Next();
            entity.GetAndFire<PrefabComponent>().Value = recipe.GetName();
            entity.GetAndFire<BuildUnderConstruction>().Recipe = recipe;
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