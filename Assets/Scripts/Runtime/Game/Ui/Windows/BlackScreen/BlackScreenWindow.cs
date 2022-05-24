using SimpleUi;
using SimpleUi.Interfaces;

namespace Runtime.Game.Ui.Windows.BlackScreen
{
    public class BlackScreenWindow : WindowBase, IPopUp
    {
        public override string Name => "BlackScreen";
        
        protected override void AddControllers()
        {
            AddController<BlackScreenViewController>();
            
        }

       
    }
}