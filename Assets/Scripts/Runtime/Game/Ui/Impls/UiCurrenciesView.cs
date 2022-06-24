using CustomSelectables;
using Runtime.Data.PlayerData.Currency;
using Runtime.Game.Ui.Objects.General;
using Runtime.Game.Ui.Objects.Layouts;
using Runtime.Services.CommonPlayerData.Data;
using SimpleUi.Abstracts;

namespace Runtime.Game.Ui.Impls
{
    public class UiCurrenciesView : UiView
    {
        public CurrencyBox SoftCurrency;
        public CurrencyBox HardCurrency;

        public void InitCurrencies(ref Money money, ICurrenciesData currenciesData)
        {
            SoftCurrency.Set(money.Get(ECurrency.Soft), currenciesData.Get(ECurrency.Soft).Icon);
            HardCurrency.Set(money.Get(ECurrency.Hard), currenciesData.Get(ECurrency.Hard).Icon);
        }

        public ref CurrencyBox GetCurrencyBox(ECurrency currency)
        {
            switch (currency)
            {
                case ECurrency.Hard:
                    return ref HardCurrency;
                default:
                    return ref SoftCurrency;
            }
        }
    }
}