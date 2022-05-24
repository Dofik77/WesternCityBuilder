using PdUtils.Dao;
using Runtime.Services.CommonPlayerData.Impl;
using Zenject;
using ZenjectUtil.Test.Extensions;

namespace Runtime.Services.CommonPlayerData
{
    public static class CommonPlayerDataInstaller
    {
        public static void InstallServices(DiContainer container)
        {
            InstallDao(container);
            container.BindSubstituteInterfacesTo<ICommonPlayerDataService<Data.CommonPlayerData>, CommonPlayerDataService>().AsSingle();
        }

        private static void InstallDao(DiContainer container)
        {
            container.BindInterfacesTo<LocalStorageDao<Data.CommonPlayerData>>().AsTransient().WithArguments("commonPlayerData");
        }
    }
}