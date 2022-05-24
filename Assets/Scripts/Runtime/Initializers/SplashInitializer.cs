using Runtime.Game.Ui.Windows.SplashScreen.Impls;
using SimpleUi.Signals;
using Zenject;

namespace Runtime.Initializers
{
    public class SplashInitializer : IInitializable
    {
        private readonly SignalBus _signalBus;

        public SplashInitializer(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }
        
        public void Initialize()
        {
            _signalBus.OpenWindow<SplashScreenWindow>();
        }
    }
}