using Plugins.PdUtils.Runtime.PdAudio;
using Runtime.Data.Base.Audio;
using Runtime.Game.Ui.Windows.Main.MainMenu;
using SimpleUi.Signals;
using Zenject;

namespace Runtime.Initializers
{
    public class MenuInitializer : IInitializable
    {
        private readonly SignalBus _signalBus;
        private readonly PdAudio _pdAudio;
        private readonly IAudioBase _audioBase;

        public MenuInitializer(SignalBus signalBus, PdAudio pdAudio, IAudioBase audioBase)
        {
            _signalBus = signalBus;
            _pdAudio = pdAudio;
            _audioBase = audioBase;
        }

        public void Initialize()
        {
            _signalBus.OpenWindow<MainMenuWindow>();
            //_pdAudio.PlayMusic(_audioBase.Get("MainMenu"));
        }
    }
}