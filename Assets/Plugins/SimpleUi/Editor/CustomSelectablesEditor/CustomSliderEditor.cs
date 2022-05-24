using UnityEngine.UI;
using UnityEditor;
using UnityEngine;

namespace CustomSelectables
{
    [CustomEditor(typeof(CustomSlider), true)]
    public class CustomSliderEditor : CustomSelectablesEditor
    {
        private SerializedProperty _direction;
        private SerializedProperty _fillRect;
        private SerializedProperty _handleRect;
        private SerializedProperty _minValue;
        private SerializedProperty _maxValue;
        private SerializedProperty _wholeNumbers;
        private SerializedProperty _value;
        private SerializedProperty _onValueChanged;

        protected override void OnEnable()
        {
            base.OnEnable();
            _fillRect = serializedObject.FindProperty("m_FillRect");
            _handleRect = serializedObject.FindProperty("m_HandleRect");
            _direction = serializedObject.FindProperty("m_Direction");
            _minValue = serializedObject.FindProperty("m_MinValue");
            _maxValue = serializedObject.FindProperty("m_MaxValue");
            _wholeNumbers = serializedObject.FindProperty("m_WholeNumbers");
            _value = serializedObject.FindProperty("m_Value");
            _onValueChanged = serializedObject.FindProperty("m_OnValueChanged");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.Space();

            serializedObject.Update();

            EditorGUILayout.PropertyField(_fillRect);
            EditorGUILayout.PropertyField(_handleRect);

            if (_fillRect.objectReferenceValue != null || _handleRect.objectReferenceValue != null)
            {
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(_direction);
                if (EditorGUI.EndChangeCheck())
                {
                    var directionSlider = (CustomSlider.DirectionSlider)_direction.enumValueIndex;
                    foreach (var obj in serializedObject.targetObjects)
                    {
                        var slider = obj as CustomSlider;
                        slider.SetDirection(directionSlider, true);
                    }
                }


                EditorGUI.BeginChangeCheck();
                var newMin = EditorGUILayout.FloatField("Min Value", _minValue.floatValue);
                if (EditorGUI.EndChangeCheck() && newMin <= _maxValue.floatValue)
                {
                    _minValue.floatValue = newMin;
                }

                EditorGUI.BeginChangeCheck();
                var newMax = EditorGUILayout.FloatField("Max Value", _maxValue.floatValue);
                if (EditorGUI.EndChangeCheck() && newMax >= _minValue.floatValue)
                {
                    _maxValue.floatValue = newMax;
                }

                EditorGUILayout.PropertyField(_wholeNumbers);
                EditorGUILayout.Slider(_value, _minValue.floatValue, _maxValue.floatValue);

                var warning = false;
                foreach (var obj in serializedObject.targetObjects)
                {
                    var slider = obj as CustomSlider;
                    var dir = slider.direction;
                    if (dir == CustomSlider.DirectionSlider.LeftToRight || dir == CustomSlider.DirectionSlider.RightToLeft)
                        warning = slider.navigation.mode != Navigation.Mode.Automatic 
                                  && (slider.FindSelectableOnLeft() != null || slider.FindSelectableOnRight() != null);
                    else
                        warning = slider.navigation.mode != Navigation.Mode.Automatic 
                                  && (slider.FindSelectableOnDown() != null || slider.FindSelectableOnUp() != null);
                }

                if (warning)
                    EditorGUILayout.HelpBox("The selected slider direction conflicts with navigation. Not all navigation options may work.", MessageType.Warning);
            }
            else
            {
                EditorGUILayout.HelpBox("Specify a RectTransform for the slider fill or the slider handle or both. Each must have a parent RectTransform that it can slide within.", MessageType.Info);
            }

            serializedObject.ApplyModifiedProperties();
        }

        protected override void DrawEvents()
        {
            base.DrawEvents();
            EditorGUILayout.PropertyField(_onValueChanged);
        }
    }
}
