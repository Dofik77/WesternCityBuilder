using SimpleUi;

namespace Runtime.Game.Ui.Windows.LevelComplete 
{
    public class LevelCompleteWindow : WindowBase 
    {
        public override string Name => "LevelComplete";
        protected override void AddControllers()
        {
            AddController<LevelCompleteController>();
        }
    }
}