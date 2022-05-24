using System;

namespace Runtime.Data
{
    public static class HasKeyExtensions
    {
        public static T Get<T>(this T[] array, string key) where T : IHasStringKey
        {
            for (var i = 0; i < array.Length; i++)
            {
                var hasKeyObject = array[i];
                if (hasKeyObject.GetLowerKey() == key)
                    return hasKeyObject;
            }
            throw new Exception("[IHasStringKey] Can't find object with key: " + key);
        }
        
        public static T Get<T>(this T[] array, Enum key) where T : IHasEnumKey
        {
            for (var i = 0; i < array.Length; i++)
            {
                var hasKeyObject = array[i];
                if (hasKeyObject.GetKey().Equals(key))
                    return hasKeyObject;
            }
            throw new Exception("[IHasEnumKey] Can't find object with key: " + key);
        }
    }
    
    public interface IHasStringKey
    {
        string GetLowerKey();
    }
    
    public interface IHasEnumKey
    {
        Enum GetKey();
    }
}