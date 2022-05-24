using System;
using DG.Tweening;
using SimpleUi.Abstracts;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Game.Ui.Windows.BlackScreen
{
    public class BlackScreenView : UiView
    {
        [SerializeField] private Image image;

        private Color _defaultColor = Color.white;

        public void Show(Action complete, bool isShow, float duration, Color color)
        {
            if (color != default && isShow)
            {
                image.color = color;
            }
            
            image.DOFade(isShow ? 0 : 1, 0);
            image.DOFade(isShow ? 1 : 0, duration)
                .OnComplete(() =>
                {
                    if (!isShow)
                    {
                        image.color = _defaultColor;
                    }
                    complete?.Invoke();
                });
        }
    }
}