using System;
using UnityEngine;

namespace Runtime.Game.Ui.Objects.General
{
    public class CustomUiObject : MonoBehaviour
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
    }
}