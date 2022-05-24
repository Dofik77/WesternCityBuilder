using SimpleUi;

namespace Runtime.Game.Ui.Windows.Levels 
{
    public class LevelsWindow : WindowBase 
    {
        public override string Name => "Levels";
        protected override void AddControllers()
        {
            AddController<LevelsController>();
        }
    }
}