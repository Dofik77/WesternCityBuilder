using DG.Tweening;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TweenAni
{
    [CustomPropertyDrawer(typeof(TweenAnimation.TweenerPreset))]
    public class TweenerPresetDrawer : PropertyDrawer
    {
        Dictionary<string, ViewState> viewStates = new Dictionary<string, ViewState>();
        EditorDrawHelper helper = new EditorDrawHelper();

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            string id = property.propertyPath;
            float height = viewStates.ContainsKey(id) ? viewStates[id].height : 0;
            return height + EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            string id = property.propertyPath;

            if (!viewStates.ContainsKey(id)) {
                viewStates.Add(id, new ViewState { height = 0 });
            }

            ViewState viewState = viewStates[id];

            var rect = new Rect(position.x + 10, position.y, position.width - 10, position.height);

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

            helper.OnGUI(contentRect);

            var name = property.FindPropertyRelative("name");
            string labelText = string.IsNullOrEmpty(name?.stringValue) ? $"Tweener {label.text}" : name.stringValue;

            property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, labelText);
            if (property.isExpanded)
            {
                var disabled = property.FindPropertyRelative("disabled");
                var anchor = property.FindPropertyRelative("anchor");
                var insertAt = property.FindPropertyRelative("insertAt");
                var interval = property.FindPropertyRelative("interval");
                var tweenerType = property.FindPropertyRelative("tweenerType");
                var target = property.FindPropertyRelative("target");
                var endValueVector = property.FindPropertyRelative("endValueVector");
                var endValue = property.FindPropertyRelative("endValue");
                var endValueColor = property.FindPropertyRelative("endValueColor");
                var vibrato = property.FindPropertyRelative("vibrato");
                var elasticity = property.FindPropertyRelative("elasticity");
                var relative = property.FindPropertyRelative("relative");
                var duration = property.FindPropertyRelative("duration");
                var ease = property.FindPropertyRelative("ease");
                var overshoot = property.FindPropertyRelative("overshoot");
                var amplitude = property.FindPropertyRelative("amplitude");
                var period = property.FindPropertyRelative("period");
                var from = property.FindPropertyRelative("from");
                var fromRelative = property.FindPropertyRelative("fromRelative");
                var rotateMode = property.FindPropertyRelative("rotateMode");

                helper.DrawPropertyInline("Label", name, 120);
                helper.DrawPropertyInline("Disable", disabled);
                helper.EndOfInlineProperties();

                if (disabled.boolValue) { GUI.enabled = false; }

                helper.DrawPropertyInline("Anchor", anchor, 120);
                if (anchor.enumValueIndex == (int)TweenAnimation.Anchor.Insert) { helper.DrawPropertyInline("Insert At", insertAt); }
                bool isInterval = anchor.enumValueIndex == (int)TweenAnimation.Anchor.AppendInterval;
                if (isInterval) { helper.DrawPropertyInline("Interval", interval); }
                helper.EndOfInlineProperties();

                if (!isInterval)
                {
                    helper.DrawPropertyInline("Type", tweenerType, 120);
                    helper.DrawPropertyInline("Target", target);
                    helper.EndOfInlineProperties();

                    if (IsVectorEndValue(tweenerType.enumValueIndex)) { helper.DrawPropertyInline("End Value", endValueVector, 180); }
                    else if(IsColorEndValue(tweenerType.enumValueIndex)) { helper.DrawPropertyInline("End Value", endValueColor, 180);}
                    else { helper.DrawPropertyInline("End Value", endValue, 120); }

                    if (IsRotate(tweenerType.enumValueIndex))
                    {
                        helper.DrawPropertyInline("Rotate Mode", rotateMode, 180, 80);
                    }
                    
                    if (IsPunch(tweenerType.enumValueIndex))
                    {
                        helper.DrawPropertyInline("Vibrato", vibrato, 50);
                        helper.DrawPropertyInline("Elasticity", elasticity, 50);
                    }
                    
                    helper.DrawPropertyInline("Relative", relative);
                    helper.EndOfInlineProperties();

                    helper.DrawProperty("Duration", duration);
                    helper.DrawPropertyInline("Ease", ease, 120);
                    if (IsPeriodNeeded(ease.enumValueIndex)) { helper.DrawPropertyInline("Period", period, 40, 40); }
                    if (IsOvershootNeeded(ease.enumValueIndex)) { helper.DrawPropertyInline("Overshoot", overshoot, labelWidth: 65); }
                    if (IsAmplitudeNeeded(ease.enumValueIndex)) { helper.DrawPropertyInline("Amplitude", amplitude); }
                    helper.EndOfInlineProperties();

                    helper.DrawPropertyInline("From", from, 30);
                    if (from.boolValue == true) { helper.DrawPropertyInline("Relative", fromRelative, 30); }
                    helper.EndOfInlineProperties();
                }

                if (disabled.boolValue) { GUI.enabled = true; }
            }

            viewState.height = helper.GetHeight();
        }

        bool IsVectorEndValue(int tweenerTypeEnumIndex)
        {
            var tweenerType = (TweenAnimation.TweenerType)tweenerTypeEnumIndex;
            return tweenerType == TweenAnimation.TweenerType.LocalMove
                   || tweenerType == TweenAnimation.TweenerType.Scale
                   || tweenerType == TweenAnimation.TweenerType.AnchorPos
                   || tweenerType == TweenAnimation.TweenerType.Pivot
                   || tweenerType == TweenAnimation.TweenerType.SizeDelta
                   || tweenerType == TweenAnimation.TweenerType.LocalRotate
                   || tweenerType == TweenAnimation.TweenerType.PunchScale;
        }

        bool IsColorEndValue(int tweenerTypeEnumIndex)
        {
            var tweenerType = (TweenAnimation.TweenerType)tweenerTypeEnumIndex;
            return tweenerType == TweenAnimation.TweenerType.Color;
        }
        
        bool IsRotate(int tweenerTypeEnumIndex)
        {
            var tweenerType = (TweenAnimation.TweenerType)tweenerTypeEnumIndex;
            return tweenerType == TweenAnimation.TweenerType.LocalRotate;
        }
        

        bool IsPunch(int tweenerTypeEnumIndex)
        {
            var tweenerType = (TweenAnimation.TweenerType) tweenerTypeEnumIndex;
            return tweenerType == TweenAnimation.TweenerType.PunchScale;
        }

        bool IsOvershootNeeded(int easeTypeEnumIndex)
        {
            var ease = (Ease)easeTypeEnumIndex;
            return ease.IsBack() || ease.IsFlash();
        }

        bool IsAmplitudeNeeded(int easeTypeEnumIndex)
        {
            var ease = (Ease)easeTypeEnumIndex;
            return ease.IsElastic();
        }

        bool IsPeriodNeeded(int easeTypeEnumIndex)
        {
            var ease = (Ease)easeTypeEnumIndex;
            return ease.IsElastic() || ease.IsFlash();
        }

        private class ViewState
        {
            public float height;
        }
    }
}