using PdUtils.Dao;

namespace Runtime.Services.UiData.Impl
{
    public class UiDataService : IUiDataService<UiData.Data.UiData>
    {
        private readonly IDao<UiData.Data.UiData> _dao;
        private UiData.Data.UiData _cachedData;

        public UiDataService(IDao<UiData.Data.UiData> dao)
        {
            _dao = dao;
        }
        public UiData.Data.UiData GetData() => _cachedData ??= _dao.Load() ?? new UiData.Data.UiData();

        public void Save(UiData.Data.UiData value)
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