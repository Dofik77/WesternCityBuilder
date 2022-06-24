using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace Runtime.Game.Ui.Objects.General
{
    public class CustomUiObject : MonoBehaviour
    {
        public RectTransform RectTransform
        {
            get
            {
                if (ReferenceEquals(m_RectTransform, null))
                    m_RectTransform = GetComponent<RectTransform>();
                return m_RectTransform;
            }
        }
        
        [NonSerialized] private RectTransform m_RectTransform;

        private Vector2 _outOfBoundsPos;
        private Vector2 _defaultPos;
        private float _tweenDuration;

        public void Init(Vector2 outOfBoundsPos, float duration, bool outOfBounds = true)
        {
            _outOfBoundsPos = outOfBoundsPos;
            _tweenDuration = duration;
            _defaultPos = RectTransform.anchoredPosition;

            if (outOfBounds)
                RectTransform.anchoredPosition = _outOfBoundsPos;
        }

        public TweenerCore<Vector2, Vector2, VectorOptions> DoToDefault() => RectTransform.DOAnchorPos(_defaultPos, _tweenDuration);
        public TweenerCore<Vector2, Vector2, VectorOptions> DoOutOfBounds() => RectTransform.DOAnchorPos(_outOfBoundsPos, _tweenDuration);


        }
}