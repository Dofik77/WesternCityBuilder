using Runtime.Game.Ui.Windows.ConsentPopUp;
using Runtime.Game.Ui.Windows.Main.MainMenu;
using SimpleUi;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Runtime.Installers
{
    [CreateAssetMenu(menuName = "Installers/MenuUiPrefabInstaller", fileName = "MenuUiPrefabInstaller")]
    public class MenuUiPrefabInstaller : ScriptableObjectInstaller
    {
        [FormerlySerializedAs("Canvas"), SerializeField]
        private Canvas canvas;
        [SerializeField] private MainMenuView mainMenu;
        [SerializeField] private ConsentPopUpTarget quit;
        public override void InstallBindings()
        {
            var canvasObj = Instantiate(canvas);
            var canvasTransform = canvasObj.transform;
            var camera = canvasTransform.GetComponentInChildren<Camera>();
            camera.depth = 0;
            
            canvasObj.gameObject.AddComponent<AudioListener>();
            Container.Bind<Canvas>().FromInstance(canvasObj).AsSingle().NonLazy();

            Container.BindUiView<ConsentPopUpViewController, ConsentPopUpTarget>(quit, canvasTransform);
            Container.BindUiView<MainMenuViewController, MainMenuView>(mainMenu, canvasTransform); 
        }
    }
}