using Runtime.Game.Ui.Objects.UiObjectives;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Game.Ui.Objects.Layouts
{
    public class UiLayoutObjective : UiObjective
    {
        [SerializeField] private Image _tapSelectedImage;

        public void SetSelected(bool value) => _tapSelectedImage.gameObject.SetActive(value);
    }
}