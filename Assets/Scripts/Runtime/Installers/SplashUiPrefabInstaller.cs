using Runtime.Game.Ui.Windows.SplashScreen.Impls;
using SimpleUi;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Runtime.Installers
{
    [CreateAssetMenu(menuName = "Installers/SplashUiPrefabInstaller", fileName = "SplashUiPrefabInstaller")]
    public class SplashUiPrefabInstaller : ScriptableObjectInstaller
    {
        [FormerlySerializedAs("Canvas"), SerializeField]
        private Canvas canvas;

        [SerializeField] private SplashScreenView splash;
        
        public override void InstallBindings()
        {
            var canvasView = Container.InstantiatePrefabForComponent<Canvas>(canvas);
            var canvasTransform = canvasView.transform;

            Container.BindUiView<SplashScreenViewController, SplashScreenView>(splash, canvasTransform);
        }
    }
}