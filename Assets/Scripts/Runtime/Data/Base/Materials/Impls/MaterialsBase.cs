using System;
using UnityEngine;

namespace Runtime.Data.Base.Materials.Impls
{
    [CreateAssetMenu(menuName = "Bases/MaterialsBase", fileName = "MaterialsBase")]
    public class MaterialsBase : ScriptableObject, IMaterialsBase
    {
        [SerializeField] private Material[] materials;

        public UnityEngine.Material Get(string name)
        {
            for (var i = 0; i < materials.Length; i++)
            {
                var material = materials[i];
                if (material.Name == name)
                    return material.UnityMaterial;
            }

            throw new Exception("[MaterialsBase] Can't find material with name: " + name);
        }

        [Serializable]
        public class Material
        {
            public string Name;
            public UnityEngine.Material UnityMaterial;
        }
    }
}