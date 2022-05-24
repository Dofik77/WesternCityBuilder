using System;
using Runtime.Data.PlayerData.Currency;
using Runtime.Game.Ui.Objects.Layouts;
using UnityEngine;

namespace Runtime.Data.PlayerData.Skins
{
    public interface ISkinsData
    {
        Skin[] Get();
    }
    
    [Serializable]
    public struct Skin : IProvideUiGeneratedEntity
    {
        public string GetLowerKey() => _key.ToLower();
        [Header("General")] [SerializeField] private string _key;
        
        public string GetName() => _name;
        [Header("Store")] [SerializeField] private string _name;
        public Sprite GetIcon() => _storeIcon;
        [SerializeField] private Sprite _storeIcon;
        public int GetCost() => _cost;
        [SerializeField] private int _cost;
        public ECurrency GetCurrency() => _currency;
        [SerializeField] private ECurrency _currency;
        
        public Material[] GetMaterials() => _materials;
        [Header("Render")] [SerializeField] private Material[] _materials;
        public Color[] GetColors() => _colors;
        [SerializeField] private Color[] _colors;
        public Gradient[] GetGradients() => _gradients;
        [SerializeField] private Gradient[] _gradients;
        public string[] GetParticlePrefabs() => _particlePrefabs;
        [SerializeField] private string[] _particlePrefabs;

        public float GetScaleMultiplier() => _scaleMultiplier;
        [Header("Transform")] [SerializeField] private float _scaleMultiplier;
    }
}