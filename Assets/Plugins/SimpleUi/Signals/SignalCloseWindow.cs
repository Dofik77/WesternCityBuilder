using SimpleUi.Interfaces;

namespace SimpleUi.Signals
{
	public class SignalCloseWindow
	{
		public readonly IWindow Window;

		public SignalCloseWindow(IWindow window)
		{
			Window = window;
		}
	}
}