using System;
using UnityEngine;

namespace Runtime.Game.Ui.Windows.TouchPad
{
    public class SwipeDetector : MonoBehaviour
    {
        private Vector2 fingerDownPosition;
        private Vector2 fingerUpPosition;

        [SerializeField]
        private bool detectSwipeOnlyAfterRelease = false;

        [SerializeField]
        private float minDistanceForSwipe = 20f;

        public Action<SwipeData> OnSwipe = delegate { };

        private void Update()
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    fingerUpPosition = touch.position;
                    fingerDownPosition = touch.position;
                }

                if (!detectSwipeOnlyAfterRelease && touch.phase == TouchPhase.Moved)
                {
                    fingerDownPosition = touch.position;
                    DetectSwipe();
                }

                if (touch.phase == TouchPhase.Ended)
                {
                    fingerDownPosition = touch.position;
                    DetectSwipe();
                }
            }
        }

        private void DetectSwipe()
        {
            if (SwipeDistanceCheckMet())
            {
                if (IsVerticalSwipe())
                {
                    var direction = fingerDownPosition.y - fingerUpPosition.y > 0 ? ESwipeDirection.Up : ESwipeDirection.Down;
                    SendSwipe(direction);
                }
                else
                {
                    var direction = fingerDownPosition.x - fingerUpPosition.x > 0 ? ESwipeDirection.Right : ESwipeDirection.Left;
                    SendSwipe(direction);
                }
                fingerUpPosition = fingerDownPosition;
            }
        }

        private bool IsVerticalSwipe()
        {
            return VerticalMovementDistance() > HorizontalMovementDistance();
        }

        private bool SwipeDistanceCheckMet()
        {
            return VerticalMovementDistance() > minDistanceForSwipe || HorizontalMovementDistance() > minDistanceForSwipe;
        }

        private float VerticalMovementDistance()
        {
            return Mathf.Abs(fingerDownPosition.y - fingerUpPosition.y);
        }

        private float HorizontalMovementDistance()
        {
            return Mathf.Abs(fingerDownPosition.x - fingerUpPosition.x);
        }

        private void SendSwipe(ESwipeDirection direction)
        {
            SwipeData swipeData = new SwipeData()
            {
                Direction = direction,
                StartPosition = fingerDownPosition,
                EndPosition = fingerUpPosition
            };
            OnSwipe(swipeData);
        }
    }

    public struct SwipeData
    {
        public Vector2 StartPosition;
        public Vector2 EndPosition;
        public ESwipeDirection Direction;
    }

    public enum ESwipeDirection
    {
        Up,
        Down,
        Left,
        Right
    }
}