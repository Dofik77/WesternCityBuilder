using Runtime.Signals;
using Zenject;

namespace Runtime.Installers
{
    public class SignalInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.DeclareSignal<SignalGameInit>();
            Container.DeclareSignal<SignalBlackScreen>();
            Container.DeclareSignal<SignalQuestionChoice>();
            Container.DeclareSignal<SignalDeveloperMode>();
            Container.DeclareSignal<SignalJoystickUpdate>();
            Container.DeclareSignal<SignalLevelState>();
            Container.DeclareSignal<SignalLevelDataUpdate>();
            Container.DeclareSignal<SignalTimerUpdate>();
            Container.DeclareSignal<SignalHpBarUpdate>();
            Container.DeclareSignal<SignalScoreUpdate>();
            Container.DeclareSignal<SignalUpdateCurrency>();
            Container.DeclareSignal<SignalStorageUpdate>();
            Container.DeclareSignal<SignalEnableResourceCounter>();
        }
    }
}