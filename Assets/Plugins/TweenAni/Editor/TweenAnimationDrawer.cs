using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace TweenAni
{
    [CustomPropertyDrawer(typeof(TweenAnimation))]
    public class TweenAnimationDrawer : PropertyDrawer
    {
        Dictionary<string, ViewState> viewStates = new Dictionary<string, ViewState>();
        EditorDrawHelper drawHelper = new EditorDrawHelper();

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            string id = property.propertyPath;
            float height = viewStates.ContainsKey(id) ? viewStates[id].height : 0;
            return height + EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            string id = property.propertyPath;

            if (!viewStates.ContainsKey(id)) {
                viewStates.Add(id, new ViewState {
                    height = 0,
                    reorderableList = BuildReorderableList(property.FindPropertyRelative("tweenerPresets"))
                });
            }

            ViewState viewState = viewStates[id];

            var foldoutRect = new Rect(
                rect.x,
                rect.y,
                rect.width,
                EditorGUIUtility.singleLineHeight);

            var contentRect = new Rect(
                rect.x, 
                rect.y + EditorGUIUtility.singleLineHeight,
                rect.width,
                rect.height - EditorGUIUtility.singleLineHeight);

            drawHelper.OnGUI(contentRect);

            property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, label, true);
            if (property.isExpanded)
            {
                int indentCache = EditorGUI.indentLevel;
                EditorGUI.indentLevel += 2;
                drawHelper.DrawProperty(property.FindPropertyRelative("timeScale"));
                drawHelper.DrawProperty(property.FindPropertyRelative("loops"));
                drawHelper.DrawProperty(property.FindPropertyRelative("loopType"));
                EditorGUI.indentLevel = 0;

                drawHelper.DrawReorderableList(viewState.reorderableList);
                EditorGUI.indentLevel = indentCache;
            }

            viewState.height = drawHelper.GetHeight();
        }

        ReorderableList BuildReorderableList(SerializedProperty property)
        {
            var rList = new ReorderableList(property.serializedObject, property, true, true, true, true);

            rList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                SerializedProperty element = rList.serializedProperty.GetArrayElementAtIndex(index);
                EditorGUI.PropertyField(rect, element, true);
            };

            rList.elementHeightCallback = (index) =>
            {
                SerializedProperty element = rList.serializedProperty.GetArrayElementAtIndex(index);
                return EditorGUI.GetPropertyHeight(element);
            };

            rList.drawHeaderCallback = (Rect rect) =>
            {
                EditorGUI.LabelField(rect, "Sequence Editor");
            };

            rList.onRemoveCallback = (ReorderableList list) =>
            {
                ReorderableList.defaultBehaviours.DoRemoveButton(list);
            };

            rList.onAddCallback = (ReorderableList list) =>
            {
                var index = list.serializedProperty.arraySize;
                list.serializedProperty.arraySize++;
                list.index = index;
                var element = list.serializedProperty.GetArrayElementAtIndex(index);
                element.isExpanded = true;
            };

            return rList;
        }

        private class ViewState
        {
            public float height;
            public ReorderableList reorderableList;
        }
    }
}