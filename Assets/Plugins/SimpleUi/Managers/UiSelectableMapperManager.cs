using System;
using System.Collections.Generic;
using CustomSelectables;
using SimpleUi.Interfaces;
using SimpleUi.Signals;
using UniRx;
using UnityEngine.UI;
using Zenject;

namespace SimpleUi.Managers
{
    public interface ISelectableMapper
    {
        void Register<T>(Selectable defaultSelectable) where T : IWindow;
        void Register(string name, Selectable defaultSelectable);
        void ChangeCurrent<T>(Selectable selectable) where T : IWindow;
        void ChangeCurrent(string name, Selectable selectable);
        void Unregister<T>()  where T : IWindow;
        void Unregister(string name);
        Selectable Get<T>() where T : IWindow;
        Selectable Get(string name);
    }
    public class UiSelectableMapperManager : ISelectableMapper, IInitializable, IDisposable
    {
        private readonly WindowState _windowState;
        private readonly SignalBus _signalBus;
        private readonly DiContainer _container;
        private readonly EWindowLayer _windowLayer;
        
        private readonly Dictionary<string, Selectable> _lastSelectables = new Dictionary<string, Selectable>();
        private readonly Dictionary<string, Selectable> _defaultSelectables = new Dictionary<string, Selectable>();
        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        public UiSelectableMapperManager(SignalBus signalBus, DiContainer container, EWindowLayer windowLayer, WindowState windowState)
        {
            _signalBus = signalBus;
            _container = container;
            _windowLayer = windowLayer;
            _windowState = windowState;
        }

        public void Initialize()
        {
            _signalBus.GetStreamId<SignalFocusWindow>(_windowLayer).Subscribe(GetSelectableForFocusWindow).AddTo(_disposables);
            CustomSelectable.OnChangeSelect += ChangeSelectable;
        }
        
        public void Dispose()
        {
            CustomSelectable.OnChangeSelect -= ChangeSelectable;
            _disposables.Dispose();
        }

        private void GetSelectableForFocusWindow(SignalFocusWindow signal)
        {
            var selectable = Get(signal.Window.Name);
            if (!selectable) return;
            selectable.Select();
            selectable.OnSelect(null);
        }
        
        private void ChangeSelectable(CustomSelectable selectable)
        {
            var currentWindow = _windowState.CurrentWindowName;
            
            if(currentWindow == null || !_defaultSelectables.ContainsKey(currentWindow)) return;
            
            ChangeCurrent(currentWindow, selectable);
        }

        public void Register<T>(Selectable defaultSelectable) where T : IWindow =>
            Register(_container.Resolve<T>().Name, defaultSelectable);

        public void Register(string name, Selectable defaultSelectable)
        {
            _defaultSelectables.Add(name, defaultSelectable);
            _lastSelectables.Add(name, defaultSelectable);
        }
        
        public void Unregister<T>() where T : IWindow => Unregister(_container.Resolve<T>().Name);

        public void Unregister(string name)
        {
            _lastSelectables.Remove(name);
            _defaultSelectables.Remove(name);
        }

        public void ChangeCurrent<T>(Selectable selectable) where T : IWindow => ChangeCurrent(_container.Resolve<T>().Name, selectable);

        public void ChangeCurrent(string name, Selectable selectable)
        {
            if (!_defaultSelectables.ContainsKey(name)) throw new Exception($"The Window({name}) must be registered first");
                
            _lastSelectables[name] = selectable;
        }

        public Selectable Get<T>() where T : IWindow => Get(_container.Resolve<T>().Name);

        public Selectable Get(string name)
        {
            if (!_lastSelectables.ContainsKey(name)) return null;

            if (_lastSelectables[name] == null || !_lastSelectables[name].interactable)
            {
                _lastSelectables[name] = _defaultSelectables[name];
            }
            
            return _lastSelectables[name];
        }
    }
}