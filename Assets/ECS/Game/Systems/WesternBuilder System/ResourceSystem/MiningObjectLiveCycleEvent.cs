using System;
using System.Collections.Generic;
using ECS.Core.Utils.ReactiveSystem;
using ECS.Core.Utils.SystemInterfaces;
using ECS.Game.Components.Flags;
using ECS.Game.Components.General;
using ECS.Game.Components.WesternBuilder_Component;
using ECS.Views;
using ECS.Views.General;
using Leopotam.Ecs;
using UnityEngine;

namespace ECS.Game.Systems.WesternBuilder_System.ResourceSystem
{
    public class MiningObjectLiveCycleEvent : ReactiveSystem<DisableMiningObject>
    {
        private readonly EcsFilter<ObjectMiningComponent, DisableMiningObject, DisableCounterComponent, LinkComponent> _disablesObject;

        protected override EcsFilter<DisableMiningObject> ReactiveFilter { get; }

        private EcsWorld _world;

        private float TreeDisabledMaxCounter = Mathf.Infinity;
        private float RockDisabledMaxCounter = Mathf.Infinity;
        protected override void Execute(EcsEntity entity)
        {
            var view = entity.Get<LinkComponent>().View as ObjectMiningView;
            view.gameObject.SetActive(false);
            view.Entity.Get<DisableCounterComponent>();

            //CheckDisablesCounter();
        }

        public void CheckDisablesCounter()
        {
            var TreeCounter = 0;
            var RockCounter = 0;

            ObjectMiningView MaxTreeDisabled = _disablesObject.Get4(1).View as ObjectMiningView;
            ObjectMiningView MaxRockDisabled =  _disablesObject.Get4(1).View as ObjectMiningView;
            

            foreach (var i in _disablesObject)
            {
                var view = _disablesObject.Get4(i).View as ObjectMiningView;

                switch (view.ResourceType)
                {
                    case RequiredResourceType.WoodResource :
                        TreeCounter++;
                        ChooseMaxDisableObject(view, MaxTreeDisabled);
                        break;
                    case RequiredResourceType.RockResource :
                        ChooseMaxDisableObject(view, MaxRockDisabled);
                        RockCounter++;
                        break;
                }
            }

            if (TreeCounter == 0)
            {
                MaxTreeDisabled.gameObject.SetActive(true);
                MaxTreeDisabled.GetCurrentResourceValue = MaxTreeDisabled.Entity.Get<ObjectMiningComponent>().MaxResourceValue;
                MaxTreeDisabled.Entity.Get<RemainingAmountResource>().Value = MaxTreeDisabled.Entity.Get<ObjectMiningComponent>().MaxResourceValue;
            
                MaxTreeDisabled.Entity.Del<DisableCounterComponent>();
                MaxTreeDisabled.Entity.Del<DisableMiningObject>();
            }
            
            else if (RockCounter == 0)
            {
                MaxRockDisabled.gameObject.SetActive(true);
                MaxRockDisabled.GetCurrentResourceValue = MaxRockDisabled.Entity.Get<ObjectMiningComponent>().MaxResourceValue;
                MaxRockDisabled.Entity.Get<RemainingAmountResource>().Value = MaxRockDisabled.Entity.Get<ObjectMiningComponent>().MaxResourceValue;
            
                MaxRockDisabled.Entity.Del<DisableCounterComponent>();
                MaxRockDisabled.Entity.Del<DisableMiningObject>();
            }
        }
        
        private void ChooseMaxDisableObject(ObjectMiningView currentView, ObjectMiningView maxViewDisabled)
        {
            if (TreeDisabledMaxCounter > currentView.Entity.Get<DisableCounterComponent>().Counter)
            {
                TreeDisabledMaxCounter = currentView.Entity.Get<DisableCounterComponent>().Counter;
                maxViewDisabled = currentView;
            }
        }
        
    }

    public struct DisableMiningObject
    {
      
    }
}