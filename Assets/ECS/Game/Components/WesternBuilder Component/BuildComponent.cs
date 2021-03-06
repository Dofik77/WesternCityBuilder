using System;
using System.Numerics;
using ECS.Game.Systems.WesternBuilder_System.StateMachine;
using ECS.Views;
using Runtime.Data;
using Runtime.Data.PlayerData.Recipe;

namespace ECS.Game.Components.WesternBuilder_Component
{
    public struct BuildComponent
    {
        public string Value;
    }

    public struct ExpectedValueResource
    {
        public ResourceTypeValuePair ResourceTypeValuePair;
        
        public int ExpectedValue;
    }

    public struct ExpectedRecipeResource
    {
        public ResourceTypeValuePair[] ResourceTypeValuePair;

        public int ExpectedValue;
    }

    public struct CurrentBuildResource
    {
        public ResourceTypeValuePair[] ResourceTypeValuePair;

        public int ExpectedValue;
    }
    
    public struct ResourceTypeValuePair : IHasEnumKey
    {
        public ResourceTypeValuePair(RequiredResourceType key, int value)
        {
            Key = key;
            Value = value;
        }
        

        public RequiredResourceType Key;
        public int Value;
        
        public Enum GetKey()
        {
            return Key;
        }
    }
    

    public struct BuildUnderConstruction
    {
        public Recipe Recipe;
        
        public BuildsView BuildsView;
        public RequiredResourceCount[] RequiredRecipeResource;
        public RequiredResourceCount[] RequiredResourceToConstruct;
    }
    
    public struct BuildCampFireComponent
    {
        public string Value;
    }
    
    public struct BuildWoodStorageComponent
    {
        
    }

    public struct BuildRockStorageComponent
    {
        
    }

    public struct BuildStorageComponent
    {
        public int MaxResource;
        
        public IsStorageOff IsStorageOff;

        public int CurrentResourceInStorage;
        public int PromisingResourceValue;
        
        public int LeftToCollectResourceCount;
    }



}