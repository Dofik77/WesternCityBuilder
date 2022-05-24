using Runtime.Data.Base.FX;
using UnityEngine;
using Zenject;

namespace Runtime.Services.FxSpawnService.Impls
{
    public class FxSpawnService : IFxSpawnService
    {
        private readonly DiContainer _container;
        private readonly IFxBase _fxBase;

        public FxSpawnService(
            DiContainer container,
            IFxBase fxBase)
        {
            _container = container;
            _fxBase = fxBase;
        }
        
        public GameObject SpawnParticle(string name)
        {
            return _container.InstantiatePrefab(_fxBase.Get(name), Vector3.zero, Quaternion.identity, null);
        }
    }
}