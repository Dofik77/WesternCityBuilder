using System;
using Leopotam.Ecs;
using Runtime.Data;

namespace ECS.Game.Components.WesternBuilder_Component
{
    public struct UnitsSkillScoreComponent
    {
        public int Speed;
        
        public int SpeedOfWoodsMine;
        public int SpeedOfRockMine; 
        public int SpeedOfCraft;
            
        public int ValueOfRock;
        public int ValueOfWoods;
        
        //разделить на 3 компонента и на 3 массива???

        public UnitSkillOfMine[] SkillsOfMine;
        public UnitSkillOfPortability[] SkillOfPortability;

    }
    
    public struct UnitSkillOfPortability : IHasEnumKey
    {
        public UnitSkillOfPortability(RequiredResourceType key, int skill)
        {
            Key = key;
            Skill = skill;
        }
        

        public RequiredResourceType Key;
        public int Skill;
        
        public Enum GetKey()
        {
            return Key;
        }
    }


    public struct UnitSkillOfMine : IHasEnumKey
    {
        public UnitSkillOfMine(RequiredResourceType key, int skill)
        {
            Key = key;
            Skill = skill;
        }
        

        public RequiredResourceType Key;
        public int Skill;
        
        public Enum GetKey()
        {
            return Key;
        }
    }
}