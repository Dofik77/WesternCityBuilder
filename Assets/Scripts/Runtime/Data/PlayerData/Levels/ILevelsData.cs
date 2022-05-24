using System;
using Runtime.Data.PlayerData.Currency;
using Runtime.Game.Ui.Objects.Layouts;
using UnityEngine;

namespace Runtime.Data.PlayerData.Levels
{
    public interface ILevelsData
    {
        Level[] Get();
    }
    
    [Serializable]
    public struct Level : IProvideUiGeneratedEntity
    {
        public string GetLowerKey() => _key.ToLower();
        [Header("General")] [SerializeField] private string _key;
        public string GetNextLevelKey() => _nextLevelKey;
        [SerializeField] private string _nextLevelKey;
        
        public string GetName() => _name;
        [Header("Store")] [SerializeField] private string _name;
        public Sprite GetIcon() => _storeIcon;
        [SerializeField] private Sprite _storeIcon;
        public int GetCost() => _cost;
        [SerializeField] private int _cost;
        public ECurrency GetCurrency() => _storeCurrency;
        [SerializeField] private ECurrency _storeCurrency;
        
        public ECurrency GetRewardCurrency() => _rewardCurrency;
        [Header("Reward")] [SerializeField] private ECurrency _rewardCurrency;
        public int GetFirstObjectiveReward() => _firstObjectiveReward;
        [SerializeField] private int _firstObjectiveReward;
        public int GetSecondObjectiveReward() => _secondObjectiveReward;
        [SerializeField] private int _secondObjectiveReward;
        public int GetThirdObjectiveReward() => _thirdObjectiveReward;
        [SerializeField] private int _thirdObjectiveReward;
        
        public float GetFirstObjective() => _firstObjective;
        [Header("TimeObjectives")] [SerializeField] private float _firstObjective;
        public float GetSecondObjective() => _secondObjective;
        [SerializeField] private float _secondObjective;
        public float GetThirdObjective() => _thirdObjective;
        [SerializeField] private float _thirdObjective;

    }
}