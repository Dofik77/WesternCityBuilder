using System;
using System.Collections.Generic;
using NUnit.Framework;
using SimpleUi;
using SimpleUi.Signals;
using Zenject;

namespace Plugins.UITest
{
	public abstract class PdZenjectUnitTestFixture
	{
		[Inject] private List<IInitializable> _initializables;
		[Inject] private List<IDisposable> _disposables;
		private DiContainer _container;
		protected bool AutoInitialize = true;
		private bool _isInitialized;

		[SetUp]
		public virtual void Setup()
		{
			_container = new DiContainer();
			SignalBusInstaller.Install(_container);
			_container.BindUiSignals(EWindowLayer.Local);
			_container.BindUiSignals(EWindowLayer.Project);

			_container.BindInterfacesTo<WindowsController>().AsCached()
				.WithArguments(EWindowLayer.Local).NonLazy();

			_container.Bind<WindowState>().AsSingle();

			Install(_container);
			_container.Inject(this);
			_container.ResolveRoots();

			if (AutoInitialize)
				Initialize();
		}

		protected void Initialize()
		{
			if (_isInitialized || _initializables == null)
				return;
			foreach (var initialized in _initializables)
				initialized.Initialize();
			_isInitialized = true;
		}

		protected abstract void Install(DiContainer container);

		[TearDown]
		public virtual void TearDown()
		{
			_isInitialized = false;
			if (_disposables != null)
			{
				foreach (var disposable in _disposables)
					disposable.Dispose();
			}

			_container.UnbindAll();
		}
	}
}