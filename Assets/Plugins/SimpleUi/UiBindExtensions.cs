using System;
using SimpleUi.Interfaces;
using SimpleUi.Managers;
using SimpleUi.Signals;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace SimpleUi
{
	public static class UiBindExtensions
	{
		public static void BindUiView<T, TU>(this DiContainer container, Object viewPrefab, Transform parent, Action<GameObject> onSpawned,
			bool enable = false)
			where TU : IUiView
			where T : IUiController
		{
			container.BindInterfacesAndSelfTo<T>().AsSingle().NonLazy();
			container.BindInterfacesAndSelfTo<TU>()
				.FromComponentInNewPrefab(viewPrefab)
				.UnderTransform(parent).AsSingle()
				.OnInstantiated((context, o) =>
				{
					var spawned = ((MonoBehaviour) o).gameObject;
					onSpawned?.Invoke(spawned.gameObject);
					spawned.SetActive(enable);
				}).NonLazy();
			
		}
		
		public static void BindUiView<T, TU>(this DiContainer container, Object viewPrefab, Transform parent, bool enable = false)
			where TU : IUiView
			where T : IUiController
		{
			container.BindInterfacesAndSelfTo<T>().AsSingle();
			container.BindInterfacesAndSelfTo<TU>()
				.FromComponentInNewPrefab(viewPrefab)
				.UnderTransform(parent).AsSingle()
				.OnInstantiated((context, o) =>
				{
					((MonoBehaviour) o).gameObject.SetActive(enable);
				});
		}

		public static void BindUiSignals(this DiContainer container, EWindowLayer windowLayer)
		{
			container.DeclareSignal<SignalOpenWindow>().WithId(windowLayer);
			container.DeclareSignal<SignalOpenRootWindow>().WithId(windowLayer);
			container.DeclareSignal<SignalBackWindow>().WithId(windowLayer);
			container.DeclareSignal<SignalActiveWindow>().WithId(windowLayer).OptionalSubscriber();
			container.DeclareSignal<SignalFocusWindow>().WithId(windowLayer).OptionalSubscriber();
			container.DeclareSignal<SignalCloseWindow>().WithId(windowLayer).OptionalSubscriber();
		}

		public static void BindWindowsController<T>(this DiContainer container, EWindowLayer windowLayer)
			where T : IWindowsController, IInitializable
		{
			container.BindInitializableExecutionOrder<T>(-1000);
			container.BindInterfacesTo<T>().AsSingle().WithArguments(windowLayer).NonLazy();
			var windowState = new WindowState();
			container.BindInterfacesTo<WindowState>().FromInstance(windowState).AsSingle();
			container.Bind<WindowState>().FromInstance(windowState).WhenInjectedInto<T>();
			container.BindInterfacesAndSelfTo<UiMapperManager>().AsSingle().WithArguments(windowLayer).NonLazy();
		}
	}
}