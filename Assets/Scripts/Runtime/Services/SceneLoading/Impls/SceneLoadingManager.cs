using PdUtils.SceneLoadingProcessor.Impls;
using Runtime.Services.SceneLoading.Processors;
using Runtime.Services.Uid;
using Runtime.Signals;
using UnityEngine.SceneManagement;
using Zenject;
// ReSharper disable InconsistentNaming

namespace Runtime.Services.SceneLoading.Impls
{
	public class SceneLoadingManager : ISceneLoadingManager
	{
		private readonly SignalBus _signalBus;
		private LoadingProcessor _processor;
		
		private const string GAME_CONTEXT = "[GAME CONTEXT]";
		private const string MENU_CONTEXT = "[MENU CONTEXT]";
		private const string MENU_SCENE = "MainMenu";
		private const string LOAD_SCENE = "LoadingScene";

		public SceneLoadingManager(SignalBus signalBus)
		{
			_signalBus = signalBus;
			CurrentScene = SceneManager.GetActiveScene().name;
		}
		public string CurrentScene { get; private set; }

		public void LoadScene(EScene key) => LoadScene(key.ToString());

		public void ReloadScene() => LoadScene(CurrentScene);
		public void LoadScene(string key)
		{
			UidGenerator.Clear();
			_processor = new LoadingProcessor();
			_processor
				.AddProcess(new ProjectWindowBack(_signalBus, true))
				.AddProcess(new LoadingProcess(LOAD_SCENE, LoadSceneMode.Additive))
				.AddProcess(new UnloadProcess(CurrentScene))
				.AddProcess(new LoadingProcess(key, LoadSceneMode.Additive))
				.AddProcess(new UnloadProcess(LOAD_SCENE))
				.AddProcess(new RunContextProcess(key == MENU_SCENE ? MENU_CONTEXT : GAME_CONTEXT))
				.AddProcess(new DelayProcess(15))
				.AddProcess(new ProjectWindowBack(_signalBus))
				.AddProcess(new SignalFireProcess(_signalBus, new SignalGameInit()))
				.DoProcess();
			CurrentScene = key;
		}

		public float GetProgress() => _processor?.Progress ?? 0f;
	}
}