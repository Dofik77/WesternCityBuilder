using System;

namespace PdUtils.SceneLoadingProcessor.Impls
{
	public abstract class Process : IProcess
	{
		public abstract void Do(Action onComplete);
	}
}