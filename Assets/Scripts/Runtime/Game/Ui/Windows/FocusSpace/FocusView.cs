using System;
using DG.Tweening;
using SimpleUi.Abstracts;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Game.Ui.Windows.FocusSpace
{
    public class FocusView : UiView
    {
        public Canvas canvas;
        public Image image;

        public void SetActiveBack(bool value, Action onComplete = null)
        {
            image.DOKill();
            image.DOFade(value ? 0.75f : 0, 0.2f).OnComplete(()=>onComplete?.Invoke());
        }
    }
}