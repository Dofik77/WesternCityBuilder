using System.Collections.Generic;
using DG.Tweening;
using TweenAni;
using UnityEngine;

namespace Plugins.TweenAni
{
    public class DoTweenAnimBase : MonoBehaviour
    {
        [SerializeField] private bool isCompleteIfKillTween;
        [SerializeField] private List<TweenAnimation> animations;

        private Tween currentTween;

        public void PlayTween(int index)
        {
            if (index >= animations.Count)
                throw new System.ArgumentOutOfRangeException();
            
            currentTween?.Kill(isCompleteIfKillTween);

            currentTween = animations[index].PlaySequence();
        }
    }
}