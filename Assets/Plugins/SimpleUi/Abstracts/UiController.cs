using System.Collections.Generic;
using SimpleUi.Interfaces;
using SimpleUi.Models;
using Zenject;

namespace SimpleUi.Abstracts
{
	public abstract class UiController<T> : IUiController where T : IUiView
	{
		private readonly Stack<UiControllerState> _states = new Stack<UiControllerState>();
		private readonly UiControllerState _defaultState = new UiControllerState(false, false, 0);

		private UiControllerState _currentState;

		[Inject] protected readonly T View;
		public bool IsActive { get; private set; }
		public bool InFocus { get; private set; }

		public void SetState(UiControllerState state)
		{
			_currentState = state;
			_states.Push(state);
		}

		public void ProcessStateOrder()
		{
			if (!_currentState.IsActive)
				return;
			SetOrder(_currentState.Order);
		}

		public void ProcessState()
		{
			if (IsActive != _currentState.IsActive)
			{
				IsActive = _currentState.IsActive;
				if (_currentState.IsActive)
					Show();
				else
					Hide();
			}

			if (InFocus == _currentState.InFocus)
				return;
			InFocus = _currentState.InFocus;
			OnHasFocus(_currentState.InFocus);
		}

		public void Back()
		{
			if (_states.Count > 0)
				_states.Pop();
			if (_states.Count == 0)
			{
				_currentState = _defaultState;
				return;
			}

			SetState(_states.Pop());
		}

		IUiElement[] IUiController.GetUiElements()
		{
			return View.GetUiElements();
		}

		public virtual void Show()
		{
			View.Show();
			OnAfterShow();
		}
		
		public virtual void OnBeforeShow()
		{
			
		}
		
		public virtual void OnAfterShow()
		{
			
		}

		public virtual void Hide()
		{
			View.Hide();
			OnAfterHide();
		}
		
		public virtual void OnBeforeHide()
		{
			
		}
		
		public virtual void OnAfterHide()
		{
			
		}

		public virtual void OnHasFocus(bool inFocus)
		{
		}

		private void SetOrder(int index)
		{
			View.SetOrder(index);
		}
	}
}