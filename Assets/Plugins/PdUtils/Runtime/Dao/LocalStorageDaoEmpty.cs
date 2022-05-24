namespace PdUtils.Dao
{
    public class DaoEmpty<T> : IDao<T> where T : class
    {
        private readonly string _filename;
        private T _data;

        public DaoEmpty(string filename)
        {
            _filename = filename;
        }

        public void Save(T vo)
        {
            _data = vo;
        }

        public T Load()
        {
            return _data;
        }

        public void Remove()
        {
            _data = null;
        }

        private string GetPath()
        {
            return "";
        }
    }
}