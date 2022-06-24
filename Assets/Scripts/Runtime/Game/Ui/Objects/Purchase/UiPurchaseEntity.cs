using System;
using CustomSelectables;
using Runtime.Game.Ui.Extensions;
using Runtime.Game.Ui.Objects.General;
using Runtime.Services.CommonPlayerData;
using Runtime.Services.CommonPlayerData.Data;
using Runtime.Services.MonetizationService;
using TMPro;
using UniRx;
using Zenject;

namespace Runtime.Game.Ui.Objects.Purchase
{
    public class UiPurchaseEntity : CustomUiObject
    {
        [Inject] protected IMonetizationService _monetizationService;
        [Inject] protected ICommonPlayerDataService<CommonPlayerData> _commonPlayerData;
        
        public CustomButton PurchaseBtn;

        public Action OnAfterPurchase;

        public virtual void Init(Action action = null)
        {
            OnAfterPurchase = action;
            PurchaseBtn.OnClickAsObservable().Subscribe(x => OnPurchase()).AddTo(this);
        }

        public virtual void OnPurchase()
        {
            
        }
    }
}