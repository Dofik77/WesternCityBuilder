namespace PdUtils.Dao
{
    public interface IDao<T> 
    {
        void Save(T entity);
        T Load();
        void Remove();
    }
}