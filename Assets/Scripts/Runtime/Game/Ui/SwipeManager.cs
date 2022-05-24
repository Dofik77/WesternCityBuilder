using UnityEngine;
using UnityEngine.Events;

namespace Runtime.Game.Ui
{
    public enum Swipe
    {
        Up,
        Down,
        Left,
        Right
    };

    public class SwipeManager : MonoBehaviour
    {
        public float minSwipeLength = 200f;

        public UnityEvent<Swipe> onSwipeDetected;

        private Vector2 _firstPressPos;
        private Vector2 _secondPressPos;
        private Vector2 _currentSwipe;

        private void Update()
        {
            DetectSwipe();
        }

        private void DetectSwipe()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _firstPressPos = Input.mousePosition;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                _secondPressPos = Input.mousePosition;
                _currentSwipe = new Vector3(_secondPressPos.x - _firstPressPos.x, _secondPressPos.y - _firstPressPos.y);
                
                if (_currentSwipe.magnitude < minSwipeLength)
                {
                    return;
                }

                _currentSwipe.Normalize();
                
                if (_currentSwipe.y > 0 && _currentSwipe.x > -0.5f && _currentSwipe.x < 0.5f)
                {
                    onSwipeDetected?.Invoke(Swipe.Up);
                }
                else if (_currentSwipe.y < 0 && _currentSwipe.x > -0.5f && _currentSwipe.x < 0.5f)
                {
                    onSwipeDetected?.Invoke(Swipe.Down);
                }
                else if (_currentSwipe.x < 0 && _currentSwipe.y > -0.5f && _currentSwipe.y < 0.5f)
                {
                    onSwipeDetected?.Invoke(Swipe.Left);
                }
                else if (_currentSwipe.x > 0 && _currentSwipe.y > -0.5f && _currentSwipe.y < 0.5f)
                {
                    onSwipeDetected?.Invoke(Swipe.Right);
                }
            }
        }
    }
}