using UnityEngine;

namespace Runtime.Game.Ui.Objects.General
{
    public class UiSwitchObject : CustomUiObject
    {
        [SerializeField] private RectTransform _switchable;

        public void Switch(bool value) => _switchable.gameObject.SetActive(value);
    }
}