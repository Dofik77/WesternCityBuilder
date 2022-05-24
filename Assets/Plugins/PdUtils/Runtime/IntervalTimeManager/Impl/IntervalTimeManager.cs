using System;
using UniRx;

namespace PdUtils.IntervalTimeManager.Impl
{
    public class IntervalTimeManager : IIntervalTimeManager
    {
        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        private readonly ISubject<Unit> _elapsed = new Subject<Unit>();
        private readonly ISubject<TimeSpan> _remainTimeUpdated = new Subject<TimeSpan>();
        private readonly TimeSpan _secondInterval = TimeSpan.FromSeconds(1);

        
        private TimeSpan _remainTimeInterval;
        
        private long _remainTimeUpdatedSec;

        public IObservable<Unit> Elapsed => _elapsed;
        public IObservable<TimeSpan> RemainTimeUpdated => _remainTimeUpdated;


        public void StartTimer(TimeSpan interval)
        {
            _disposables.Clear();

            _remainTimeUpdated.OnNext(interval);
            _remainTimeInterval = interval;//.Add(_secondInterval); //just in case we add one second, so UI will be updated 1 sec later

            Observable.Interval(_secondInterval).Subscribe(_ =>
            {
                if (_remainTimeInterval.TotalSeconds <= 0)
                {
                    _elapsed.OnNext(Unit.Default);
                    _disposables.Clear();
                }
                else
                {
                    _remainTimeInterval = _remainTimeInterval.Subtract(_secondInterval);
                    _remainTimeUpdated.OnNext(_remainTimeInterval);
                }
            }).AddTo(_disposables);
        }
    }
}