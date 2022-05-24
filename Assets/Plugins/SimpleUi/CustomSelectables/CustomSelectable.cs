using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace CustomSelectables
{
    [Flags]
    public enum Direction
    {
        Up = 1 << 1,
        Down = 1 << 2,
        Left = 1 << 3,
        Right = 1 << 4
    }

    [AddComponentMenu("UI/Custom/Custom Selectable", 70)]
    public class CustomSelectable : Selectable
    {
        public RectTransform rectTransform
        {
            get
            {
                if (ReferenceEquals(m_RectTransform, null))
                    m_RectTransform = GetComponent<RectTransform>();
                return m_RectTransform;
            }
        }
        
        [NonSerialized] private RectTransform m_RectTransform;
        
        [SerializeField] private bool pointerEnterSelecting = true;
        public Direction automaticNavigationDirection;

        [Serializable]
        public class SimpleEvent : UnityEvent { }

        public static Action OnCallNavigation;
        public static Action<CustomSelectable> OnChangeSelect;

        private static Vector3 WorldPosition(RectTransform rectTransform) => rectTransform.TransformPoint(rectTransform.rect.center);

        public Selectable FindSelectableClosest()
        {
            var targetPos = WorldPosition(transform as RectTransform);
            var distance = float.MaxValue;
            Selectable result = null;

            for (var i = 0; i < s_SelectableCount; i++)
            {
                var selectable = s_Selectables[i];

                if (selectable == this ||
                    !selectable.IsInteractable() ||
                    selectable.navigation.mode == Navigation.Mode.None)
                    continue;
                
                var newDistance = Vector3.Distance(targetPos, WorldPosition(selectable.transform as RectTransform));
                if (newDistance <= distance)
                {
                    distance = newDistance;
                    result = selectable;
                }
            }

            return result;
        }
        
        public override void OnMove(AxisEventData eventData)
        {
            OnCallNavigation?.Invoke();
            base.OnMove(eventData);
        }

        public override Selectable FindSelectableOnUp()
        {
            if (automaticNavigationDirection.HasFlag(Direction.Up))
                return FindSelectable(transform.rotation * Vector3.up);

            if (navigation.mode == Navigation.Mode.Explicit && navigation.selectOnUp != null && navigation.selectOnUp.IsInteractable())
            {
                return navigation.selectOnUp;
            }
            if ((navigation.mode & Navigation.Mode.Vertical) != 0)
            {
                return FindSelectable(transform.rotation * Vector3.up);
            }
            return null;
        }

        public override Selectable FindSelectableOnDown()
        {
            if (automaticNavigationDirection.HasFlag(Direction.Down))
                return FindSelectable(transform.rotation * Vector3.down);

            if (navigation.mode == Navigation.Mode.Explicit && navigation.selectOnDown != null && navigation.selectOnDown.IsInteractable())
            {
                return navigation.selectOnDown;
            }
            if ((navigation.mode & Navigation.Mode.Vertical) != 0)
            {
                return FindSelectable(transform.rotation * Vector3.down);
            }
            return null;
        }

        public override Selectable FindSelectableOnLeft()
        {
            if (automaticNavigationDirection.HasFlag(Direction.Left))
                return FindSelectable(transform.rotation * Vector3.left);
            
            if (navigation.mode == Navigation.Mode.Explicit && navigation.selectOnLeft != null && navigation.selectOnLeft.IsInteractable())
            {
                return navigation.selectOnLeft;
            }
            if ((navigation.mode & Navigation.Mode.Horizontal) != 0)
            {
                return FindSelectable(transform.rotation * Vector3.left);
            }
            return null;
        }

        public override Selectable FindSelectableOnRight()
        {
            if (automaticNavigationDirection.HasFlag(Direction.Right))
                return FindSelectable(transform.rotation * Vector3.right);
            
            if (navigation.mode == Navigation.Mode.Explicit && navigation.selectOnRight != null && navigation.selectOnRight.IsInteractable())
            {
                return navigation.selectOnRight;
            }
            if ((navigation.mode & Navigation.Mode.Horizontal) != 0)
            {
                return FindSelectable(transform.rotation * Vector3.right);
            }
            return null;
        }
        
        public new Selectable FindSelectable(Vector3 dir)
        {
            dir = dir.normalized;
            Vector3 localDir = Quaternion.Inverse(transform.rotation) * dir;
            Vector3 pos = transform.TransformPoint(GetPointOnRectEdge(transform as RectTransform, localDir));
            float maxScore = Mathf.NegativeInfinity;
            float maxFurthestScore = Mathf.NegativeInfinity;
            float score = 0;

            bool wantsWrapAround = navigation.wrapAround && (navigation.mode == Navigation.Mode.Vertical || navigation.mode == Navigation.Mode.Horizontal);

            Selectable bestPick = null;
            Selectable bestFurthestPick = null;

            for (int i = 0; i < s_SelectableCount; ++i)
            {
                Selectable sel = s_Selectables[i];

                if (sel == this)
                    continue;

                if (!sel.IsInteractable() || sel.navigation.mode == Navigation.Mode.None)
                    continue;

#if UNITY_EDITOR
                // Apart from runtime use, FindSelectable is used by custom editors to
                // draw arrows between different selectables. For scene view cameras,
                // only selectables in the same stage should be considered.
                if (Camera.current != null && !UnityEditor.SceneManagement.StageUtility.IsGameObjectRenderedByCamera(sel.gameObject, Camera.current))
                    continue;
#endif

                var selRect = sel.transform as RectTransform;
                Vector3 selCenter = selRect != null ? (Vector3)selRect.rect.center : Vector3.zero;
                Vector3 myVector = sel.transform.TransformPoint(selCenter) - pos;

                // Value that is the distance out along the direction.
                float dot = Vector3.Dot(dir, myVector);

                // If element is in wrong direction and we have wrapAround enabled check and cache it if furthest away.
                if (wantsWrapAround && dot < 0)
                {
                    score = -dot * myVector.sqrMagnitude;

                    if (score > maxFurthestScore)
                    {
                        maxFurthestScore = score;
                        bestFurthestPick = sel;
                    }

                    continue;
                }

                // Skip elements that are in the wrong direction or which have zero distance.
                // This also ensures that the scoring formula below will not have a division by zero error.
                if (dot <= 0)
                    continue;

                // This scoring function has two priorities:
                // - Score higher for positions that are closer.
                // - Score higher for positions that are located in the right direction.
                // This scoring function combines both of these criteria.
                // It can be seen as this:
                //   Dot (dir, myVector.normalized) / myVector.magnitude
                // The first part equals 1 if the direction of myVector is the same as dir, and 0 if it's orthogonal.
                // The second part scores lower the greater the distance is by dividing by the distance.
                // The formula below is equivalent but more optimized.
                //
                // If a given score is chosen, the positions that evaluate to that score will form a circle
                // that touches pos and whose center is located along dir. A way to visualize the resulting functionality is this:
                // From the position pos, blow up a circular balloon so it grows in the direction of dir.
                // The first Selectable whose center the circular balloon touches is the one that's chosen.
                score = dot / myVector.sqrMagnitude;

                if (score > maxScore)
                {
                    maxScore = score;
                    bestPick = sel;
                }
            }

            if (wantsWrapAround && null == bestPick) return bestFurthestPick;

            return bestPick;
        }
        
        private static Vector3 GetPointOnRectEdge(RectTransform rect, Vector2 dir)
        {
            if (rect == null)
                return Vector3.zero;
            if (dir != Vector2.zero)
                dir /= Mathf.Max(Mathf.Abs(dir.x), Mathf.Abs(dir.y));
            dir = rect.rect.center + Vector2.Scale(rect.rect.size, dir * 0.5f);
            return dir;
        }

//------------------DISABLE-ENABLE-----------------------
        [FormerlySerializedAs("onEnable")] 
        [SerializeField]
        private SimpleEvent m_OnEnable = new SimpleEvent();
        
        [FormerlySerializedAs("onDisable")] 
        [SerializeField]
        private SimpleEvent m_OnDisable = new SimpleEvent();

        private bool _cacheInteracted;

        public SimpleEvent onEnable
        {
            get => m_OnEnable;
            set => m_OnEnable = value;
        }
        
        public SimpleEvent onDisable
        {
            get => m_OnDisable;
            set => m_OnDisable = value;
        }

        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            base.DoStateTransition(state, instant);

            switch (state)
            {
                case SelectionState.Normal when !_cacheInteracted:
                    m_OnEnable.Invoke();
                    break;
                case SelectionState.Disabled:
                    _cacheInteracted = false;
                    m_OnDisable.Invoke();
                    break;
            }
        }
//-------------------------------------------------------

//-----------------------SELECT--------------------------   
        [FormerlySerializedAs("onSelect")]
        [SerializeField]
        private SimpleEvent m_OnSelect = new SimpleEvent();
    
        public SimpleEvent onSelect
        {
            get => m_OnSelect;
            set => m_OnSelect = value;
        }

        public override void OnSelect(BaseEventData eventData)
        {
            if(!interactable) return;
            
            base.OnSelect(eventData);
            
            m_OnSelect.Invoke();
            OnChangeSelect?.Invoke(this);
        }
//-------------------------------------------------------    

//---------------------DESELECT--------------------------
        [FormerlySerializedAs("onDeselect")]
        [SerializeField]
        private SimpleEvent m_OnDeselect = new SimpleEvent();
    
        public SimpleEvent onDeselect
        {
            get => m_OnDeselect;
            set => m_OnDeselect = value;
        }

        public override void OnDeselect(BaseEventData eventData = null)
        {
            base.OnDeselect(eventData);
            m_OnDeselect.Invoke();
        }
//-------------------------------------------------------

//---------------------ON-ENTER--------------------------
        [FormerlySerializedAs("onEnter")]
        [SerializeField]
        private SimpleEvent m_OnEnter = new SimpleEvent();
    
        public SimpleEvent onEnter
        {
            get => m_OnEnter;
            set => m_OnEnter = value;
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            if (pointerEnterSelecting && IsInteractable() && IsActive())
            {
                Select();
                m_OnEnter.Invoke();
                return;
            }
            
            base.OnPointerEnter(eventData);
            m_OnEnter.Invoke();
        }
//-------------------------------------------------------

//---------------------ON-EXIT---------------------------
        [FormerlySerializedAs("onExit")]
        [SerializeField]
        private SimpleEvent m_OnExit = new SimpleEvent();
        public SimpleEvent onExit
        {
            get => m_OnExit;
            set => m_OnExit = value;
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            m_OnExit.Invoke();
        } 
//-------------------------------------------------------
    }
}