using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace CustomSelectables
{
    [AddComponentMenu("UI/Custom/Custom Toggle")]
    [RequireComponent(typeof(RectTransform))]
    public class CustomToggle : CustomSelectable, IPointerClickHandler, ISubmitHandler, ICanvasElement
    {
        [Serializable]
        public class ToggleEvent : UnityEvent<bool>{}

        [SerializeField]
        private CustomToggleGroup m_Group;
        
        public CustomToggleGroup group
        {
            get => m_Group;
            set => SetToggleGroup(value, true);
        }

        [Tooltip("Is the toggle currently on or off?")]
        [SerializeField]
        private bool m_IsOn;
        
        public bool isOn
        {
            get => m_IsOn;
            set => Set(value);
        }
        
        [FormerlySerializedAs("onValueChanged")] [SerializeField]
        private ToggleEvent m_OnValueChanged = new ToggleEvent();
        
        public ToggleEvent onValueChanged
        {
            get => m_OnValueChanged;
            set => m_OnValueChanged = value;
        }

//----------------------UNITY-EVENTS---------------------
        
#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            if (!UnityEditor.PrefabUtility.IsPartOfPrefabAsset(this) && !Application.isPlaying)
                CanvasUpdateRegistry.RegisterCanvasElementForLayoutRebuild(this);
        }

#endif // if UNITY_EDITOR

        public virtual void Rebuild(CanvasUpdate executing)
        {
#if UNITY_EDITOR
            if (executing == CanvasUpdate.Prelayout)
                onValueChanged.Invoke(m_IsOn);
#endif
        }

        public virtual void LayoutComplete() {}

        public virtual void GraphicUpdateComplete() {}

        protected override void OnDestroy()
        {
            if (m_Group == null) return;
            m_Group.EnsureValidState();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            SetToggleGroup(m_Group, false);
        }

        protected override void OnDisable()
        {
            SetToggleGroup(null, false);
            base.OnDisable();
        }
        
        private void SetToggleGroup(CustomToggleGroup newGroup, bool setMemberValue)
        {
            if (m_Group != null)
                m_Group.UnregisterToggle(this);
            
            if (setMemberValue)
                m_Group = newGroup;
            
            if (newGroup != null && IsActive())
                newGroup.RegisterToggle(this);
            
            if (newGroup != null && isOn && IsActive())
                newGroup.NotifyToggleOn(this);
        }
        
//-------------------------------------------------------

//-------------------ON-CHANGE-VALUE---------------------
        
        private void InternalToggle()
        {
            if (!IsActive() || !IsInteractable())
                return;

            isOn = !isOn;
        }
        
        private void Set(bool value, bool sendCallback = true)
        {
            if (m_IsOn == value) return;
            
            m_IsOn = value;
            if (m_Group != null && m_Group.isActiveAndEnabled && IsActive())
            {
                if (m_IsOn || (!m_Group.AnyTogglesOn() && !m_Group.allowSwitchOff))
                {
                    m_IsOn = true;
                    m_Group.NotifyToggleOn(this, sendCallback);
                }
            }

            if (!sendCallback) return;
            UISystemProfilerApi.AddMarker("Toggle.value", this);
            onValueChanged.Invoke(m_IsOn);
        }
        
        public void SetIsOnWithoutNotify(bool value)
        {
            Set(value, false);
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            InternalToggle();
        }

        public virtual void OnSubmit(BaseEventData eventData)
        {
            InternalToggle();
        }
    }
}