using System;
using UnityEngine;

namespace Runtime.Game.Utils.MonoBehUtils
{
    public class ScreenVariables : MonoBehaviour
    {
        
        [SerializeField] private TransformPoint[] _points;
        [SerializeField] private FloatValue[] _floats;
        [SerializeField] private IntValue[] _ints;
        [SerializeField] private MaterialValue[] _materials;

        [Serializable]
        private struct TransformPoint
        {
            public string Key;
            public Transform Transform;
        }
        
        [Serializable]
        private struct FloatValue
        {
            public string Key;
            public float Value;
        }

        [Serializable]
        private struct IntValue
        {
            public string Key;
            public int Value;
        }
        
        [Serializable]
        private struct MaterialValue
        {
            public string Key;
            public Material Value;
        }
        
        public Transform GetTransformPoint(string key)
        {
            foreach (var point in _points)
                if (key == point.Key)
                    return point.Transform;
            throw new Exception($"No position on scene with key {key} was found!");
        }
        
        public float GetFloatValue(string key)
        {
            foreach (var value in _floats)
                if (key == value.Key)
                    return value.Value;
            throw new Exception($"No position on scene with key {key} was found!");
        }
        
        public int GetIntValue(string key)
        {
            foreach (var value in _ints)
                if (key == value.Key)
                    return value.Value;
            throw new Exception($"No position on scene with key {key} was found!");
        }

        public Material GetMaterialValue(string key)
        {
            foreach (var value in _materials)
                if (key == value.Key)
                    return value.Value;
            throw new Exception($"No position on scene with key {key} was found!");
        }
    }
}