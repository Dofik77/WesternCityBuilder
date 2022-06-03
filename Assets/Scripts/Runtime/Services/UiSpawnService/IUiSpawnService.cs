using UnityEngine;

namespace Runtime.Services.UiSpawnService
{
    public interface IUiSpawnService
    {
        GameObject Spawn(string name, Transform parent = null);
    }
}