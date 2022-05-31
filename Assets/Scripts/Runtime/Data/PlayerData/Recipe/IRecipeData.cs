using System;
using ECS.Game.Components.WesternBuilder_Component;
using Runtime.Data.PlayerData.Currency;
using Runtime.Game.Ui.Objects.Layouts;
using UnityEngine;

namespace Runtime.Data.PlayerData.Recipe
{
    public interface IRecipeData
    {
        Recipe[] Get();
    }
    
    [Serializable]
    public struct Recipe : IProvideUiGeneratedEntity
    {
        public string GetLowerKey() => _key.ToLower();
        
        [Header("General")] [SerializeField] private string _key;
        public string GetDependKey() => _dependKey;
        [SerializeField] private string _dependKey;
        public string GetName() => _name;
        
        [Header("Recipe")] [SerializeField] private string _name;
        
        public Sprite GetIcon() => _recipeIcon;
        [SerializeField] private Sprite _recipeIcon;

        public ResourceCount[] GetResourceCount() => _recipeResources;
        [SerializeField] private ResourceCount[] _recipeResources;
        
        public ECurrency GetCurrency()
        {
            return ECurrency.Hard;
        }
        
        public int GetCost()
        {
            return 0;
        }
    }
    
    
    [Serializable]
    public struct ResourceCount
    {
        public RequiredResourceType Key;
        public int Value;
    }
}