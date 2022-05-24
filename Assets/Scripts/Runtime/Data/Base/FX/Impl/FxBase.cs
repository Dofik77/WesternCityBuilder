using System;
using UnityEngine;

namespace Runtime.Data.Base.FX.Impl
{
    [CreateAssetMenu(menuName = "Bases/FxBase", fileName = "FxBase")]
    public class FxBase : ScriptableObject, IFxBase
    {
        [SerializeField] private Fx[] Fxs;

        [Serializable]
        private class Fx
        {
            public string Name;
            public GameObject ParticleSystem;
        }

        public GameObject Get(string key)
        {
            foreach (var t in Fxs)
            {
                var clip = t;
                if (clip.Name == key)
                    return t.ParticleSystem;
            }

            throw new Exception("[FxBase] Can't find FX with name: " + key);
        }
    }
}