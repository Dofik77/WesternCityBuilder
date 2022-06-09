using ECS.Game.Components.WesternBuilder_Component;
using ECS.Views;
using Runtime.Data.PlayerData.Recipe;
using Zenject.ReflectionBaking.Mono.Cecil;

namespace Runtime.Signals
{
    public struct SignalStorageUpdate
    {
        public BuildsView BuildsView;
        
        public SignalStorageUpdate(BuildsView buildsView)
        {
            BuildsView = buildsView;
        }
    }
}