using SimpleUi.Interfaces;

namespace SimpleUi.Signals
{
	public class SignalActiveWindow
	{
		public readonly IWindow Window;

		public SignalActiveWindow(IWindow window)
		{
			Window = window;
		}
	}
}