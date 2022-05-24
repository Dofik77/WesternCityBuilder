using Zenject;

namespace ZenjectUtil.Installers {
	public class HelpersInstaller : MonoInstaller{
		public override void InstallBindings() {
			Container.Bind<GuiRenderableManager>().AsSingle();
			Container.Bind<GuiRenderer>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
			Container.Bind<GizmoRenderableManager>().AsSingle();
			Container.Bind<GizmoRenderer>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
		}
	}
}
