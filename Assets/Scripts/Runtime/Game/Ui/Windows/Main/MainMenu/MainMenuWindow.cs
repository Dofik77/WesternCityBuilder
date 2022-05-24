using SimpleUi;
using SimpleUi.Interfaces;
using SimpleUi.Managers;
using Zenject;

namespace Runtime.Game.Ui.Windows.Main.MainMenu
{
    public class MainMenuWindow : WindowBase, INoneBack
    {
        [Inject] private ISelectableMapper _selectableMapper;
        
        public override string Name => "MainMenu";
        protected override void AddControllers()
        {
            AddController<MainMenuViewController>(out var controller);

            var firstSelectable = (IDefaultSelectable) controller; //Setting start selectable to mapper;
            _selectableMapper.Register(Name, firstSelectable.DefaultSelectable);
        }
    }
}