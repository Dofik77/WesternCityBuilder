using System;
using UnityEngine.UI;

namespace SimpleUi.Signals
{
	public class SignalOpenWindow
	{
		public readonly Type Type;
		public readonly string Name;

		public SignalOpenWindow(Type type)
		{
			Type = type;
		}
		
		public SignalOpenWindow(string name)
		{
			Name = name;
		}
	}
}