namespace Zenject {
	// Note that if you want to bind to this class to have GuiRender called on
	// your non-MonoBehaviour classes, you also need to add
	// `Container.Bind<GuiRenderer>().FromNewComponentOnNewGameObject().NonLazy()` to an installer somewhere
	// Or, if you always want this to be supported in all scenes, then add the following inside
	// an installer that is attached to the ProjectContext:
	// `Container.Bind<GizmoRenderer>().FromNewComponentOnNewGameObject().AsSingle().CopyIntoAllSubContainers().NonLazy()`
	public interface IGizmoRenderable {
		void GizmoRender();
	}
}
