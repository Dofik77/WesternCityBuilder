using Runtime.Game.Ui.Windows.BlackScreen;
using SimpleUi;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using Zenject;

namespace Runtime.Installers
{
    [CreateAssetMenu(menuName = "Installers/ProjectUiPrefabInstaller", fileName = "ProjectUiPrefabInstaller")]
    public class ProjectUiPrefabInstaller : ScriptableObjectInstaller
    {
        [FormerlySerializedAs("Canvas"), SerializeField]
        private Canvas canvas;
        [SerializeField]private EventSystem eventSystem;

        
        [SerializeField] private BlackScreenView blackScreen;
        
        public override void InstallBindings()
        {
            var canvasView = Container.InstantiatePrefabForComponent<Canvas>(canvas);
            var canvasTransform = canvasView.transform;
            var camera = canvasTransform.GetComponentInChildren<Camera>();
            camera.depth = 10;
            camera.clearFlags = CameraClearFlags.Depth;
            Container.BindUiView<BlackScreenViewController, BlackScreenView>(blackScreen, canvasTransform);
        }
    }
}