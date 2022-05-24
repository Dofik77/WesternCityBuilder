using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PdUtils.SceneLoadingProcessor.Impls
{
	public class LoadingProcess : Process, IProgressable
	{
		public float Progress => _operation?.progress ?? 0;
		
		private readonly string _sceneName;
		private readonly LoadSceneMode _mode;
		private AsyncOperation _operation;
		private Action _complete;

		public LoadingProcess(string sceneName, LoadSceneMode mode)
		{
			_sceneName = sceneName;
			_mode = mode;
		}

		public override void Do(Action complete)
		{
			_complete = complete;
			_operation = SceneManager.LoadSceneAsync(_sceneName, _mode);
			_operation.completed += OnCompleted;
		}

		private void OnCompleted(AsyncOperation obj)
		{
			_operation.completed -= OnCompleted;
			_complete();
		}
	}
}