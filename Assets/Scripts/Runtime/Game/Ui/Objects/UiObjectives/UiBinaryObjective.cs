using Runtime.Game.Ui.Objects.General;
using TMPro;
using UnityEngine;

namespace Runtime.Game.Ui.Objects.UiObjectives
{
    public class UiBinaryObjective : CustomUiObject
    {
        public TMP_Text Text;
        public RectTransform CompleteRect;
        public RectTransform FailRect;
        public TMP_Text Value;

        public RectTransform SetComplete(bool value)
        {
            CompleteRect.gameObject.SetActive(value);
            FailRect.gameObject.SetActive(!value);
            return value ? CompleteRect : FailRect;
        }
    }
}