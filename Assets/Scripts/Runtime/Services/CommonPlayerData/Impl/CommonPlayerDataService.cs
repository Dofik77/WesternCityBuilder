using PdUtils.Dao;

namespace Runtime.Services.CommonPlayerData.Impl
{
    public class CommonPlayerDataService : ICommonPlayerDataService<Data.CommonPlayerData>
    {
        private readonly IDao<Data.CommonPlayerData> _dao;
        private Data.CommonPlayerData _cachedData;

        public CommonPlayerDataService(IDao<Data.CommonPlayerData> dao)
        {
            _dao = dao;
        }
        public Data.CommonPlayerData GetData() => _cachedData ??= _dao.Load() ?? new Data.CommonPlayerData();

        public void Save(Data.CommonPlayerData value)
        {
            _cachedData = value;
            _dao.Save(value);
        }

        public void Remove()
        {
            _cachedData = null;
            _dao.Remove();
        }
    }
}