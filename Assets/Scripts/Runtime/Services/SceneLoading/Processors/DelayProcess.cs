using System;
using PdUtils.SceneLoadingProcessor.Impls;
using UniRx;
using Zenject;

namespace Runtime.Services.SceneLoading.Processors
{
    public class DelayProcess : Process, IDisposable
    {
        private readonly int _delay;
        private readonly SignalBus _signalBus;
        private readonly bool _isShow;
        private IDisposable _disposable = Disposable.Empty;

        public DelayProcess(int delayFrames)
        {
            _delay = delayFrames;
        }
        public override void Do(Action onComplete)
        {
            _disposable = Observable.TimerFrame(_delay).Subscribe(x =>
            {
                _disposable?.Dispose();
                onComplete?.Invoke();
            });
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }
}