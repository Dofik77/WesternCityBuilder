using UnityEngine;

namespace Runtime.Data.PlayerData.Skins.Impls
{
    [CreateAssetMenu(menuName = "PlayerData/SkinsData", fileName = "SkinsData")]
    public class SkinsData : ScriptableObject, ISkinsData
    {
        public Skin[] Get() => _skins;
        [SerializeField] private Skin[] _skins;
    }
}