using System;
using System.Collections.Generic;
using ECS.Core.Utils.ReactiveSystem;
using ECS.Core.Utils.SystemInterfaces;
using ECS.Game.Components.Flags;
using ECS.Game.Components.General;
using ECS.Game.Components.WesternBuilder_Component;
using ECS.Utils.Extensions;
using ECS.Views;
using ECS.Views.General;
using Leopotam.Ecs;
using UniRx;
using UnityEngine;
using Zenject;

namespace ECS.Game.Systems.WesternBuilder_System
{
    public class AssignmentOfTasksForUnitsSystem : ReactiveSystem<RecipeComponent> //eventaddcomponent
    {
       
        protected override EcsFilter<RecipeComponent> ReactiveFilter { get; }
        
        protected override void Execute(EcsEntity entity)
        {
            var recipe = entity.Get<RecipeComponent>();
            
            
        }
    }
    
    public struct RecipeComponent
    {
        public float WoodCount;
        public float RockCount;

        public Vector3 BuildPoint;
    }
}