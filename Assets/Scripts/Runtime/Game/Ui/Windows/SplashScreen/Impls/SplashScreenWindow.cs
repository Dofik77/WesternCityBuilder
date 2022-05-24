using SimpleUi;

namespace Runtime.Game.Ui.Windows.SplashScreen.Impls
{
    public class SplashScreenWindow : WindowBase
    {
        public override string Name => "SplashScreen";
        protected override void AddControllers()
        {
            AddController<SplashScreenViewController>();
        }
    }
}