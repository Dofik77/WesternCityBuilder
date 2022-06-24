using CustomSelectables;
using Runtime.Game.Ui.Objects.General;
using Runtime.Game.Ui.Objects.Purchase;
using SimpleUi.Abstracts;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Game.Ui.Windows.Purchases
{
    public class PurchasesView : UiView
    {
        public Image Background;
        public CustomUiObject UiBox;
        public CustomButton BackBtn;

        public UiInAppPurchaseEntity Part1VipUnlock;
        public UiRewardedPurchaseEntity LootboxReward;
        public UiRewardedPurchaseEntity VipLevelReward;
        public UiCurrencyPurchaseEntity[] CurrencyPurchaseEntities;
    }
}