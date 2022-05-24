using SimpleUi;

namespace Runtime.Game.Ui.Windows.DeveloperMode 
{
    public class DeveloperModeWindow : WindowBase 
    {
        public override string Name => "DeveloperMode";
        protected override void AddControllers()
        {
            AddController<DeveloperModeController>();
        }
    }
}