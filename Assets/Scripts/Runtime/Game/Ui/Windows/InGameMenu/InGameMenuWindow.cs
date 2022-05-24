using SimpleUi;

namespace Runtime.Game.Ui.Windows.InGameMenu
{
    public class InGameMenuWindow : WindowBase
    {
        public override string Name => "InGameMenu";
        protected override void AddControllers()
        {
            AddController<InGameMenuController>();
        }
    }
}