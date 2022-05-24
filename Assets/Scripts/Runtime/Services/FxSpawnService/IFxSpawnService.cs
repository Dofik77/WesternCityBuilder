using UnityEngine;

namespace Runtime.Services.FxSpawnService
{
    public interface IFxSpawnService
    {
        GameObject SpawnParticle(string name);
    }
}