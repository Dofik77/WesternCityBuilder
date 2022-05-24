namespace Runtime.Services.GameStateService
{
    public interface IGameStateService<out T> 
    {
        T GetData();
        void Remove();
        void Save();
    }
}