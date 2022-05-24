using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace TweenAni
{
    [Serializable]
    public partial class TweenAnimation
    {
        [SerializeField] float timeScale = 1f;
        [SerializeField, Range(-1, 10)] int loops = 1;
        [SerializeField] LoopType loopType;
        [SerializeField] List<TweenerPreset> tweenerPresets = new List<TweenerPreset>();

        Sequence sequenceInPlay = null;
        public bool isPlaying => sequenceInPlay != null;

        public Tween PlaySequence()
        {
            if (isPlaying)
            {
                Debug.LogWarning("Attempt to play animation while it is playing.");
                return null;
            }

            var seq = DOTween.Sequence();
            var hook = new SequenceHookHelper();
            foreach (var tp in tweenerPresets)
            {
                if (tp.disabled) { continue; }

                if (tp.anchor == Anchor.Append)
                {
                    var tween = tp.CreateTweener();
                    seq.Append(tween);

                    hook.Update(hook.sequenceEnd, tween.Duration());
                }
                if (tp.anchor == Anchor.Join)
                {
                    var tween = tp.CreateTweener();
                    seq.Join(tween);

                    hook.Update(hook.recentTweenPos, tween.Duration());
                }
                if (tp.anchor == Anchor.Insert)
                {
                    var tween = tp.CreateTweener();
                    seq.Insert(tp.insertAt, tween);

                    hook.Update(tp.insertAt, tween.Duration());
                }
                if (tp.anchor == Anchor.Follow)
                {
                    var tween = tp.CreateTweener();
                    seq.Insert(hook.recentTweenEnd, tween);

                    hook.Update(hook.recentTweenEnd, tween.Duration());
                }
                if (tp.anchor == Anchor.AppendInterval)
                {
                    seq.AppendInterval(tp.interval);
                    hook.sequenceEnd += tp.interval;
                }
            }

            if (loops != 1) { seq.SetLoops(loops, loopType); }
            seq.timeScale = timeScale;

            sequenceInPlay = seq;
            seq.onKill += () => { sequenceInPlay = null; };

            return seq;
        }

        public void StopSequence(bool resetToInitState)
        {
            if (!isPlaying)
            {
                Debug.LogWarning("Attempt to stop animation while none is playing.");
            }
            else
            {
                if (resetToInitState) { sequenceInPlay.Goto(0f); }
                sequenceInPlay.Kill(true);
            }
        }

        /// <summary>
        /// Helper class for implementing <see cref="Anchor.Follow"></see> feature.
        /// </summary>
        private class SequenceHookHelper
        {
            public float recentTweenPos = 0;
            public float recentTweenEnd = 0;
            public float sequenceEnd = 0;

            public void Update(float tweenPos, float tweenDuration)
            {
                recentTweenPos = tweenPos;
                recentTweenEnd = tweenPos + tweenDuration;
                if (sequenceEnd < recentTweenEnd) { sequenceEnd = recentTweenEnd; }
            }
        }
    }
}