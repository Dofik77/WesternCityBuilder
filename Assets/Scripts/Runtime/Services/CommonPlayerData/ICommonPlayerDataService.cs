namespace Runtime.Services.CommonPlayerData
{
    public interface ICommonPlayerDataService<T>
    {
        T GetData();
        void Save(T value);
        void Remove();
    }
}