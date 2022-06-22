using ECS.Views;

namespace Runtime.Signals
{
    public struct SignalRecipeUpdate
    {
        public string Key;
        
        public SignalRecipeUpdate(string key)
        {
            Key = key;
        }
    }
}