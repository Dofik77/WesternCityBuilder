using System.Collections.Generic;

namespace PdUtils.PlayerPrefs.Impl
{
    public class InMemoryPlayerPrefsManager : IPlayerPrefsManager
    {
        private readonly Dictionary<string, object> _values = new Dictionary<string, object>();
        
        public bool HasKey(string key)
        {
            return _values.ContainsKey(key);
        }

        public void DeleteKey(string key)
        {
            _values.Remove(key);
        }

        public void DeleteAll()
        {
            _values.Clear();
        }

        public void Save()
        {
//            var str = string.Join(";", _values.Select(x => x.Key + "=" + x.Value).ToArray());
//            Debug.Log("Save player prefs \n" + str);
        }

        public void SetValue<T>(string key, T value)
        {
            if (_values.ContainsKey(key))
            {
                _values[key] = value;
            }
            else
            {
                _values.Add(key, value);
            }
        }
        
        public T GetValue<T>(string key)
        {
            if (_values.ContainsKey(key))
            {
                return (T) _values[key];
            }
            return default;
        }
        
        public T GetValue<T>(string key, T defaultValue)
        {
            if (_values.ContainsKey(key))
            {
                return (T) _values[key];
            }
            return defaultValue;
        }
    }
}