using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.SceneManagement;
using UnityEditor.AnimatedValues;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

namespace CustomSelectables
{
    [CustomEditor(typeof(CustomSelectable), true)]
    public class CustomSelectablesEditor : Editor
    {
        private SerializedProperty _automaticNavigationDirectionProperty;
        private SerializedProperty _onEntryPointerSelectingProperty;
        private SerializedProperty _script;
        private SerializedProperty _interactableProperty;
        private SerializedProperty _targetGraphicProperty;
        private SerializedProperty _transitionProperty;
        private SerializedProperty _colorBlockProperty;
        private SerializedProperty _spriteStateProperty;
        private SerializedProperty _animTriggerProperty;
        private SerializedProperty _navigationProperty;
        
        // Events
        private SerializedProperty _onEnableProperty;
        private SerializedProperty _onDisableProperty;
        private SerializedProperty _onEnterProperty;
        private SerializedProperty _onExitProperty;
        private SerializedProperty _onSelectProperty;
        private SerializedProperty _onDeselectProperty;
        
        private readonly GUIContent _visualizeNavigation = EditorGUIUtility.TrTextContent("Visualize", "Show navigation flows between selectable UI elements.");
        private readonly AnimBool _showColorTint       = new AnimBool();
        private readonly AnimBool _showSpriteTransition = new AnimBool();
        private readonly AnimBool _showAnimTransition  = new AnimBool();
        
        private static readonly List<CustomSelectablesEditor> Editors = new List<CustomSelectablesEditor>();
        private static bool _showNavigation;
        private static bool _showEvents;

        private const string ShowNavigationKey = "CustomSelectablesEditor.ShowNavigation";
        private const float KArrowThickness = 2.5f;
        private const float KArrowHeadSize = 1.2f;
        
        // Whenever adding new SerializedProperties to the Selectable and SelectableEditor
        // Also update this guy in OnEnable. This makes the inherited classes from Selectable not require a CustomEditor.
        private string[] _propertyPathToExcludeForChildClasses;
        
        protected virtual void OnEnable()
        {
            _script                = serializedObject.FindProperty("m_Script");
            _onEntryPointerSelectingProperty = serializedObject.FindProperty("pointerEnterSelecting");
            _automaticNavigationDirectionProperty = serializedObject.FindProperty("automaticNavigationDirection");
            _interactableProperty  = serializedObject.FindProperty("m_Interactable");
            _targetGraphicProperty = serializedObject.FindProperty("m_TargetGraphic");
            _transitionProperty    = serializedObject.FindProperty("m_Transition");
            _colorBlockProperty    = serializedObject.FindProperty("m_Colors");
            _spriteStateProperty   = serializedObject.FindProperty("m_SpriteState");
            _animTriggerProperty   = serializedObject.FindProperty("m_AnimationTriggers");
            _navigationProperty    = serializedObject.FindProperty("m_Navigation");
            
            _onEnableProperty      = serializedObject.FindProperty("m_OnEnable");
            _onDisableProperty     = serializedObject.FindProperty("m_OnDisable");
            _onEnterProperty       = serializedObject.FindProperty("m_OnEnter");
            _onExitProperty        = serializedObject.FindProperty("m_OnExit");
            _onSelectProperty      = serializedObject.FindProperty("m_OnSelect");
            _onDeselectProperty    = serializedObject.FindProperty("m_OnDeselect");
            
            _propertyPathToExcludeForChildClasses = new[]
            {
                _script.propertyPath,
                _navigationProperty.propertyPath,
                _transitionProperty.propertyPath,
                _colorBlockProperty.propertyPath,
                _spriteStateProperty.propertyPath,
                _animTriggerProperty.propertyPath,
                _interactableProperty.propertyPath,
                _targetGraphicProperty.propertyPath
            };
        
            var trans = GetTransition(_transitionProperty);
            _showColorTint.value       = trans == Selectable.Transition.ColorTint;
            _showSpriteTransition.value = trans == Selectable.Transition.SpriteSwap;
            _showAnimTransition.value  = trans == Selectable.Transition.Animation;
        
            _showColorTint.valueChanged.AddListener(Repaint);
            _showSpriteTransition.valueChanged.AddListener(Repaint);
        
            Editors.Add(this);
            RegisterStaticOnSceneGUI();
        
            _showNavigation = EditorPrefs.GetBool(ShowNavigationKey);
        }
        
        protected virtual void OnDisable()
        {
            _showColorTint.valueChanged.RemoveListener(Repaint);
            _showSpriteTransition.valueChanged.RemoveListener(Repaint);
        
            Editors.Remove(this);
            RegisterStaticOnSceneGUI();
        }
        
        private static void RegisterStaticOnSceneGUI()
        {
            SceneView.duringSceneGui -= StaticOnSceneGUI;
            if (Editors.Count > 0)
                SceneView.duringSceneGui += StaticOnSceneGUI;
        }

        private static Selectable.Transition GetTransition(SerializedProperty transition) => (Selectable.Transition)transition.enumValueIndex;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
        
            EditorGUILayout.PropertyField(_onEntryPointerSelectingProperty);
            EditorGUILayout.PropertyField(_interactableProperty);
        
            var trans = GetTransition(_transitionProperty);
        
            var graphic = _targetGraphicProperty.objectReferenceValue as Graphic;
            if (graphic == null) graphic = (target as Selectable)?.GetComponent<Graphic>();
        
            var animator = (target as Selectable)?.GetComponent<Animator>();
            _showColorTint.target = !_transitionProperty.hasMultipleDifferentValues && trans == Selectable.Transition.ColorTint;
            _showSpriteTransition.target = !_transitionProperty.hasMultipleDifferentValues && trans == Selectable.Transition.SpriteSwap;
            _showAnimTransition.target = !_transitionProperty.hasMultipleDifferentValues && trans == Selectable.Transition.Animation;
        
            EditorGUILayout.PropertyField(_transitionProperty);
        
            ++EditorGUI.indentLevel;
            {
                if (trans == Selectable.Transition.ColorTint || trans == Selectable.Transition.SpriteSwap)
                {
                    EditorGUILayout.PropertyField(_targetGraphicProperty);
                }
        
                switch (trans)
                {
                    case Selectable.Transition.ColorTint:
                        if (graphic == null)
                            EditorGUILayout.HelpBox("You must have a Graphic target in order to use a color transition.", MessageType.Warning);
                        break;
        
                    case Selectable.Transition.SpriteSwap:
                        if (graphic as Image == null)
                            EditorGUILayout.HelpBox("You must have a Image target in order to use a sprite swap transition.", MessageType.Warning);
                        break;
                }
        
                if (EditorGUILayout.BeginFadeGroup(_showColorTint.faded))
                {
                    EditorGUILayout.PropertyField(_colorBlockProperty);
                }
                EditorGUILayout.EndFadeGroup();
        
                if (EditorGUILayout.BeginFadeGroup(_showSpriteTransition.faded))
                {
                    EditorGUILayout.PropertyField(_spriteStateProperty);
                }
                EditorGUILayout.EndFadeGroup();
        
                if (EditorGUILayout.BeginFadeGroup(_showAnimTransition.faded))
                {
                    EditorGUILayout.PropertyField(_animTriggerProperty);
        
                    if (animator == null || animator.runtimeAnimatorController == null)
                    {
                        Rect buttonRect = EditorGUILayout.GetControlRect();
                        buttonRect.xMin += EditorGUIUtility.labelWidth;
                        if (GUI.Button(buttonRect, "Auto Generate Animation", EditorStyles.miniButton))
                        {
                            var controller = GenerateSelectableAnimatorController((target as Selectable)?.animationTriggers, target as Selectable);
                            if (controller != null)
                            {
                                if (animator == null)
                                    animator = (target as Selectable)?.gameObject.AddComponent<Animator>();
        
                                AnimatorController.SetAnimatorController(animator, controller);
                            }
                        }
                    }
                }
                EditorGUILayout.EndFadeGroup();
            }
            --EditorGUI.indentLevel;
        
            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(_automaticNavigationDirectionProperty);
            EditorGUILayout.PropertyField(_navigationProperty);

            EditorGUI.BeginChangeCheck();
            Rect toggleRect = EditorGUILayout.GetControlRect();
            toggleRect.xMin += EditorGUIUtility.labelWidth;
            _showNavigation = GUI.Toggle(toggleRect, _showNavigation, _visualizeNavigation, EditorStyles.miniButton);
            if (EditorGUI.EndChangeCheck())
            {
                EditorPrefs.SetBool(ShowNavigationKey, _showNavigation);
                SceneView.RepaintAll();
            }
        
            // We do this here to avoid requiring the user to also write a Editor for their Selectable-derived classes.
            // This way if we are on a derived class we dont draw anything else, otherwise draw the remaining properties.
            ChildClassPropertiesGUI();

            _showEvents = EditorGUILayout.Foldout(_showEvents, "Events");
            if (_showEvents)
                DrawEvents();
            
            serializedObject.ApplyModifiedProperties();
        }

        protected virtual void DrawEvents()
        {
            EditorGUILayout.PropertyField(_onEnableProperty);
            EditorGUILayout.PropertyField(_onDisableProperty);
            EditorGUILayout.PropertyField(_onSelectProperty);
            EditorGUILayout.PropertyField(_onDeselectProperty);
            EditorGUILayout.PropertyField(_onEnterProperty);
            EditorGUILayout.PropertyField(_onExitProperty);
        }
        
        // Draw the extra SerializedProperties of the child class.
        // We need to make sure that m_PropertyPathToExcludeForChildClasses has all the Selectable properties and in the correct order.
        // TODO: find a nicer way of doing this. (creating a InheritedEditor class that automagically does this)
        private void ChildClassPropertiesGUI()
        {
            if (IsDerivedSelectableEditor())
                return;
        
            DrawPropertiesExcluding(serializedObject, _propertyPathToExcludeForChildClasses);
        }
        
        private bool IsDerivedSelectableEditor()
        {
            return GetType() != typeof(SelectableEditor);
        }
        
        private static AnimatorController GenerateSelectableAnimatorController(AnimationTriggers animationTriggers, Selectable target)
        {
            if (target == null)
                return null;
        
            // Where should we create the controller?
            var path = GetSaveControllerPath(target);
            if (string.IsNullOrEmpty(path))
                return null;
        
            // figure out clip names
            var normalName = string.IsNullOrEmpty(animationTriggers.normalTrigger) ? "Normal" : animationTriggers.normalTrigger;
            var highlightedName = string.IsNullOrEmpty(animationTriggers.highlightedTrigger) ? "Highlighted" : animationTriggers.highlightedTrigger;
            var pressedName = string.IsNullOrEmpty(animationTriggers.pressedTrigger) ? "Pressed" : animationTriggers.pressedTrigger;
            var selectedName = string.IsNullOrEmpty(animationTriggers.selectedTrigger) ? "Selected" : animationTriggers.selectedTrigger;
            var disabledName = string.IsNullOrEmpty(animationTriggers.disabledTrigger) ? "Disabled" : animationTriggers.disabledTrigger;
        
            // Create controller and hook up transitions.
            var controller = AnimatorController.CreateAnimatorControllerAtPath(path);
            GenerateTriggerableTransition(normalName, controller);
            GenerateTriggerableTransition(highlightedName, controller);
            GenerateTriggerableTransition(pressedName, controller);
            GenerateTriggerableTransition(selectedName, controller);
            GenerateTriggerableTransition(disabledName, controller);
        
            AssetDatabase.ImportAsset(path);
        
            return controller;
        }
        
        private static string GetSaveControllerPath(Component target)
        {
            var defaultName = target.gameObject.name;
            var message = $"Create a new animator for the game object '{defaultName}':";
            return EditorUtility.SaveFilePanelInProject("New Animation Controller", defaultName, "controller", message);
        }
        
        private static void SetUpCurves(AnimationClip highlightedClip, AnimationClip pressedClip, string animationPath)
        {
            string[] channels = { "m_LocalScale.x", "m_LocalScale.y", "m_LocalScale.z" };
        
            var highlightedKeys = new[] { new Keyframe(0f, 1f), new Keyframe(0.5f, 1.1f), new Keyframe(1f, 1f) };
            var highlightedCurve = new AnimationCurve(highlightedKeys);
            foreach (var channel in channels)
                AnimationUtility.SetEditorCurve(highlightedClip, EditorCurveBinding.FloatCurve(animationPath, typeof(Transform), channel), highlightedCurve);
        
            var pressedKeys = new[] { new Keyframe(0f, 1.15f) };
            var pressedCurve = new AnimationCurve(pressedKeys);
            foreach (var channel in channels)
                AnimationUtility.SetEditorCurve(pressedClip, EditorCurveBinding.FloatCurve(animationPath, typeof(Transform), channel), pressedCurve);
        }

        private static void GenerateTriggerableTransition(string name, AnimatorController controller)
        {
            // Create the clip
            var clip = AnimatorController.AllocateAnimatorClip(name);
            AssetDatabase.AddObjectToAsset(clip, controller);
        
            // Create a state in the animator controller for this clip
            var state = controller.AddMotion(clip);
        
            // Add a transition property
            controller.AddParameter(name, AnimatorControllerParameterType.Trigger);
        
            // Add an any state transition
            var stateMachine = controller.layers[0].stateMachine;
            var transition = stateMachine.AddAnyStateTransition(state);
            transition.AddCondition(AnimatorConditionMode.If, 0, name);
        }
        
        private static void StaticOnSceneGUI(SceneView view)
        {
            if (!_showNavigation)  return;
        
            foreach (var s in Selectable.allSelectablesArray)
            {
                if (StageUtility.IsGameObjectRenderedByCamera(s.gameObject, Camera.current))
                    DrawNavigationForSelectable(s);
            }
        }
        
        private static void DrawNavigationForSelectable(Selectable sel)
        {
            if (sel == null) return;
        
            var transform = sel.transform;
            var active = Selection.transforms.Any(e => e == transform);
        
            Handles.color = new Color(1.0f, 0.6f, 0.2f, active ? 1.0f : 0.4f);
            DrawNavigationArrow(-Vector2.right, sel, sel.FindSelectableOnLeft());
            DrawNavigationArrow(Vector2.up, sel, sel.FindSelectableOnUp());
        
            Handles.color = new Color(1.0f, 0.9f, 0.1f, active ? 1.0f : 0.4f);
            DrawNavigationArrow(Vector2.right, sel, sel.FindSelectableOnRight());
            DrawNavigationArrow(-Vector2.up, sel, sel.FindSelectableOnDown());
        }
        
        private static void DrawNavigationArrow(Vector2 direction, Component fromObj, Component toObj)
        {
            if (fromObj == null || toObj == null) return;
            
            var fromTransform = fromObj.transform;
            var toTransform = toObj.transform;
        
            var sideDir = new Vector2(direction.y, -direction.x);
            var fromPoint = fromTransform.TransformPoint(GetPointOnRectEdge(fromTransform as RectTransform, direction));
            var toPoint = toTransform.TransformPoint(GetPointOnRectEdge(toTransform as RectTransform, -direction));
            var fromSize = HandleUtility.GetHandleSize(fromPoint) * 0.05f;
            var toSize = HandleUtility.GetHandleSize(toPoint) * 0.05f;
            
            fromPoint += fromTransform.TransformDirection(sideDir) * fromSize;
            toPoint += toTransform.TransformDirection(sideDir) * toSize;
            
            var length = Vector3.Distance(fromPoint, toPoint);
            var fromTangent = fromTransform.rotation * direction * length * 0.3f;
            var rotation = toTransform.rotation;
            var toTangent = rotation * -direction * length * 0.3f;
        
            Handles.DrawBezier(fromPoint, toPoint, fromPoint + fromTangent, toPoint + toTangent, Handles.color, null, KArrowThickness);
            Handles.DrawAAPolyLine(KArrowThickness, toPoint, toPoint + rotation * (-direction - sideDir) * toSize * KArrowHeadSize);
            Handles.DrawAAPolyLine(KArrowThickness, toPoint, toPoint + rotation * (-direction + sideDir) * toSize * KArrowHeadSize);
        }
        
        private static Vector3 GetPointOnRectEdge(RectTransform rect, Vector2 dir)
        {
            if (rect == null) return Vector3.zero;
            
            if (dir != Vector2.zero) dir /= Mathf.Max(Mathf.Abs(dir.x), Mathf.Abs(dir.y));
            
            return rect.rect.center + Vector2.Scale(rect.rect.size, dir * 0.5f);
        }
    }
}