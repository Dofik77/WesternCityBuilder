namespace PdUtils.SceneLoadingProcessor
{
	public interface IProcessor
	{
		IProcessor AddProcess(IProcess process);
		void DoProcess();
	}
}