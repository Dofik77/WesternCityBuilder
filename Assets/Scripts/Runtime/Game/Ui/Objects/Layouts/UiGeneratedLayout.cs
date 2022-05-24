using System;
using Runtime.Data;
using Runtime.Data.PlayerData.Currency;
using Runtime.Game.Ui.Extensions;
using Runtime.Game.Ui.Objects.General;
using Runtime.Services.CommonPlayerData.Data;
using Runtime.Services.UiSpawnService;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Runtime.Game.Ui.Objects.Layouts
{
    public class UiGeneratedLayout : CustomUiObject
    {
        [Inject] private IUiSpawnService _uiSpawnService;
        [Inject] private ICurrenciesData _currenciesData;

        [SerializeField] private string _uiPrefabName;
        public ScrollRect ScrollRect => _scrollRect;
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private LayoutGroup _layoutGroup;
        [SerializeField] private RectTransform _emptyLayoutBox;
        [SerializeField] private float _bottomPadding;
        [SerializeField] private string _greyStateText;
        [SerializeField] private string _actionStateText;
        [SerializeField] private string _rejectStateText;
        public string DataKeyMask => _dataKeyMask;
        [SerializeField] private string _dataKeyMask;
        private float _entityHeight;
        public UiGeneratedEntity[] Entities => _entities;
        private UiGeneratedEntity[] _entities;
        [HideInInspector] public UiGeneratedEntity CurrentGreyEntity;

        public void Init<T>(T[] data) where T : IProvideUiGeneratedEntity
        {
            GameObject entityObject;
            UiGeneratedEntity generatedEntity;
            IProvideUiGeneratedEntity dataEntity;

            var maskedData = data.Filter(x => x.GetLowerKey().StartsWith(_dataKeyMask));
            _entities = new UiGeneratedEntity[maskedData.Count];
            for (int i = 0; i < maskedData.Count; i++)
            {
                entityObject = _uiSpawnService.Spawn(_uiPrefabName);
                entityObject.transform.SetParent(_layoutGroup.transform);
                if (i == 0)
                {
                    var rectTrans = entityObject.GetComponent<RectTransform>();
                    _entityHeight = rectTrans.sizeDelta.y;
                }

                dataEntity = maskedData[i];
                generatedEntity = entityObject.GetComponent<UiGeneratedEntity>();
                generatedEntity.InitEntity(dataEntity, _greyStateText, _actionStateText, _rejectStateText,
                    _currenciesData.Get(dataEntity.GetCurrency()).Icon);
                _entities[i] = generatedEntity;
            }

            var layoutRect = _layoutGroup.GetComponent<RectTransform>();
            layoutRect.sizeDelta =
                new Vector2(layoutRect.sizeDelta.x, (_bottomPadding + _entityHeight) * maskedData.Count);

            if (_entities.Length < 1)
            {
                _layoutGroup.gameObject.SetActive(false);
                _emptyLayoutBox.gameObject.SetActive(true);
            }
        }

        public void DoWithEachEntity(Action<UiGeneratedEntity> OnAfterInit)
        {
            foreach (var uiGeneratedEntity in _entities)
                OnAfterInit.Invoke(uiGeneratedEntity);
        }


        public void UpdateGeneratedLayoutData<T>(T[] providedData,
            CommonPlayerData playerData, Action<UiGeneratedEntity, UiGeneratedLayout> onButton,
            Predicate<string> rejectStagePredicate = null,
            Predicate<string> purchaseStagePredicate = null,
            Predicate<string> greyStagePredicate = null) where T : IProvideUiGeneratedEntity
        {
            var entities = Entities;
            for (int i = 0; i < entities.Length; i++)
            {
                var entity = entities[i];
                if (rejectStagePredicate?.Invoke(entity.Key) ?? false)
                    entity.SetState(EUiEntityState.Reject);
                else if (purchaseStagePredicate?.Invoke(entity.Key) ?? false)
                    entity.SetPurchaseStates(providedData.Get(entity.Key), ref playerData.Money.CurrencyValues);
                else if (greyStagePredicate?.Invoke(entity.Key) ?? true)
                {
                    entity.SetState(EUiEntityState.Grey);
                    CurrentGreyEntity = entity;
                }
                else
                    entity.SetState(EUiEntityState.Action);
                
                entity.Button.OnClickAsObservable().Subscribe(x => OnButton()).AddTo(entity.Button);
                void OnButton() => onButton.Invoke(entity, this);
            }
        }

        public void UpdatePurchaseStates<T>(T[] providedData, ref Money.CurrencyValue[] currencyValues)
            where T : IProvideUiGeneratedEntity
        {
            foreach (var entity in Entities)
            {
                if (entity.CurrentUiState != EUiEntityState.Available &&
                    entity.CurrentUiState != EUiEntityState.Unavailable)
                    continue;
                entity.SetPurchaseStates(providedData.Get(entity.Key), ref currencyValues);
            }
        }
    }
}