using System;
using Runtime.Data.PlayerData.Currency;
using Runtime.Services.AnalyticsService;
using Runtime.Services.AnalyticsService.Impls;
using TMPro;
using UnityEngine;
using Zenject;

namespace Runtime.Game.Ui.Objects.Purchase
{
    public class UiCurrencyPurchaseEntity : UiInAppPurchaseEntity
    {
        [Inject] private IAnalyticsService _analyticsService;

        public TMP_Text RewardTxt;

        [Header("Currency Reward")] public ECurrency Currency;
        public int Value;

        public override void Init(Action action = null)
        {
            base.Init(action);
            RewardTxt.text = "+" + Value;

            OnAfterPurchase = AddCurrency;
        }

        private void AddCurrency()
        {
            var data = _commonPlayerData.GetData();
            data.Money.Add(Currency, Value);
            _commonPlayerData.Save(data);
            _analyticsService.SendDesignEvent(AnalyticsState.Get,
                Enum.GetName(typeof(ECurrency), Currency), Value.ToString());
        }
    }
}