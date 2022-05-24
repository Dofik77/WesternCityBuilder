using System;
using UnityEngine;

namespace Runtime.Data.PlayerData.Currency.Impls
{
    [CreateAssetMenu(menuName = "PlayerData/CurrenciesData", fileName = "CurrenciesData")]
    public class CurrenciesData : ScriptableObject, ICurrenciesData
    {
        public Currency[] Get() => _currencies;
        [SerializeField] private Currency[] _currencies;
        
        public Currency Get(ECurrency type)
        {
            for (var i = 0; i < _currencies.Length; i++)
            {
                var skin = _currencies[i];
                if (skin.Type == type)
                    return skin;
            }
            throw new Exception("[CurrenciesData] Can't find skin with key: " + type);
        }
    }
}