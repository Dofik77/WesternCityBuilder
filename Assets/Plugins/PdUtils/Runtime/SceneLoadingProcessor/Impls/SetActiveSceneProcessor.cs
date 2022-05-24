using System;
using UnityEngine.SceneManagement;

namespace PdUtils.SceneLoadingProcessor.Impls
{
    public class SetActiveSceneProcess : Process, IProgressable
    {
        private readonly string _name;
        public float Progress => .5f;

        public SetActiveSceneProcess(string name)
        {
            _name = name;
        }

        public override void Do(Action onComplete)
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(_name));
            onComplete();
        }
    }
}