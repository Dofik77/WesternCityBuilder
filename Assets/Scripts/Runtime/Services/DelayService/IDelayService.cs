using System;

namespace Runtime.Services.DelayService
{
    public interface IDelayService
    {
        void Do(float delay, Action action);
    }
}