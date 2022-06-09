using System.Diagnostics.CodeAnalysis;
using CustomSelectables;
using Runtime.Game.Ui.Objects.General;
using Runtime.Game.Ui.Objects.UiObjectives;
using SimpleUi.Abstracts;
using TMPro;
using UnityEngine;

namespace Runtime.Game.Ui.Windows.InGameButtons
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "RedundantDefaultMemberInitializer")]
    public class InGameButtonsView : UiView
    {
        public CustomButton DeveloperModeBtn;
        public CustomButton InGameMenuBtn;
        public CustomButton Levels;
        public CustomButton Store;
        public CustomButton AdsOff;
        public CustomButton StartToPlayBtn;
        public CustomButton HintBtn;
        public UiObjective Timer;
        public RectTransform UiBox;
        public RectTransform Top;
        public RectTransform Center;
        public RectTransform Bottom;

        public ResourceCounter WoodReserveData;
        public ResourceCounter RockReserveData;
        public ResourceCounter FoodReserveData;

        public TMP_Text LevelN;
        
        public TMP_Text Score;
        public TMP_Text HighScore;
        public TMP_Text Currency;

        public RectTransform Vignette;
        public float VignetteDuration = 0.3f;
        public int MaxHp = 100;
        public ProgressBar HpBar;
    }
}