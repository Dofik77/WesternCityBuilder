using System;
using Runtime.Services.MonetizationService;
using TMPro;
using Zenject;

namespace Runtime.Game.Ui.Objects.Purchase
{
    public class UiInAppPurchaseEntity : UiPurchaseEntity
    {
        [Inject] private IMonetizationService _monetizationService;
        
        public string PurchaseKey;
        public TMP_Text CostTxt;

        public override void Init(Action action = null)
        {
            base.Init(action);
            CostTxt.text = _monetizationService.GetAndroidCost(PurchaseKey) + " $";
        }

        public override void OnPurchase()
        {
            _monetizationService.TryToInAppPurchase(PurchaseKey, OnAfterPurchase);
        }
    }
}