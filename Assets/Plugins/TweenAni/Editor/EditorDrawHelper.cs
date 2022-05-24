using System;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace TweenAni
{
    /// <summary>
    /// Helper class for custom property drawer.
    /// </summary>
    public class EditorDrawHelper
    {
        const float marginInRows = 2;
        const float marginInColumns = 5;
        const float omitWidth = 24;
        const string omitSymbol = "[..]";

        Vector2 startPos;
        Vector2 currentPos;
        Vector2 rowSize;

        float widthToTheRight => rowSize.x - (currentPos.x - startPos.x);

        public void OnGUI(Rect rect)
        {
            startPos = rect.position;
            currentPos = startPos;
            rowSize = new Vector2(rect.width, EditorGUIUtility.singleLineHeight);
        }

        public float GetHeight()
        {
            return currentPos.y - startPos.y;
        }

        public void DrawProperty(SerializedProperty property)
        {
            if (property == null) { Debug.LogError($"Serialized property is null."); return; }

            EditorGUI.PropertyField(GetRow(), property);
            NextRow();
        }

        public void DrawProperty(string label, SerializedProperty property, float fieldWidth = -1, float labelWidth = 60)
        {
            if (property == null) { Debug.LogError($"Serialized property of '{label}' is null."); return; }

            DrawPropertyInline(label, property, fieldWidth, labelWidth);
            EndOfInlineProperties();
        }

        public void DrawPropertyInline(string label, SerializedProperty property, float fieldWidth = -1, float labelWidth = 60)
        {
            if (property == null) { Debug.LogError($"Serialized property of '{label}' is null."); return; }

            if (labelWidth < widthToTheRight - omitWidth)
            {
                EditorGUI.LabelField(GetColumnInRow(labelWidth), label);
                NextColumn(labelWidth);
            }
            else
            {
                EditorGUI.LabelField(GetColumnInRow(omitWidth), omitSymbol);
                NextColumn(omitWidth);
                return;
            }

            if (fieldWidth < widthToTheRight - omitWidth)
            {
                var fieldAutoWidth = fieldWidth < 0 ? widthToTheRight : fieldWidth;
                EditorGUI.PropertyField(GetColumnInRow(fieldAutoWidth), property, GUIContent.none);
                NextColumn(fieldAutoWidth);
            }
            else
            {
                EditorGUI.LabelField(GetColumnInRow(omitWidth), omitSymbol);
                NextColumn(omitWidth);
                return;
            }
        }

        public void EndOfInlineProperties()
        {
            NextRow();
        }

        public void DrawEditorButton(string label, Action onButtonClick)
        {
            if (GUI.Button(GetRow(), label)) { onButtonClick?.Invoke(); }
            NextRow();
        }

        public void DrawReorderableList(ReorderableList reorderableList)
        {
            float height = reorderableList.GetHeight();
            reorderableList.DoList(GetRow(height));
            NextRow(height);
        }

        Rect GetRow()
        {
            return new Rect(currentPos, rowSize);
        }

        Rect GetRow(float height)
        {
            return new Rect(currentPos.x, currentPos.y, rowSize.x, height);
        }

        Rect GetColumnInRow(float width)
        {
            return new Rect(currentPos.x, currentPos.y, width, rowSize.y);
        }

        void NextRow()
        {
            currentPos = new Vector2(startPos.x, currentPos.y + rowSize.y + marginInRows);
        }

        void NextRow(float height)
        {
            currentPos = new Vector2(startPos.x, currentPos.y + height + marginInRows);
        }

        void NextColumn(float width)
        {
            currentPos.x += width + marginInColumns;
        }
    }
}