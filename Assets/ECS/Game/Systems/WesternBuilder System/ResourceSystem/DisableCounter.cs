using ECS.Core.Utils.ReactiveSystem;
using ECS.Core.Utils.SystemInterfaces;
using ECS.Game.Components.General;
using ECS.Game.Components.WesternBuilder_Component;
using ECS.Views;
using Leopotam.Ecs;
using UnityEngine;

namespace ECS.Game.Systems.WesternBuilder_System.ResourceSystem
{
    public class DisableCounterSystem : IEcsUpdateSystem
    {
        private readonly EcsFilter<DisableCounterComponent, LinkComponent> _disableCounter;
        
        public void Run()
        {
            foreach (var i in _disableCounter)
            {
                _disableCounter.Get1(i).Counter += Time.deltaTime;

                if (_disableCounter.Get1(i).Counter > 50.0f)
                {
                    var view = _disableCounter.Get2(i).View as ObjectMiningView;
                    ReWriteObjectData(view);
                }
            }
        }

        private void ReWriteObjectData(ObjectMiningView view)
        {
            view.gameObject.SetActive(true);
            view.GetCurrentResourceValue = view.Entity.Get<ObjectMiningComponent>().MaxResourceValue;
            view.Entity.Get<RemainingAmountResource>().Value = view.Entity.Get<ObjectMiningComponent>().MaxResourceValue;
            
            view.Entity.Del<DisableCounterComponent>();
            view.Entity.Del<DisableMiningObject>();
        }
    }

    public struct DisableCounterComponent
    {
        public float Counter;
    }
}