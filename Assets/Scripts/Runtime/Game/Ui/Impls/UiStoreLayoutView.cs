using CustomSelectables;
using Runtime.Game.Ui.Objects.Layouts;
using Runtime.Services.UiData.Data;
using UnityEngine;

namespace Runtime.Game.Ui.Impls
{
    public class UiStoreLayoutView : UiCurrenciesView
    {
        public CustomButton BackBtn;
        public UiLayoutContainer LayoutContainer;
        
        public void RebuildUiData(UiData.LayoutContainerData containerData)
        {
            Vector2 normalizedPosition;
            UiLayoutContainer.LayoutGroup group;
            for (int i = 0; i < LayoutContainer.LayoutGroups.Length; i++)
            {
                normalizedPosition = containerData.Positions[i];
                group = LayoutContainer.LayoutGroups[i];
                group.GeneratedLayout.ScrollRect.normalizedPosition = normalizedPosition;
            }
            LayoutContainer.LeafTo(containerData.CurrentPage);
        }
    }
}