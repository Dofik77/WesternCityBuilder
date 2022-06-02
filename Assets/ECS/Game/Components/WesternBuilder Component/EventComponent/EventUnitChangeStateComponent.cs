using ECS.Views.General;
using ECS.Views.WesterBuilderView;
using UnityEngine;

namespace ECS.Game.Components.WesternBuilder_Component
{
    public struct EventUnitChangeStateComponent
    {
        public UnitAction State;
    }


    public enum RequiredResourceType
    {
        Default, 
        
        WoodResource,
        RockResource,
        OreResource,
        FoodResource
    }


    public enum UnitAction
    {
        Default,
        
        FollowAndSetState,
        
        AwaitNearCampFire,
        
        FetchResource,
        TakeResource,
        PutResource,
        
        MiningWood,
        FindNearestWood,
        CarryWood,
    }

    
}