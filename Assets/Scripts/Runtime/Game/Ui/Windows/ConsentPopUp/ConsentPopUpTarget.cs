using SimpleUi.Abstracts;
using TMPro;
using UnityEngine;
using CustomSelectables;

namespace Runtime.Game.Ui.Windows.ConsentPopUp
{
    public class ConsentPopUpTarget : UiView
    {
        public TextMeshProUGUI Title;
        public CustomButton Yes;
        public CustomButton No;
        public CanvasGroup canvasGroup;
        public GameObject blockMouse;
    }
}