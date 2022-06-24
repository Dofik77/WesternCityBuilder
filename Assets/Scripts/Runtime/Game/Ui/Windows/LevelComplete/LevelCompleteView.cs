using System.Diagnostics.CodeAnalysis;
using CustomSelectables;
using Runtime.Game.Ui.Impls;
using Runtime.Game.Ui.Objects;
using Runtime.Game.Ui.Objects.General;
using Runtime.Game.Ui.Objects.UiObjectives;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Game.Ui.Windows.LevelComplete 
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class LevelCompleteView : UiCurrenciesView 
    {
        public TMP_Text LevelN;
        
        public CustomButton NextLevel;

        public Image Back;
        public CustomUiObject Top;
        public CustomUiObject Center;
        public CustomUiObject Bottom;

        public ProgressBar ScoreBar;
        public UiBinaryObjective LevelComplete;
        public UiBinaryObjective NoHint;
        public UiBinaryObjective NoRollback;
        
        public UiBinaryObjective FirstObjective;
        public UiBinaryObjective SecondObjective;
        public UiBinaryObjective ThirdObjective;
        public UiObjective Time;
        public UiObjective Reward;
    }
}