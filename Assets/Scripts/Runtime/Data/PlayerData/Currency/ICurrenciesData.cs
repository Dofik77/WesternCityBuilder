using System;
using UnityEngine;

namespace Runtime.Data.PlayerData.Currency
{
    public interface ICurrenciesData
    {
        Currency Get(ECurrency type);
        Currency[] Get();
    }
    
    [Serializable]
    public struct Currency
    {
        [Header("General")]
        public ECurrency Type;
        [Header("Store")]
        public string Name;
        public Sprite Icon;
    }

    public enum ECurrency
    {
        Soft,
        Hard
    }
}