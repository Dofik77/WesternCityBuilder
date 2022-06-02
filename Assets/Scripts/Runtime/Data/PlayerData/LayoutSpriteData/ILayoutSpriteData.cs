using System;
using Runtime.Data.PlayerData.Currency;
using UnityEngine;

namespace Runtime.Data.PlayerData.LayoutSpriteData
{
    public interface ILayoutSpriteData
    {
        LayoutSprite Get(ELayoutSprite type);
        LayoutSprite[] Get();
    }
    
    [Serializable]
    public struct LayoutSprite
    {
        [Header("General")]
        public ELayoutSprite Type;
        
        [Header("Data")]
        public string Name;
        public GameObject Icon;
    }
    
    
    public enum ELayoutSprite
    {
        WoodResource,
        RockResource,
        OreResource,
        FoodResource
    }
}