using Runtime.Game.Ui.Windows.ConsentPopUp;
using Runtime.Game.Ui.Windows.Main.MainMenu;
using Runtime.Initializers;
using Zenject;

namespace Runtime.Installers
{
    public class MenuInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<MenuInitializer>().AsSingle();
            BindWindows();
        }

        private void BindWindows()
        {
            Container.BindInterfacesAndSelfTo<MainMenuWindow>().AsSingle();
            Container.BindInterfacesAndSelfTo<ConsentWindow>().AsSingle();
        }
    }
}