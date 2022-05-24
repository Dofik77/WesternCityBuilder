using System;
using System.Collections;
using System.Threading;
using UniRx;
using Thread = System.Threading.Thread;

namespace Utils.SeparateThreadExecutor.Impl
{
	public class DefaultSeparateThreadExecutor<T> : ISeparateThreadExecutor<T>
	{
		public void Execute(Func<T> func, Action<T> mainThreadAction)
		{
			var worker = new Worker(func);
			Observable.FromMicroCoroutine(worker.Start)
				.Subscribe(unit => mainThreadAction(worker.Result));
		}

		private class Worker
		{
			private readonly Func<T> _func;

			public T Result { get; private set; }
			private bool _isComplete;

			public Worker(Func<T> func)
			{
				_func = func;
			}

			public IEnumerator Start()
			{
				var thread = new Thread(o =>
				{
					Result = _func();
					_isComplete = true;
				})
				{
					IsBackground = true,
					Priority = ThreadPriority.Lowest
				};
				thread.Start();

				while (!_isComplete)
				{
					yield return null;
				}
			}
		}
	}

	public class DefaultSeparateThreadExecutor : ISeparateThreadExecutor, IDisposable
	{
		private Worker worker;
		public void 
			Execute(Action action, Action mainThreadAction)
		{
			worker = new Worker(action);
			Observable.FromMicroCoroutine(worker.Start)
				.Subscribe(unit => mainThreadAction());
		}

		private class Worker
		{
			private readonly Action _action;
			private Thread _thread;
			private bool _isComplete;

			public Worker(Action action)
			{
				_action = action;
			}

			public IEnumerator Start()
			{
				_thread = new Thread(o =>
				{
					_action();
					_isComplete = true;
				})
				{
					IsBackground = true,
					Priority = ThreadPriority.Lowest
				};
				_thread.Start();

				while (!_isComplete)
				{
					yield return null;
				}
			}
			public void DisposeThread()
			{
				_thread?.Abort();
			}
		}

		public void Dispose()
		{
			worker?.DisposeThread();
		}
	}
}