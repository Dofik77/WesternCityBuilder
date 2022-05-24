using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace TweenAni
{
    public partial class TweenAnimation
    {
        [Serializable]
        public class TweenerPreset
        {
            public string name;
            public bool disabled;

            public TweenerType tweenerType;
            public GameObject target;

            public Anchor anchor;
            public float insertAt;
            public float interval;

            public float endValue;
            public Vector3 endValueVector;
            public Color endValueColor;
            public bool relative;
            public int vibrato;
            public float elasticity;
            public RotateMode rotateMode;

            public float duration;

            public Ease ease;
            public float overshoot;
            public float amplitude;
            public float period;

            public bool from;
            public bool fromRelative;

            public Tweener CreateTweener()
            {
                Tweener tweener;
                switch (tweenerType)
                {
                    case TweenerType.LocalMoveX:
                        tweener = target.transform.DOLocalMoveX(endValue, duration); break;
                    case TweenerType.LocalMoveY:
                        tweener = target.transform.DOLocalMoveY(endValue, duration); break;
                    case TweenerType.ScaleUniformly:
                        tweener = target.transform.DOScale(endValue, duration); break;
                    case TweenerType.ScaleX:
                        tweener = target.transform.DOScaleX(endValue, duration); break;
                    case TweenerType.ScaleY:
                        tweener = target.transform.DOScaleY(endValue, duration); break;
                    case TweenerType.AnchorPosX:
                        tweener = target.GetComponent<RectTransform>().DOAnchorPosX(endValue, duration); break;
                    case TweenerType.AnchorPosY:
                        tweener = target.GetComponent<RectTransform>().DOAnchorPosY(endValue, duration); break;
                    case TweenerType.PivotX:
                        tweener = target.GetComponent<RectTransform>().DOPivotX(endValue, duration); break;
                    case TweenerType.PivotY:
                        tweener = target.GetComponent<RectTransform>().DOPivotY(endValue, duration); break;
                    case TweenerType.FadeCanvas:
                        tweener = target.GetComponent<CanvasGroup>().DOFade(endValue, duration); break;
                    case TweenerType.FadeImage:
                        tweener = target.GetComponent<Image>().DOFade(endValue, duration); break;
                    case TweenerType.FadeText:
                        tweener = target.GetComponent<Text>().DOFade(endValue, duration); break;
                    case TweenerType.DoText:
                        var textComponent = target.GetComponent<Text>();
                        string text = textComponent.text;
                        textComponent.text = string.Empty;
                        tweener = textComponent.DOText(text, duration);
                        break;
                    case TweenerType.LocalMove:
                        tweener = target.transform.DOLocalMove(endValueVector, duration); break;
                    case TweenerType.Scale:
                        tweener = target.transform.DOScale(endValueVector, duration); break;
                    case TweenerType.AnchorPos:
                        tweener = target.GetComponent<RectTransform>().DOAnchorPos(endValueVector, duration); break;
                    case TweenerType.Pivot:
                        tweener = target.GetComponent<RectTransform>().DOPivot(endValueVector, duration); break;
                    case TweenerType.SizeDelta:
                        tweener = target.GetComponent<RectTransform>().DOSizeDelta(endValueVector, duration); break;
                    case TweenerType.SizeDeltaX:
                        tweener = target.GetComponent<RectTransform>().DOSizeDeltaX(endValue, duration); break;
                    case TweenerType.SizeDeltaY:
                        tweener = target.GetComponent<RectTransform>().DOSizeDeltaY(endValue, duration); break;
                    case TweenerType.LocalRotate:
                        tweener = target.transform.DOLocalRotate(endValueVector, duration, rotateMode); break;
                    case TweenerType.PunchScale:
                        tweener = target.transform.DOPunchScale(endValueVector, duration, vibrato, elasticity); break;
                    case TweenerType.Color:
                        tweener = target.GetComponent<Graphic>().DOColor(endValueColor, duration); break;
                    default:
                        throw new NotImplementedException($"TweenerType '{tweenerType}' is not implemented yet.");
                }

                if (ease.IsBack()) { tweener.SetEase(ease, overshoot); }
                else if (ease.IsElastic()) { tweener.SetEase(ease, amplitude, period); }
                else if (ease.IsFlash()) { tweener.SetEase(ease, overshoot, period); }
                else { tweener.SetEase(ease); }

                if (relative) { tweener.SetRelative(); }
                if (from) { tweener.From(fromRelative); }

                return tweener;
            }
        }
    }
}