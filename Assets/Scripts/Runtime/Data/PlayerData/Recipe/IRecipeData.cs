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
        public string GetName() => _name;
        [SerializeField] private string _description;
        public string GetDescription() => _description;
        [SerializeField] private Sprite _recipeIcon;
        public Sprite GetIcon() => _recipeIcon;
        [SerializeField] private RequiredResourceCount[] _recipeResources;
        public RequiredResourceCount[] GetResourceCount() => _recipeResources;
        [SerializeField] private string _levelOpenData;
        public string GetLevelOpenData() => _levelOpenData;
        [SerializeField] private IsStorageOff isStorageOff;
        public IsStorageOff GetIsStorageOffData() => isStorageOff;

        [Header("StorageInfo")] 
        [SerializeField] private int _maxResourceStorage;
        public int GetMaxResourceStorage() => _maxResourceStorage;
        //wanna hide if StorageOff == none

        
        public ECurrency GetCurrency()
        {
            return ECurrency.Hard;
        }
        
        public int GetCost()
        {
            return 0;
        }
    }
    
    public enum IsStorageOff
    {
        None,
        
        Wood,
        Rock,
        Ore,
        Food,
    }


    [Serializable]
    public struct RequiredResourceCount
    {
        public RequiredResourceType Key;
        public int NeedToConstruct;
    }
    
    
}