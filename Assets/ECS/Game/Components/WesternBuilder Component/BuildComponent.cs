using System.Numerics;

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