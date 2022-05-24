using System;
using UnityEngine;

namespace Runtime.Data.Base.Prefabs.Impl
{
    [CreateAssetMenu(menuName = "Bases/PrefabsBase", fileName = "PrefabsBase")]
    public class PrefabsBase : ScriptableObject, IPrefabsBase
    {
        [SerializeField] private Prefab[] prefabs;

        public GameObject Get(string prefabName)
        {
            for (var i = 0; i < prefabs.Length; i++)
            {
                var prefab = prefabs[i];
                if (prefab.Name == prefabName)
                    return prefab.GameObject;
            }

            throw new Exception("[PrefabsBase] Can't find prefab with name: " + prefabName);
        }

        [Serializable]
        public class Prefab
        {
            public string Name;
            public GameObject GameObject;
        }
    }
}