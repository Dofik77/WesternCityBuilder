using System;
using UniRx;

namespace PdUtils.ScheduledExecutorService.Impl
{
    public class ScheduledExecutorService : IScheduledExecutorService, IDisposable
    {
        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        public void Schedule(Action action, float delay)
        {
            Observable.Timer(TimeSpan.FromSeconds(delay))
                .Subscribe(_ => action?.Invoke()).AddTo(_disposables);
        }

        public void Dispose()
        {
            _disposables?.Dispose();
        }
    }
}