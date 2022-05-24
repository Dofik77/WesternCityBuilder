using SimpleUi;

namespace Runtime.Game.Ui.Windows.InGameButtons 
{
    public class InGameButtonsWindow : WindowBase 
    {
        public override string Name => "InGameButtons";
        protected override void AddControllers()
        {
            AddController<InGameButtonsController>();
        }
    }
}