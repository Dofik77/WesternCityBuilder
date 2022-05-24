using UnityEngine;

namespace Runtime.Data.PlayerData.Levels.Impls
{
    [CreateAssetMenu(menuName = "PlayerData/LevelsData", fileName = "LevelsData")]
    public class LevelsData : ScriptableObject, ILevelsData
    {
        public Level[] Get() => _levels;
        [SerializeField] private Level[] _levels;
    }
}