using SimpleUi.Managers;
using SimpleUi.Signals;
using UnityEngine;
using Zenject;

namespace SimpleUi
{
	public class SimpleUiInstaller : MonoInstaller
	{
		[SerializeField] private EWindowLayer windowLayer;

		public override void InstallBindings()
		{
			Container.BindInitializableExecutionOrder<UiSelectableMapperManager>(-1001);
			Container.BindInterfacesAndSelfTo<UiSelectableMapperManager>().AsSingle().WithArguments(windowLayer).NonLazy();
			Container.BindInitializableExecutionOrder<WindowsController>(-1000);
			Container.BindInterfacesAndSelfTo<WindowsController>().AsSingle()
				.WithArguments(windowLayer).NonLazy();
			Container.Bind<WindowState>().AsSingle();

			Container.BindUiSignals(windowLayer);
			
		}
	}
}