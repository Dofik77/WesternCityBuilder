using Runtime.Game.Ui.Windows.DeveloperMode;
using Runtime.Game.Ui.Windows.InGameButtons;
using Runtime.Game.Ui.Windows.TouchPad;
using SimpleUi;

namespace Runtime.Game.Ui.Windows
{
    public class GameHudWindow : WindowBase
    {
        public override string Name => nameof(GetType);
        protected override void AddControllers()
        {
            AddController<InGameButtonsController>();       
            AddController<DeveloperModeController>();      
            AddController<TouchpadViewController>();  
        }
    }
}