using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PdUtils.SceneLoadingProcessor.Impls
{
	public class UnloadProcess : Process
	{
		private readonly string _sceneName;
		private Action _complete;

		public UnloadProcess(string sceneName)
		{
			_sceneName = sceneName;
		}

		public override void Do(Action complete)
		{
			_complete = complete;
			var unloadSceneAsync = SceneManager.UnloadSceneAsync(_sceneName);
			unloadSceneAsync.completed += OnUnloadSceneCompleted;
		}

		private void OnUnloadSceneCompleted(AsyncOperation obj)
		{
			var unloadUnusedAssets = Resources.UnloadUnusedAssets();
			unloadUnusedAssets.completed += OnUnloadUnusedAssetsCompleted;
		}

		private void OnUnloadUnusedAssetsCompleted(AsyncOperation obj)
		{
			_complete();
		}
	}
}