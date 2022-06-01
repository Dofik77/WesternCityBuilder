using System.Numerics;
using Runtime.Data.PlayerData.Recipe;

namespace ECS.Game.Components.WesternBuilder_Component
{
    public struct BuildComponent
    {
        public string Value;
    }

    public struct ExpectedAmountOfResource
    {
        public int ExpectedValue;
    }

    public struct BuildUnderConstruction
    {
        public Recipe Recipe;

        public int CurrentResourceCollected;
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
        public int CurrentResource;
    }



}