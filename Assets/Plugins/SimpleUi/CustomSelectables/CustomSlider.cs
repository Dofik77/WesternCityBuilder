using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CustomSelectables
{
    [AddComponentMenu("UI/Custom/Custom Slider", 70)]
    public class CustomSlider : CustomSelectable, IDragHandler, IInitializePotentialDragHandler, ICanvasElement
    {
        public enum DirectionSlider
        {
            LeftToRight,
            RightToLeft,
            BottomToTop,
            TopToBottom,
        }

        [Serializable]
        public class SliderEvent : UnityEvent<float> {}

        [SerializeField]
        private RectTransform m_FillRect;
        public RectTransform fillRect 
        { 
            get => m_FillRect;
            set
            {
                if (!SetPropertyUtility.SetClass(ref m_FillRect, value)) return;
                UpdateCachedReferences(); UpdateVisuals();
            } 
        }

        [SerializeField]
        private RectTransform m_HandleRect;
        public RectTransform handleRect
        {
            get => m_HandleRect;
            set
            {
                if (!SetPropertyUtility.SetClass(ref m_HandleRect, value)) return;
                UpdateCachedReferences(); UpdateVisuals();
            }
        }

        [Space]

        [SerializeField]
        private DirectionSlider m_Direction = DirectionSlider.LeftToRight;
        public DirectionSlider direction
        {
            get => m_Direction;
            set { if (SetPropertyUtility.SetStruct(ref m_Direction, value)) UpdateVisuals(); }
        }

        [SerializeField]
        private float m_MinValue = 0;
        public float minValue
        {
            get => m_MinValue;
            set
            {
                if (!SetPropertyUtility.SetStruct(ref m_MinValue, value)) return;
                Set(m_Value); UpdateVisuals();
            }
        }

        [SerializeField]
        private float m_MaxValue = 1;
        public float maxValue
        {
            get => m_MaxValue;
            set
            {
                if (!SetPropertyUtility.SetStruct(ref m_MaxValue, value)) return;
                Set(m_Value); UpdateVisuals();
            }
        }

        [SerializeField]
        private bool m_WholeNumbers = false;
        public bool wholeNumbers
        {
            get => m_WholeNumbers;
            set
            {
                if (!SetPropertyUtility.SetStruct(ref m_WholeNumbers, value)) return;
                Set(m_Value); UpdateVisuals();
            }
        }

        [SerializeField]
        protected float m_Value;
        public virtual float value
        {
            get => wholeNumbers ? Mathf.Round(m_Value) : m_Value;
            set => Set(value);
        }
        
        public virtual void SetValueWithoutNotify(float input) => Set(input, false);
        
        public float normalizedValue
        {
            get
            {
                if (Mathf.Approximately(minValue, maxValue))
                    return 0;
                return Mathf.InverseLerp(minValue, maxValue, value);
            }
            set => this.value = Mathf.Lerp(minValue, maxValue, value);
        }

        [Space]

        [SerializeField]
        private SliderEvent m_OnValueChanged = new SliderEvent();
        public SliderEvent onValueChanged 
        { 
            get => m_OnValueChanged;
            set => m_OnValueChanged = value;
        }

        // Private fields
        private Image m_FillImage;
        private Transform m_FillTransform;
        private RectTransform m_FillContainerRect;
        private Transform m_HandleTransform;
        private RectTransform m_HandleContainerRect;

        // The offset from handle position to mouse down position
        private Vector2 m_Offset = Vector2.zero;

        // field is never assigned warning
        #pragma warning disable 649
        private DrivenRectTransformTracker m_Tracker;
        #pragma warning restore 649

        // This "delayed" mechanism is required for case 1037681.
        private bool m_DelayedUpdateVisuals = false;

        // Size of each step.
        private float stepSize => wholeNumbers ? 1 : (maxValue - minValue) * 0.1f;

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            if (wholeNumbers)
            {
                m_MinValue = Mathf.Round(m_MinValue);
                m_MaxValue = Mathf.Round(m_MaxValue);
            }

            //OnValidate is called before OnEnabled. We need to make sure not to touch any other objects before OnEnable is run.
            if (IsActive())
            {
                UpdateCachedReferences();
                // Update rects in next update since other things might affect them even if value didn't change.
                m_DelayedUpdateVisuals = true;
            }

            if (!UnityEditor.PrefabUtility.IsPartOfPrefabAsset(this) && !Application.isPlaying)
                CanvasUpdateRegistry.RegisterCanvasElementForLayoutRebuild(this);
        }

#endif // if UNITY_EDITOR

        public virtual void Rebuild(CanvasUpdate executing)
        {
#if UNITY_EDITOR
            if (executing == CanvasUpdate.Prelayout)
                onValueChanged.Invoke(value);
#endif
        }
        
        public virtual void LayoutComplete() {}
        
        public virtual void GraphicUpdateComplete() {}

        protected override void OnEnable()
        {
            base.OnEnable();
            UpdateCachedReferences();
            Set(m_Value, false);
            // Update rects since they need to be initialized correctly.
            UpdateVisuals();
        }

        protected override void OnDisable()
        {
            m_Tracker.Clear();
            base.OnDisable();
        }
        
        protected virtual void Update()
        {
            if (!m_DelayedUpdateVisuals) return;
            m_DelayedUpdateVisuals = false;
            Set(m_Value, false);
            UpdateVisuals();
        }

        protected override void OnDidApplyAnimationProperties()
        {
            m_Value = ClampValue(m_Value);
            var oldNormalizedValue = normalizedValue;
            if (m_FillContainerRect != null)
            {
                if (m_FillImage != null && m_FillImage.type == Image.Type.Filled)
                    oldNormalizedValue = m_FillImage.fillAmount;
                else
                    oldNormalizedValue = reverseValue ? 1 - m_FillRect.anchorMin[(int)axis] : m_FillRect.anchorMax[(int)axis];
            }
            else if (m_HandleContainerRect != null)
            {
                var anchorMin = m_HandleRect.anchorMin[(int)axis];
                oldNormalizedValue = reverseValue ? 1 - anchorMin : anchorMin;
            }

            UpdateVisuals();

            if (oldNormalizedValue - normalizedValue == 0) return;
            UISystemProfilerApi.AddMarker("Slider.value", this);
            onValueChanged.Invoke(m_Value);
        }

        private void UpdateCachedReferences()
        {
            if (m_FillRect && m_FillRect != (RectTransform)transform)
            {
                m_FillTransform = m_FillRect.transform;
                m_FillImage = m_FillRect.GetComponent<Image>();
                if (m_FillTransform.parent != null)
                    m_FillContainerRect = m_FillTransform.parent.GetComponent<RectTransform>();
            }
            else
            {
                m_FillRect = null;
                m_FillContainerRect = null;
                m_FillImage = null;
            }

            if (m_HandleRect && m_HandleRect != (RectTransform)transform)
            {
                m_HandleTransform = m_HandleRect.transform;
                if (m_HandleTransform.parent != null)
                    m_HandleContainerRect = m_HandleTransform.parent.GetComponent<RectTransform>();
            }
            else
            {
                m_HandleRect = null;
                m_HandleContainerRect = null;
            }
        }

        private float ClampValue(float input)
        {
            var newValue = Mathf.Clamp(input, minValue, maxValue);
            if (wholeNumbers)
                newValue = Mathf.Round(newValue);
            return newValue;
        }
        
        protected virtual void Set(float input, bool sendCallback = true)
        {
            var newValue = ClampValue(input);
            
            if (m_Value - newValue == 0) return;

            m_Value = newValue;
            UpdateVisuals();
            if (!sendCallback) return;
            UISystemProfilerApi.AddMarker("Slider.value", this);
            m_OnValueChanged.Invoke(newValue);
        }

        protected override void OnRectTransformDimensionsChange()
        {
            base.OnRectTransformDimensionsChange();
            
            if (!IsActive()) return;
            UpdateVisuals();
        }

        private enum Axis
        {
            Horizontal = 0,
            Vertical = 1
        }

        private Axis axis => m_Direction == DirectionSlider.LeftToRight || m_Direction == DirectionSlider.RightToLeft ? Axis.Horizontal : Axis.Vertical;
        private bool reverseValue => m_Direction == DirectionSlider.RightToLeft || m_Direction == DirectionSlider.TopToBottom;

        private void UpdateVisuals()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                UpdateCachedReferences();
#endif

            m_Tracker.Clear();

            Vector2 anchorMin;
            Vector2 anchorMax;
            
            if (m_FillContainerRect != null)
            {
                m_Tracker.Add(this, m_FillRect, DrivenTransformProperties.Anchors);
                anchorMin = Vector2.zero;
                anchorMax = Vector2.one;

                if (m_FillImage != null && m_FillImage.type == Image.Type.Filled)
                {
                    m_FillImage.fillAmount = normalizedValue;
                }
                else
                {
                    if (reverseValue)
                        anchorMin[(int)axis] = 1 - normalizedValue;
                    else
                        anchorMax[(int)axis] = normalizedValue;
                }

                m_FillRect.anchorMin = anchorMin;
                m_FillRect.anchorMax = anchorMax;
            }

            if (m_HandleContainerRect == null) return;

            m_Tracker.Add(this, m_HandleRect, DrivenTransformProperties.Anchors);
            anchorMin = Vector2.zero;
            anchorMax = Vector2.one;
            anchorMin[(int)axis] = anchorMax[(int)axis] = reverseValue ? 1 - normalizedValue : normalizedValue;
            m_HandleRect.anchorMin = anchorMin;
            m_HandleRect.anchorMax = anchorMax;
        }

        private void UpdateDrag(PointerEventData eventData, Camera cam)
        {
            var clickRect = m_HandleContainerRect ? m_HandleContainerRect : m_FillContainerRect;
            if (clickRect == null || !(clickRect.rect.size[(int)axis] > 0)) return;
            
            var position = Vector2.zero;
            if (!MultipleDisplayUtilities.GetRelativeMousePositionForDrag(eventData, ref position))
                return;

            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(clickRect, position, cam, out var localCursor))
                return;
            localCursor -= clickRect.rect.position;

            var val = Mathf.Clamp01((localCursor - m_Offset)[(int)axis] / clickRect.rect.size[(int)axis]);
            normalizedValue = reverseValue ? 1f - val : val;
        }

        private bool MayDrag(PointerEventData eventData)
        {
            return IsActive() && IsInteractable() && eventData.button == PointerEventData.InputButton.Left;
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            if (!MayDrag(eventData))
                return;

            base.OnPointerDown(eventData);

            m_Offset = Vector2.zero;
            if (m_HandleContainerRect != null && RectTransformUtility.RectangleContainsScreenPoint(m_HandleRect, eventData.pointerPressRaycast.screenPosition, eventData.enterEventCamera))
            {
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(m_HandleRect, eventData.pointerPressRaycast.screenPosition, eventData.pressEventCamera, out var localMousePos))
                    m_Offset = localMousePos;
            }
            else
            {
                // Outside the slider handle - jump to this point instead
                UpdateDrag(eventData, eventData.pressEventCamera);
            }
        }

        public virtual void OnDrag(PointerEventData eventData)
        {
            if (!MayDrag(eventData))
                return;
            UpdateDrag(eventData, eventData.pressEventCamera);
        }

        public override void OnMove(AxisEventData eventData)
        {
            if (!IsActive() || !IsInteractable())
            {
                base.OnMove(eventData);
                return;
            }

            switch (eventData.moveDir)
            {
                case MoveDirection.Left:
                    if (axis == Axis.Horizontal && FindSelectableOnLeft() == null)
                        Set(reverseValue ? value + stepSize : value - stepSize);
                    else
                        base.OnMove(eventData);
                    break;
                case MoveDirection.Right:
                    if (axis == Axis.Horizontal && FindSelectableOnRight() == null)
                        Set(reverseValue ? value - stepSize : value + stepSize);
                    else
                        base.OnMove(eventData);
                    break;
                case MoveDirection.Up:
                    if (axis == Axis.Vertical && FindSelectableOnUp() == null)
                        Set(reverseValue ? value - stepSize : value + stepSize);
                    else
                        base.OnMove(eventData);
                    break;
                case MoveDirection.Down:
                    if (axis == Axis.Vertical && FindSelectableOnDown() == null)
                        Set(reverseValue ? value + stepSize : value - stepSize);
                    else
                        base.OnMove(eventData);
                    break;
            }
        }
        
        /// <summary>
        /// See CustomSelectable.FindSelectableOnLeft
        /// </summary>
        public override Selectable FindSelectableOnLeft()
        {
            if (navigation.mode == Navigation.Mode.Automatic && axis == Axis.Horizontal)
                return null;
            return base.FindSelectableOnLeft();
        }

        /// <summary>
        /// See CustomSelectable.FindSelectableOnRight
        /// </summary>
        public override Selectable FindSelectableOnRight()
        {
            if (navigation.mode == Navigation.Mode.Automatic && axis == Axis.Horizontal)
                return null;
            return base.FindSelectableOnRight();
        }

        /// <summary>
        /// See CustomSelectable.FindSelectableOnUp
        /// </summary>
        public override Selectable FindSelectableOnUp()
        {
            if (navigation.mode == Navigation.Mode.Automatic && axis == Axis.Vertical)
                return null;
            return base.FindSelectableOnUp();
        }

        /// <summary>
        /// See CustomSelectable.FindSelectableOnDown
        /// </summary>
        public override Selectable FindSelectableOnDown()
        {
            if (navigation.mode == Navigation.Mode.Automatic && axis == Axis.Vertical)
                return null;
            return base.FindSelectableOnDown();
        }

        public virtual void OnInitializePotentialDrag(PointerEventData eventData)
        {
            eventData.useDragThreshold = false;
        }
        
        public void SetDirection(DirectionSlider directionSlider, bool includeRectLayouts)
        {
            var oldAxis = axis;
            var oldReverse = reverseValue;
            direction = directionSlider;

            if (!includeRectLayouts)
                return;

            if (axis != oldAxis)
                RectTransformUtility.FlipLayoutAxes(transform as RectTransform, true, true);

            if (reverseValue != oldReverse)
                RectTransformUtility.FlipLayoutOnAxis(transform as RectTransform, (int)axis, true, true);
        }
    }
}