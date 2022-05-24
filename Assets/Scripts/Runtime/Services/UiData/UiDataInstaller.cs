using PdUtils.Dao;
using Runtime.Services.UiData.Impl;
using Zenject;
using ZenjectUtil.Test.Extensions;

namespace Runtime.Services.UiData
{
    public static class UiDataInstaller
    {
        public static void InstallServices(DiContainer container)
        {
            InstallDao(container);
            container.BindSubstituteInterfacesTo<IUiDataService<Data.UiData>, UiDataService>().AsSingle();
        }

        private static void InstallDao(DiContainer container)
        {
            container.BindInterfacesTo<LocalStorageDao<Data.UiData>>().AsTransient().WithArguments("uiData");
        }
    }
}