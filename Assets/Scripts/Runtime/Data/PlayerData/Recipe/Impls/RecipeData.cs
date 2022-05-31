using ECS.Game.Components.WesternBuilder_Component;
using UnityEngine;

namespace Runtime.Data.PlayerData.Recipe.Impls
{
    [CreateAssetMenu(menuName = "PlayerData/RecipeData", fileName = "RecipeData")]
    public class RecipeData : ScriptableObject, IRecipeData
    {
        public Recipe[] Get() => _recipes;
        [SerializeField] private Recipe[] _recipes;
    }
}