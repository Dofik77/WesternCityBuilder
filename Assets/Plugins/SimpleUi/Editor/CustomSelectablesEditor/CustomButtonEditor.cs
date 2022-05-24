using UnityEditor;
using UnityEditor.UI;

namespace CustomSelectables
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

        protected override void DrawEvents()
        {
            base.DrawEvents();
            EditorGUILayout.PropertyField(_onClickProperty);
        }
    }
}