namespace Runtime.Services.SceneLoading
{
    public interface ISceneLoadingManager
    {
        string CurrentScene { get; }
        void LoadScene(string key);
        void LoadScene(EScene key);

        void ReloadScene();
    }
}