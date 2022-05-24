using SimpleUi;

namespace Runtime.Game.Ui.Windows.Store 
{
    public class StoreWindow : WindowBase 
    {
        public override string Name => "Store";
        protected override void AddControllers()
        {
            AddController<StoreController>();
        }
    }
}