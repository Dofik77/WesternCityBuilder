using System;
using CustomSelectables;
using Runtime.Data.PlayerData.Currency;
using Runtime.Data.PlayerData.Currency.Impls;
using Runtime.Data.PlayerData.LayoutSpriteData;
using Runtime.Data.PlayerData.Recipe;
using Runtime.Data.PlayerData.Recipe.Impls;
using Runtime.Game.Ui.Objects.Layouts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Runtime.Game.Ui.Objects
{
    public class UiRecipeDataEntity : UiGeneratedEntity
    {
        [Inject] private ILayoutSpriteData _LayoutSpriteDataData;

        [SerializeField] private TMP_Text _recipeName;
        [SerializeField] private TMP_Text _recipeDescription;
        [SerializeField] private TMP_Text _levelOpenData;
        [SerializeField] private LayoutGroup _layoutResourceData;

        public override void InitEntity(IProvideUiGeneratedEntity dataEntity, string greyStateText, string actionStateText, string rejectState, Sprite currencyIcon)
        {
            base.InitEntity(dataEntity, greyStateText, actionStateText, rejectState, currencyIcon);

            var entity = (Recipe) dataEntity;

            _recipeName.text = entity.GetName();
            _recipeDescription.text = entity.GetDescription();
            _levelOpenData.text = entity.GetLevelOpenData();
            

            foreach (var resource in entity.GetResourceCount())
            {
                //берем по имени ключа названия ресурса из массива необходимых ресусров 
                // берем из SO необходимый спрайт по имени
                var resourceName = (ELayoutSprite)Enum.Parse(typeof(ELayoutSprite), resource.Key.ToString()); 
                GameObject resourceSprite = _LayoutSpriteDataData.Get(resourceName).Icon; 
                
                //exeption - Setting the parent of a transform which resides in a Prefab Asset is disabled to prevent data corruption
                //that's way Inst
                Instantiate(resourceSprite, Vector3.zero, Quaternion.identity);
            }
        }
    }
}