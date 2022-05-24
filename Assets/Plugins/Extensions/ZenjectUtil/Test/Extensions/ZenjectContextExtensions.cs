using Zenject;

namespace ZenjectUtil.Test.Extensions {
	public static class ZenjectContextExtensions {

		public static void Run(this SceneDecoratorContext context, DiContainer container) {
			context.Initialize(container);
			context.InstallDecoratorSceneBindings();
			context.InstallDecoratorInstallers();
			context.InstallLateDecoratorInstallers();
		}
		
	}
}
