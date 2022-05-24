using CustomSelectables;
using Runtime.Data;
using Runtime.Data.PlayerData.Currency;
using Runtime.Game.Ui.Objects.General;
using Runtime.Services.CommonPlayerData.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Game.Ui.Objects.Layouts
{
    public class UiGeneratedEntity : CustomUiObject
    {
        public Image Icon;
        public TMP_Text Text;
        public CustomButton Button;
        [SerializeField] private UiLayoutObjective _availableState;
        [SerializeField] private UiLayoutObjective _unavailableState;
        [SerializeField] private UiLayoutObjective _greyState;
        [SerializeField] private UiLayoutObjective _actionState;
        [SerializeField] private UiLayoutObjective _rejectState;

        public string Key => _key.ToLower();
        private string _key;
        [HideInInspector] public EUiEntityState CurrentUiState;
        [HideInInspector] public UiLayoutObjective CurrentStateObjective;

        public void InitEntity(IProvideUiGeneratedEntity dataEntity, string greyStateText, string actionStateText, string rejectState, Sprite currencyIcon)
        {
            transform.localScale = Vector3.one;
            _key = dataEntity.GetLowerKey();
            Text.text = dataEntity.GetName();
            Icon.sprite = dataEntity.GetIcon();
            _availableState.Value.text = dataEntity.GetCost().ToString();
            _availableState.Icon.sprite = currencyIcon;
            _unavailableState.Value.text = dataEntity.GetCost().ToString();
            _unavailableState.Icon.sprite = currencyIcon;
            _rejectState.Value.text = rejectState;
            _greyState.Value.text = greyStateText;
            _actionState.Value.text = actionStateText;
        }
        
        public void SetState(EUiEntityState state)
        {
            CurrentUiState = state;
            _availableState.gameObject.SetActive(false);
            _unavailableState.gameObject.SetActive(false);
            _greyState.gameObject.SetActive(false);
            _actionState.gameObject.SetActive(false);
            _rejectState.gameObject.SetActive(false);

            switch (state)
            {
                case EUiEntityState.Available:
                    CurrentStateObjective = _availableState;
                    break;
                case EUiEntityState.Unavailable:
                    CurrentStateObjective = _unavailableState;
                    break;
                case EUiEntityState.Grey:
                    CurrentStateObjective = _greyState;
                    break;
                case EUiEntityState.Action:
                    CurrentStateObjective = _actionState;
                    break;
                case EUiEntityState.Reject:
                    CurrentStateObjective = _rejectState;
                    break;
            }
            CurrentStateObjective.gameObject.SetActive(true);
        }
        
        
        public void SetPurchaseStates(IProvideUiGeneratedEntity keyData, ref Money.CurrencyValue[] currencyValues)
        {
            foreach (var currencyValue in currencyValues)
            {
                if (currencyValue.Currency == keyData.GetCurrency())
                {
                    if (currencyValue.Value >= keyData.GetCost())
                        SetState(EUiEntityState.Available);
                    else
                        SetState(EUiEntityState.Unavailable);
                    break;
                }
            }
        }
    }

    public enum EUiEntityState
    {
        Available,
        Unavailable,
        Grey,
        Action,
        Reject
    }

    public interface IProvideUiGeneratedEntity : IHasStringKey
    {
        string GetName();
        Sprite GetIcon();
        int GetCost();
        ECurrency GetCurrency();
    }
}