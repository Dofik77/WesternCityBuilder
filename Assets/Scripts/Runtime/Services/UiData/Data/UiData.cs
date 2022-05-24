using System;
using Runtime.Game.Ui.Objects.Layouts;
using UnityEngine;

namespace Runtime.Services.UiData.Data
{
    [Serializable]
    public class UiData
    {
        public LayoutContainerData LevelsUiData;
        public LayoutContainerData StoreUiData;

        public UiData()
        {
            LevelsUiData = new LayoutContainerData(0);
            StoreUiData = new LayoutContainerData(0);
        }

        [Serializable]
        public struct LayoutContainerData
        {
            public Vector2[] Positions;
            public int CurrentPage;

            public LayoutContainerData(int capacity)
            {
                Positions = new Vector2[capacity];
                for (var i = 0; i < Positions.Length; i++)
                    Positions[i] = Vector2.one;
                CurrentPage = 0;
            }

            public void Set(UiLayoutContainer uiLayoutContainer)
            {
                for (int i = 0; i < Positions.Length; i++)
                {
                    Positions[i] = uiLayoutContainer.LayoutGroups[i].GeneratedLayout.ScrollRect.normalizedPosition;
                }
                CurrentPage = uiLayoutContainer.GroupIndex;
            }
        }
    }
}