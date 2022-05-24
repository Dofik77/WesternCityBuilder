using System;
using CustomSelectables;
using DG.Tweening;
using Runtime.Game.Ui.Extensions;
using Runtime.Game.Ui.Objects.General;
using Runtime.Services.CommonPlayerData.Data;
using UniRx;
using UnityEngine;
using Utils;

namespace Runtime.Game.Ui.Objects.Layouts
{
    public class UiLayoutContainer : CustomUiObject
    {
        [SerializeField] private CustomButton _leftButton;
        [SerializeField] private CustomButton _rightButton;
        [SerializeField] private float _transitionDuration = 0.35f;
        [SerializeField] private float _layoutDistance = 80f;
        public ref LayoutGroup[] LayoutGroups => ref _layoutGroups;
        [SerializeField] private LayoutGroup[] _layoutGroups;

        public int GroupIndex => _groupIndex;
        private int _groupIndex;
        private RectTransform _currentLabel;
        private RectTransform _currentLayout;
        private Vector2 _labelOffset;
        private Vector2 _layoutOffset;

        public void Init<T>(T[] data, CommonPlayerData playerData,
            Action<UiGeneratedEntity, UiGeneratedLayout> onButton, Predicate<string> rejectStagePredicate = null,
            Predicate<string> purchaseStagePredicate = null, Predicate<string> greyStatePredicate = null)
            where T : IProvideUiGeneratedEntity
        {
            LayoutGroup group;
            for (int i = 0; i < _layoutGroups.Length; i++)
            {
                group = _layoutGroups[i];
                group.GeneratedLayout.Init(data);
                group.GeneratedLayout.UpdateGeneratedLayoutData(data, playerData, onButton, rejectStagePredicate,
                    purchaseStagePredicate, greyStatePredicate);
                group.LabelLayout.gameObject.SetActive(false);
                group.GeneratedLayout.gameObject.SetActive(false);
            }

            group = _layoutGroups[0];
            _currentLabel = group.LabelLayout;
            _currentLabel.gameObject.SetActive(true);
            _currentLayout = group.GeneratedLayout.rectTransform;
            _currentLayout.gameObject.SetActive(true);
            _labelOffset = new Vector2(_currentLabel.rect.width + _layoutDistance, 0);
            _layoutOffset = new Vector2(_currentLayout.rect.width + _layoutDistance, 0);

            if (_layoutGroups.Length > 1)
            {
                _leftButton.OnClickAsObservable().Subscribe(x => OnLeaf(false)).AddTo(this);
                // _leftButton.InteractionCooldown = _transitionDuration;
                _rightButton.OnClickAsObservable().Subscribe(x => OnLeaf(true)).AddTo(this);
                // _rightButton.InteractionCooldown = _transitionDuration;
            }
            else
            {
                _leftButton.gameObject.SetActive(false);
                _rightButton.gameObject.SetActive(false);
            }
        }

        public void UpdatePurchaseStates<T>(T[] providedData, ref Money.CurrencyValue[] currencyValues)
            where T : IProvideUiGeneratedEntity
        {
            foreach (var group in _layoutGroups)
                group.GeneratedLayout.UpdatePurchaseStates(providedData, ref currencyValues);
        }

        public void LeafTo(int index)
        {
            var direction = _groupIndex - index;
            if (direction == 0)
                return;
            for (int i = 0; i != direction;)
                if (direction > 0)
                {
                    OnLeaf(false, false);
                    direction--;
                }
                else
                {
                    OnLeaf(true, false);
                    direction++;
                }
        }

        private void OnLeaf(bool value, bool useTweens = true)
        {
            var i = value ? -1 : 1;
            var currentLabel = _currentLabel;
            var currentLayout = _currentLayout;
            void DisableLabel() => currentLabel.gameObject.SetActive(false);
            void DisableLayout() => currentLayout.gameObject.SetActive(false);
            void EnableLeafButtons(bool value)
            {
                _rightButton.interactable = value;
                _leftButton.interactable = value;
            }

            if (useTweens)
            {
                EnableLeafButtons(false);
                currentLabel
                    .DoToPosition(currentLabel.anchoredPosition + i * _labelOffset, _transitionDuration, DisableLabel)
                    .SetEase(Ease.OutQuad);
                currentLayout
                    .DoToPosition(currentLayout.anchoredPosition + i * _layoutOffset, _transitionDuration,
                        DisableLayout).SetEase(Ease.OutQuad);
            }
            else
            {
                DisableLabel();
                DisableLayout();
            }

            _groupIndex += value ? 1 : -1;
            if (_groupIndex >= _layoutGroups.Length)
                _groupIndex = 0;
            if (_groupIndex <= -1)
                _groupIndex = _layoutGroups.Length - 1;

            var group = _layoutGroups[_groupIndex];
            _currentLabel = group.LabelLayout;
            _currentLayout = group.GeneratedLayout.rectTransform;
            _currentLabel.gameObject.SetActive(true);
            _currentLayout.gameObject.SetActive(true);

            if (useTweens)
            {
                _currentLabel.DoFromPosition(_currentLabel.anchoredPosition + -i * _labelOffset, _transitionDuration)
                    .SetEase(Ease.OutQuad);
                _currentLayout.DoFromPosition(_currentLayout.anchoredPosition + -i * _layoutOffset, _transitionDuration)
                    .SetEase(Ease.OutQuad).OnComplete(() => EnableLeafButtons(true));
            }
        }

        [Serializable]
        public class LayoutGroup
        {
            public RectTransform LabelLayout;
            public UiGeneratedLayout GeneratedLayout;
        }
    }
}