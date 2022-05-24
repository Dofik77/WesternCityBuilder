using Runtime.Data.Base.Ui;
using UnityEngine;
using Zenject;

namespace Runtime.Services.UiSpawnService.Impls
{
    public class UiSpawnService : IUiSpawnService
    {
        private readonly DiContainer _container;
        private readonly IUiPrefabsBase _uiPrefabs;

        public UiSpawnService(
            DiContainer container,
            IUiPrefabsBase uiPrefabs)
        {
            _container = container;
            _uiPrefabs = uiPrefabs;
        }
        
        public GameObject Spawn(string name)
        {
            return _container.InstantiatePrefab(_uiPrefabs.Get(name), Vector3.zero, Quaternion.identity, null);
        }
    }
}