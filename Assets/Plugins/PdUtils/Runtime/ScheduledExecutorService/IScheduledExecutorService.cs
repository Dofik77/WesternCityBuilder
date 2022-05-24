using System;

namespace PdUtils.ScheduledExecutorService
{
    public interface IScheduledExecutorService
    {
        void Schedule(Action action, float delay);
    }
}