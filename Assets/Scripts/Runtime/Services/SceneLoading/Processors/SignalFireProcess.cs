using System;
using PdUtils.SceneLoadingProcessor.Impls;
using Zenject;

namespace Runtime.Services.SceneLoading.Processors
{
    public class SignalFireProcess : Process
    {
        private readonly SignalBus _signalBus;
        private readonly object _signalToFire;

        public SignalFireProcess(SignalBus signalBus, object signalToFire)
        {
            _signalBus = signalBus;
            _signalToFire = signalToFire;
        }
        public override void Do(Action onComplete)
        {
            _signalBus.Fire(_signalToFire);
            onComplete();
        }
    }
}