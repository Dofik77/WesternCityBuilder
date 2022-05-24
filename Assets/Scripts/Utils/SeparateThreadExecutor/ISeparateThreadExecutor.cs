using System;

namespace Utils.SeparateThreadExecutor
{
    public interface ISeparateThreadExecutor<T>
    {
        void Execute(Func<T> val, Action<T> mainThreadAction);
    }
    
    public interface ISeparateThreadExecutor
    {
        void Execute(Action action, Action mainThreadAction);
    }
}