using System.Collections.Generic;

namespace PdUtils.SceneLoadingProcessor.Impls
{
	public class LoadingProcessor : ILoadingProcessor
	{
		public float Progress
		{
			get
			{
				if (_progressables.Count == 0)
					return 0;
				var progress = 0f;
				foreach (var progressable in _progressables) 
					progress += progressable.Progress;
				return progress / _progressables.Count;
			}
		}

		private readonly Queue<IProcess> _processes = new Queue<IProcess>();
		private List<IProgressable> _progressables = new List<IProgressable>();

		public IProcessor AddProcess(IProcess process)
		{
			_processes.Enqueue(process);
			if(process is IProgressable progressable)
				_progressables.Add(progressable);
			return this;
		}

		public void DoProcess()
		{
			var process = _processes.Dequeue();
			process.Do(OnComplete);
		}

		private void OnComplete()
		{
			if (_processes.Count == 0)
				return;
			DoProcess();
		}
	}
}