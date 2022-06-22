using System;
using Runtime.Data;
using Runtime.Data.PlayerData.Currency;

namespace Runtime.Services.CommonPlayerData.Data
{
    [Serializable]
    public class CommonPlayerData
    {
        public Money Money;
        
        public AvailableSkins Skins;
        public string CurrentCubeSkinKey;
        public string CurrentBackgroundSkinKey;
        
        public Levels Levels;
        public string PreviousLevel;
        public string CurrentLevel;
        
        public bool IsAdsOff;

        public Recipes Recipes;

        public CommonPlayerData()
        {
            var defaultCubeSkin = "cube_default";
            var defaultBackgroundSkin = "background_default";

            CurrentCubeSkinKey = defaultCubeSkin;
            CurrentBackgroundSkinKey = defaultBackgroundSkin;

            Skins = new AvailableSkins {Keys = new string[0]};
            Skins.Add(defaultCubeSkin);
            Skins.Add(defaultBackgroundSkin);

            Money.CurrencyValues = new[]
                {new Money.CurrencyValue(ECurrency.Soft), new Money.CurrencyValue(ECurrency.Hard)};

            Levels = new Levels {Properties = Array.Empty<Levels.LevelProperty>()};
            SetNextLevel("Level_001");

            Recipes = new Recipes{Keys = Array.Empty<string>()};
        }

        public void SetNextLevel(string nextLevelKey)
        {
            PreviousLevel = CurrentLevel;
            CurrentLevel = nextLevelKey;
            Levels.Add(CurrentLevel);
        }

        public void SetPreviousLevel() =>
            CurrentLevel = PreviousLevel;

        public void SetCurrentLevelComplete(int totalTime, bool isFirstObjectiveComplete,
            bool isSecondObjectiveComplete, bool isThirdObjectiveComplete) =>
            Levels.Set(new Levels.LevelProperty(CurrentLevel, true, totalTime, isFirstObjectiveComplete,
                isSecondObjectiveComplete, isThirdObjectiveComplete));

        public Levels.LevelProperty GetCurrentLevel() => Levels.Properties.Get(CurrentLevel);
    }

    [Serializable]
    public struct AvailableSkins : IStringKeyData
    {
        public string[] Keys;

        public bool Contain(string key)
        {
            for (int i = 0; i < Keys.Length; i++)
                if (Keys[i] == key)
                    return true;
            return false;
        }

        public void Add(string key)
        {
            Array.Resize(ref Keys, Keys.Length + 1);
            Keys[Keys.Length - 1] = key;
        }
    }

    [Serializable]
    public struct Money
    {
        public CurrencyValue[] CurrencyValues;

        public int Get(ECurrency currency)
        {
            for (int i = 0; i < CurrencyValues.Length; i++)
                if (CurrencyValues[i].Currency == currency)
                    return CurrencyValues[i].Value;
            return 0;
        }

        public void Set(ECurrency currency, int value)
        {
            for (int i = 0; i < CurrencyValues.Length; i++)
                if (CurrencyValues[i].Currency == currency)
                {
                    CurrencyValues[i].Value = value;
                    return;
                }
        }

        public void Add(ECurrency currency, int value)
        {
            for (int i = 0; i < CurrencyValues.Length; i++)
                if (CurrencyValues[i].Currency == currency)
                {
                    CurrencyValues[i].Value += value;
                    return;
                }
        }

        [Serializable]
        public struct CurrencyValue
        {
            public ECurrency Currency;
            public int Value;

            public CurrencyValue(ECurrency currency)
            {
                Currency = currency;
                Value = 0;
            }
        }
    }


    [Serializable]
    public struct Recipes : IStringKeyData
    {
        public string[] Keys;
        
        public bool Contain(string key)
        {
            for (int i = 0; i < Keys.Length; i++)
                if (Keys[i] == key)
                    return true;
            return false;
        }
        

        public void Add(string key)
        {
            if (Contain(key))
                return;
            Array.Resize(ref Keys, Keys.Length + 1);
            Keys[Keys.Length - 1] = key;
        }
        
        
    }

    [Serializable]
    public struct Levels : IStringKeyData
    {
        public LevelProperty[] Properties;

        public void Set(LevelProperty property)
        {
            for (int i = 0; i < Properties.Length; i++)
                if (Properties[i].GetLowerKey() == property.GetLowerKey())
                {
                    Properties[i] = property;
                    return;
                }

            Add(property.GetLowerKey());
            Properties[Properties.Length - 1] = property;
        }

        public bool Contain(string key)
        {
            for (int i = 0; i < Properties.Length; i++)
                if (Properties[i].GetLowerKey() == key)
                    return true;
            return false;
        }

        public void Add(string key)
        {
            if (Contain(key))
                return;
            Array.Resize(ref Properties, Properties.Length + 1);
            Properties[Properties.Length - 1] = new LevelProperty(key, false, -1);
        }

        [Serializable]
        public struct LevelProperty : IHasStringKey
        {
            public string Key;
            public bool IsCompleted;
            public int TotalTime;
            public bool IsFirstObjectiveComplete;
            public bool IsSecondObjectiveComplete;
            public bool IsThirdObjectiveComplete;

            public LevelProperty(string key, bool isCompleted, int totalTime, bool isFirstObjectiveComplete = false,
                bool isSecondObjectiveComplete = false, bool isThirdObjectiveComplete = false)
            {
                Key = key;
                IsCompleted = isCompleted;
                TotalTime = totalTime;
                IsFirstObjectiveComplete = isFirstObjectiveComplete;
                IsSecondObjectiveComplete = isSecondObjectiveComplete;
                IsThirdObjectiveComplete = isThirdObjectiveComplete;
            }

            public string GetLowerKey() => Key.ToLower();
        }
    }

    public interface IStringKeyData
    {
        bool Contain(string key);
        void Add(string key);
    }
}