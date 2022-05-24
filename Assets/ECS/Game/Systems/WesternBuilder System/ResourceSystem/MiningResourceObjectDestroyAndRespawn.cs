using System;
using ECS.Core.Utils.ReactiveSystem;
using ECS.Core.Utils.SystemInterfaces;
using ECS.Game.Components.Flags;
using ECS.Game.Components.General;
using ECS.Game.Components.WesternBuilder_Component;
using ECS.Views;
using Leopotam.Ecs;

namespace ECS.Game.Systems.WesternBuilder_System.ResourceSystem
{
    public class MiningResourceObjectDestroyAndRespawn : IEcsUpdateSystem
    {
        private EcsFilter<MiningResourceObjectDestroyComponent> _resourceDestroy { get; }


        public void Run()
        {
            foreach (var i in _resourceDestroy)
            {
                var objectToDestroy = _resourceDestroy.GetEntity(i).Get<LinkComponent>().View as ObjectMiningView;
                
                // if (objectToDestroy.Entity.Get<RemainingAmountResource>().Value == 0)
                //     objectToDestroy.Entity.Get<IsDestroyedComponent>();
            }
        }
    }

    public struct MiningResourceObjectDestroyComponent
    {
    }
}