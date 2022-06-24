using CustomSelectables;
using SimpleUi.Abstracts;
using TMPro;
using UnityEngine;

namespace Runtime.Game.Ui.Windows.DeveloperMode
{
    public class DeveloperModeView : UiView
    {
        public RectTransform UiBox;
        public CustomButton DropProgressBtn;
        public TMP_Text FpsCounter;
        public float FpsCheckTime = 0.5f;

        private float _elapsed;
        private int _count;

        private void Update()
        {
            if (!UiBox.gameObject.activeSelf)
                return;
            _elapsed += Time.deltaTime;
            _count++;
            if (_elapsed >= FpsCheckTime)
            {
                // FpsCounter.text = Mathf.RoundToInt(1.0f / Time.deltaTime).ToString();
                FpsCounter.text = Mathf.RoundToInt(1.0f / (_elapsed / _count)).ToString();
                _elapsed = 0;
                _count = 0;
            }
        }
    }
}