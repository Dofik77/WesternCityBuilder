using Runtime.Data.PlayerData.Recipe;
using Runtime.Data.PlayerData.Recipe.Impls;
using Runtime.Game.Ui.Objects.Layouts;
using TMPro;
using UnityEngine;

namespace Runtime.Game.Ui.Objects
{
    public class UiRecipeDataEntity : UiGeneratedEntity
    {
        [SerializeField] private TMP_Text _recipeName;
        [SerializeField] private TMP_Text _recipeDescription;
        [SerializeField] private TMP_Text _levelOpenData;

        [SerializeField] private TMP_Text[] _resourcesRequired;
        
        public override void InitEntity(IProvideUiGeneratedEntity dataEntity, string greyStateText, string actionStateText, string rejectState, Sprite currencyIcon)
        {
            base.InitEntity(dataEntity, greyStateText, actionStateText, rejectState, currencyIcon);

            var entity = (Recipe) dataEntity;

            _recipeName.text = entity.GetName();
            _recipeDescription.text = entity.GetDescription();
            _levelOpenData.text = entity.GetLevelOpenData();
            
            foreach (var resource in entity.GetResourceCount())
            {
                _resourcesRequired[0].text = resource.Key.ToString() + " " + resource.Value.ToString();
                //как обрезать строку?
                //динамичиская подстройка картинок? ( SO Currency ) 
            }
            
        }
    }
}