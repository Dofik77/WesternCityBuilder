namespace Runtime.Services.UiData
{
    public interface IUiDataService<T>
    {
        T GetData();
        void Save(T value);
        void Remove();
    }
}