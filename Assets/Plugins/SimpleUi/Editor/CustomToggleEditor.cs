using CustomSelectables;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.UI;
using UnityEngine.UI;

namespace CustomSelectablesEditor
{
    [CustomEditor(typeof(CustomToggle), true)]
    [CanEditMultipleObjects]
    public class CustomToggleEditor : CustomSelectablesEditor
    {
        private SerializedProperty _onValueChanged;
        private SerializedProperty _groupProperty;
        private SerializedProperty _isOnProperty;

        protected override void OnEnable()
        {
            base.OnEnable();
            _groupProperty = serializedObject.FindProperty("m_Group");
            _isOnProperty = serializedObject.FindProperty("m_IsOn");
            _onValueChanged = serializedObject.FindProperty("m_OnValueChanged");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            var toggle = serializedObject.targetObject as CustomToggle;
            
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(_isOnProperty);
            if (EditorGUI.EndChangeCheck())
            {
                EditorSceneManager.MarkSceneDirty(toggle.gameObject.scene);
                var group = _groupProperty.objectReferenceValue as CustomToggleGroup;

                toggle.isOn = _isOnProperty.boolValue;

                if (group != null && group.isActiveAndEnabled && toggle.IsActive())
                {
                    if (toggle.isOn || (!group.AnyTogglesOn() && !group.allowSwitchOff))
                    {
                        toggle.isOn = true;
                        group.NotifyToggleOn(toggle);
                    }
                }
            }
            
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(_groupProperty);
            if (EditorGUI.EndChangeCheck())
            {
                EditorSceneManager.MarkSceneDirty(toggle.gameObject.scene);
                var group = _groupProperty.objectReferenceValue as CustomToggleGroup;
                toggle.group = group;
            }

            EditorGUILayout.Space();
            
            base.OnInspectorGUI();
            EditorGUILayout.PropertyField(_onValueChanged);

            serializedObject.ApplyModifiedProperties();
        }
    }
}