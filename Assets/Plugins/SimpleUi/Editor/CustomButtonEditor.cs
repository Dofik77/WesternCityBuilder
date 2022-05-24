using CustomSelectables;
using UnityEditor;
using UnityEditor.UI;

namespace CustomSelectablesEditor
{
    [CustomEditor(typeof(CustomButton), true)]
    [CanEditMultipleObjects]
    public class CustomButtonEditor : CustomSelectablesEditor
    {
        private SerializedProperty _onClickProperty;

        protected override void OnEnable()
        {
            base.OnEnable();
            _onClickProperty = serializedObject.FindProperty("m_OnClick");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.Space();

            serializedObject.Update();
            EditorGUILayout.PropertyField(_onClickProperty);
            serializedObject.ApplyModifiedProperties();
        }
    }
}