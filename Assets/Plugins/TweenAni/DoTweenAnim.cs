using System;
using DG.Tweening;
using TweenAni;
using UnityEngine;

namespace Plugins.TweenAni
{
    public class DoTweenAnim : MonoBehaviour
    {
        [SerializeField] private bool isCompleteIfKillTween;
        [SerializeField] private new TweenAnimation animation;

        private Tween _currentTween;

        private void OnEnable()
        {
            _currentTween = animation.PlaySequence();
        }

        private void OnDisable()
        {
            _currentTween?.Kill(isCompleteIfKillTween);
        }
    }
}