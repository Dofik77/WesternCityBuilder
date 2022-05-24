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

        private void Update()
        {
            if (!UiBox.gameObject.activeSelf)
                return;
            _elapsed += Time.deltaTime;
            if (_elapsed >= FpsCheckTime)
            {
                _elapsed = 0;
                FpsCounter.text = Mathf.RoundToInt(1.0f / Time.deltaTime).ToString();
            }

            
        }
    }
}