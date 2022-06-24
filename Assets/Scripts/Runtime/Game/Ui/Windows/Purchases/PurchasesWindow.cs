using SimpleUi;

namespace Runtime.Game.Ui.Windows.Purchases 
{
    public class PurchasesWindow : WindowBase 
    {
        public override string Name => "Purchases";
        protected override void AddControllers()
        {
            AddController<PurchasesController>();
        }
    }
}