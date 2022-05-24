using SimpleUi.Models;
using UnityEngine.UI;

namespace SimpleUi.Interfaces
{
	public interface IUiController
	{
		bool IsActive { get; }
		bool InFocus { get; }
		void SetState(UiControllerState state);
		void ProcessStateOrder();
		void ProcessState();
		void Back();
		IUiElement[] GetUiElements();
	}
}