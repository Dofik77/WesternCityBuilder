using UnityEngine;

namespace Runtime.Signals
{
    public struct SignalJoystickUpdate
    {
        public bool IsPressed;
        public Vector2 ButtonPosition;
        public Vector2 OriginPosition;
        
        public SignalJoystickUpdate(bool isPressed, Vector2 buttonPosition, Vector2 originPosition)
        {
            ButtonPosition = buttonPosition;
            IsPressed = isPressed;
            OriginPosition = originPosition;
        }
    }
}