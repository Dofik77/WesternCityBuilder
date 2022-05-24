using ECS.DataSave;
using Facebook.Unity;
using PdUtils;
using PdUtils.Dao;
using PdUtils.PlayerPrefs;
using PdUtils.PlayerPrefs.Impl;
using Runtime.Game.Ui.Windows.BlackScreen;
using Runtime.Initializers;
using Runtime.Managers.Impl;
using Runtime.Services.CommonPlayerData;
using Runtime.Services.GameStateService;
using Runtime.Services.GameStateService.Impl;
using Runtime.Services.PlayerSettings;
using Runtime.Services.PlayerSettings.Impl;
using Runtime.Services.SceneLoading;
using Runtime.Services.SceneLoading.Impls;
using Runtime.Services.UiData;
using UniRx;
using UnityEngine;
using Utils.SeparateThreadExecutor.Impl;
using Utils.ThreadLocalStorage;
using Zenject;
using ZenjectUtil.Test.Extensions; // using GameAnalyticsSDK;

namespace Runtime.Installers
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            MainThreadDispatcher.Initialize();
            SignalBusInstaller.Install(Container);
            CommonPlayerDataInstaller.InstallServices(Container);
            UiDataInstaller.InstallServices(Container);

            Application.targetFrameRate = 60;
            FB.Init();
            // GameAnalytics.Initialize();
            
            Container.BindInterfacesTo<ThreadedLocalStorageDao<GameState>>().AsTransient().WithArguments("gameState");
            Container.BindInterfacesTo<DefaultSeparateThreadExecutor<string>>().AsSingle();
            Container.BindInterfacesTo<DefaultSeparateThreadExecutor>().AsSingle();
            Container.BindInterfacesTo<DefaultSeparateThreadExecutor<DataPool>>().AsSingle();

            Container.BindSubstituteInterfacesTo<ISceneLoadingManager, SceneLoadingManager>().AsSingle();
            Container.BindSubstituteInterfacesTo<IGameStateService<GameState>, GameStateService>().AsSingle();
            Container.BindSubstituteInterfacesTo<IPlayerSettingsService, PlayerSettingsService>().AsSingle();
            Container.BindFromSubstitute<IPlayerPrefsManager, PersistancePlayerPrefsManager>().AsSingle();
            Container.BindInterfacesTo<PdAudioInitializer>().AsSingle().NonLazy();

            Container.BindInterfacesTo<LocalStorageDao<PlayerSettings>>()
                .AsTransient().WithArguments("playerSettings");

            Container.BindInterfacesAndSelfTo<GameDataManager>().AsSingle();
            
            BindWindows();
        }
        
        private void BindWindows()
        {
            Container.BindInterfacesAndSelfTo<BlackScreenWindow>().AsSingle();
        }
    }
}