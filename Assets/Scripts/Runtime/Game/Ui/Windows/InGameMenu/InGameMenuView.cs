using CustomSelectables;
using SimpleUi.Abstracts;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Game.Ui.Windows.InGameMenu
{
    public class InGameMenuView : UiView
    {
        public string PrivacyPolicyURL;
        public string TermsOfUseURL;
        
        public Image Background;
        public RectTransform Top;
        public RectTransform Center;
        public RectTransform Bottom;

        public CustomButton Back;
        public CustomButton SoundOn;
        public CustomButton SoundOff;
        public CustomButton VibrationOn;
        public CustomButton VibrationOff;
        public CustomButton PrivacyPolicy;
        public CustomButton TermsOfUse;
        
        
    }
}