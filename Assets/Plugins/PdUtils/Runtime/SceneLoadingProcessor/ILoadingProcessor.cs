namespace PdUtils.SceneLoadingProcessor
{
	public interface ILoadingProcessor : IProcessor
	{
		float Progress { get; }
	}
}