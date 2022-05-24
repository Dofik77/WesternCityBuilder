using Runtime.Game.Ui.Windows.SplashScreen.Impls;
using Runtime.Initializers;
using Zenject;

namespace Runtime.Installers
{
    public class SplashInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<SplashScreenWindow>().AsSingle();
            Container.BindInterfacesAndSelfTo<SplashInitializer>().AsSingle();
        }
    }
}