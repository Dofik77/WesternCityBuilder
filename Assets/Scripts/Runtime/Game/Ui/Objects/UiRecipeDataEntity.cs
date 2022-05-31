using Runtime.Data.PlayerData.Recipe;
using Runtime.Data.PlayerData.Recipe.Impls;
using Runtime.Game.Ui.Objects.Layouts;
using UnityEngine;

namespace Runtime.Game.Ui.Objects
{
    public class UiRecipeDataEntity : UiGeneratedEntity
    {
        public override void InitEntity(IProvideUiGeneratedEntity dataEntity, string greyStateText, string actionStateText, string rejectState, Sprite currencyIcon)
        {
            base.InitEntity(dataEntity, greyStateText, actionStateText, rejectState, currencyIcon);

            var entity = (Recipe) dataEntity;
            var resource = entity.GetResourceCount();
            
            entity.GetName();

        }
    }
}