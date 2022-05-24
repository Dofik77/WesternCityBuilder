using Runtime.Game.Ui.Objects.UiObjectives;
using UnityEngine;

namespace Runtime.Game.Ui.Objects.General
{
    public class CurrencyBox : UiObjective
    {
        public void Set(int value, Sprite icon)
        {
            Icon.sprite = icon;
            Value.text = value.ToString();
        }
    }
}