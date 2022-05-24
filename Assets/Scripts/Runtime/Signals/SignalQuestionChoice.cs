using System;

namespace Runtime.Signals
{
    public struct SignalQuestionChoice
    {
        public string Title;
        public Action<bool> Action;

        public SignalQuestionChoice(string title, Action<bool> action)
        {
            Title = title;
            Action = action;
        }
    }
}