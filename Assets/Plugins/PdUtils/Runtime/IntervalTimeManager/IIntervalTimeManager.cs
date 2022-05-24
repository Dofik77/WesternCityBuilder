using System;
using UniRx;

namespace PdUtils.IntervalTimeManager
{
    public interface IIntervalTimeManager
    {
        IObservable<Unit> Elapsed { get; }
        IObservable<TimeSpan> RemainTimeUpdated { get; }

        void StartTimer(TimeSpan interval);
    }
}