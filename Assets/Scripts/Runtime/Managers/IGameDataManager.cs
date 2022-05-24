namespace Runtime.Managers
{
    public interface IGameDataManager
    {
        bool NewGame { get; }
        void RemoveAllData();
        void RemoveCollectableData();
        void RemoveNonCollectableData();
        void RemoveLiveGameState();
    }
}