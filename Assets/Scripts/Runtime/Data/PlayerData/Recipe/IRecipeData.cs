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
        
        [Header("General")] 
        [SerializeField] private string _key;
        public string GetDependKey() => _dependKey;
        [SerializeField] private string _dependKey;
        
        [Header("Recipe")] 
        [SerializeField] private string _name;
        [SerializeField] private string _description;
        [SerializeField] private Sprite _recipeIcon;
        [SerializeField] private ResourceCount[] _recipeResources;
        [SerializeField] private string _levelOpenData;

        public string GetLevelOpenData() => _levelOpenData;
        public string GetName() => _name;
        public string GetDescription() => _description;
        public Sprite GetIcon() => _recipeIcon;
        public ResourceCount[] GetResourceCount() => _recipeResources;
       
        
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