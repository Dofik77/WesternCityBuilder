using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Runtime.Game.Ui.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace Utils
{
    public static class DoTweenExtensions
    {
        public static TweenerCore<float, float, FloatOptions> DOFillAmount(this SlicedFilledImage target, float endValue, float duration)
        {
            endValue = Mathf.Clamp(endValue, 0, 1);
            var t = DOTween.To(() => target.fillAmount, x => target.fillAmount = x, endValue, duration);
            t.SetTarget(target);
            return t;
        }

        public static Tweener DoAppear(this Transform transform, float duration)
        {
            var localScale = transform.localScale;
            transform.localScale = Vector3.zero;
            return transform.DOScale(localScale, duration);
        }

        public static Tweener DoDisappear(this Transform transform, float duration)
        {
            return transform.DOScale(Vector3.zero, duration);
        }
        
        public static Tweener DoGradient(this LineRenderer lr, Gradient from, Gradient to, float duration)
        {
            return lr.DOColor(
                new Color2(from.Evaluate(0), from.Evaluate(1)),
                new Color2(to.Evaluate(0), to.Evaluate(1)), duration);
        }
        
        public static Tweener DoAppearColor(this Image image, float duration)
        {
            var oldColor = image.color;
            image.color = Color.clear;
            return image.DOColor(oldColor, duration);
        }
        
        public static Tweener DoDisappearColor(this Image image, float duration, Action afterComplete = default)
        {
            var oldColor = image.color;
            return image.DOColor(Color.clear, duration).OnComplete(() =>
            {
                afterComplete?.Invoke();
                image.color = oldColor;
            });
        }
        
        [Obsolete]
        public static Tweener DoFromPosition(this RectTransform rectTransform, Vector2 from, float duration)
        {
            var oldPos = rectTransform.anchoredPosition;
            rectTransform.anchoredPosition = from;
            return rectTransform.DOAnchorPos(oldPos, duration);
        }
        
        [Obsolete]
        public static Tweener DoToPosition(this RectTransform rectTransform, Vector2 to, float duration, Action afterComplete = default)
        {
            var oldPos = rectTransform.anchoredPosition;
            return rectTransform.DOAnchorPos(to, duration).OnComplete(() =>
            {
                afterComplete?.Invoke();
                rectTransform.anchoredPosition = oldPos;
            });
        }
    }
}