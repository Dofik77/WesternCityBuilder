using System;

namespace PdUtils.ScheduledExecutorService.Impl
{
    public class ScheduledExecutorServiceEmpty : IScheduledExecutorService
    {
        public void Schedule(Action action, float delay)
        {
            action.Invoke();
        }
    }
}