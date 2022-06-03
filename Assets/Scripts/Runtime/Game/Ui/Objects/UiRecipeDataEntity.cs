using System;
using CustomSelectables;
using Runtime.Data.Base.Ui;
using Runtime.Data.PlayerData.Currency;
using Runtime.Data.PlayerData.Currency.Impls;
using Runtime.Data.PlayerData.LayoutSpriteData;
using Runtime.Data.PlayerData.Recipe;
using Runtime.Data.PlayerData.Recipe.Impls;
using Runtime.Game.Ui.Objects.Layouts;
using Runtime.Game.Ui.Objects.UiObjectives;
using Runtime.Services.UiSpawnService;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Runtime.Game.Ui.Objects
{
    public class UiRecipeDataEntity : UiGeneratedEntity
    {
        [Inject] private ILayoutSpriteData _LayoutSpriteData;
        [Inject] private IUiSpawnService _spawnService;

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
                var layout =_spawnService.Spawn("ResourceData", _layoutResourceData.transform);
                var component = layout.GetComponent<UiObjective>();
                var resourceName = (ELayoutSprite)Enum.Parse(typeof(ELayoutSprite), resource.Key.ToString()); 
                Sprite resourceSprite = _LayoutSpriteData.Get(resourceName).Icon; 
                component.Icon.sprite = resourceSprite;
                component.Text.text = resource.NeedToConstruct.ToString();
            }
        }
    }
}