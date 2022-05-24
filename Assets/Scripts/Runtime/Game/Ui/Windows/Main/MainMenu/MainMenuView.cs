using System.Diagnostics.CodeAnalysis;
using CustomSelectables;
using SimpleUi.Abstracts;

namespace Runtime.Game.Ui.Windows.Main.MainMenu
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class MainMenuView : UiView
    {
        public CustomButton PlayGame;
        public CustomButton Exit;
    }
}