using System;
using UnityEngine;

namespace Runtime.Signals
{
    public struct SignalBlackScreen
    {
        public readonly bool IsShow;
        public readonly Action Complete;
        public readonly float Duration;
        public readonly Color ChangeColor;

        public SignalBlackScreen(bool isShow, Action complete = null, float duration = 0, Color color = default)
        {
            IsShow = isShow;
            Duration = duration;
            Complete = complete;
            ChangeColor = color;
        }
    }
}