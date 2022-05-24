using System;
using Runtime.Data.Base.Prefabs.Impl;
using UnityEngine;
using Zenject;

namespace Runtime.Data.Base.Ui.Impl
{
    [CreateAssetMenu(menuName = "Bases/UiPrefabsBase", fileName = "UiPrefabsBase")]
    public class UiPrefabsBase : ScriptableObject, IUiPrefabsBase
    {
        [SerializeField] private PrefabsBase.Prefab[] prefabs;
        
        private readonly DiContainer _container;

        public GameObject Get(string prefabName)
        {
            for (var i = 0; i < prefabs.Length; i++)
            {
                var prefab = prefabs[i];
                if (prefab.Name == prefabName)
                    return prefab.GameObject;
            }

            throw new Exception("[PrefabsBase] Can't find ui prefab with name: " + prefabName);
        }
        
    }
}