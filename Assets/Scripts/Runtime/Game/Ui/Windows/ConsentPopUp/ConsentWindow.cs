using SimpleUi;
using SimpleUi.Interfaces;

namespace Runtime.Game.Ui.Windows.ConsentPopUp
{
    public class ConsentWindow : WindowBase, IPopUp
    {
        public override string Name => "ConsentPopUp";

        protected override void AddControllers()
        {
            AddController<ConsentPopUpViewController>();
        }
    }
}