using SimpleUi.Interfaces;

namespace SimpleUi.Signals
{
	public class SignalFocusWindow
	{
		public readonly IWindow Window;

		public SignalFocusWindow(IWindow window)
		{
			Window = window;
		}
	}
}