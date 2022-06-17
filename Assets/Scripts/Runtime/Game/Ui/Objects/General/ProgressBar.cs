using DG.Tweening;
using Runtime.Game.Ui.Extensions;
using TMPro;
using UnityEngine;
using Utils;

namespace Runtime.Game.Ui.Objects.General
{
    public class ProgressBar : CustomUiObject
    {
        [SerializeField] private SlicedFilledImage progress;
        public TMP_Text Text;

        public void Repaint(float ratio, Color color, float duration = 0.05f)
        {
            progress.DOKill();
            progress.DOFillAmount(Mathf.Abs(ratio), 0.1f);
            progress.DOColor(color, duration);
        }

        public void Repaint(float ratio, float duration = 0.05f)
        {
            progress.DOKill();
            progress.DOFillAmount(Mathf.Abs(ratio), 0.1f);
            progress.DOColor(progress.color, duration);
        }

        public void RepaintConstruction(float ratio, float delay)
        {
            progress.DOKill();
            progress.DOFillAmount(Mathf.Abs(ratio), delay);
            progress.DOColor(progress.color, delay);
        }
    }
}