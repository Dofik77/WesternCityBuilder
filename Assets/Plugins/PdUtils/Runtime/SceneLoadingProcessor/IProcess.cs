using System;

namespace PdUtils.SceneLoadingProcessor
{
	public interface IProcess
	{
		void Do(Action complete);
	}
}