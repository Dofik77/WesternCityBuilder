using System;
using System.Collections.Generic;
using CustomSelectables;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Runtime.Game.Ui.Windows.Main
{
    [RequireComponent(typeof(CustomSelectable))]
    public class Switcher : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textState;
        [SerializeField] private Button prevBtn;
        [SerializeField] private Button nextBtn;

        private CustomSelectable _selectable;
        private int _curIndex;
        private List<string> _options;
        private Action<int> _onChange;

        public CustomSelectable MainSelectable => _selectable;

        private void Awake()
        {
            _selectable = GetComponent<CustomSelectable>();
            _selectable.OnMoveAsObservable().Subscribe(Move).AddTo(_selectable);
            
            prevBtn.OnClickAsObservable().Subscribe(x => PrevState()).AddTo(prevBtn);
            nextBtn.OnClickAsObservable().Subscribe(x => NextState()).AddTo(prevBtn);
        }

        private void Move(AxisEventData data)
        {
            switch (data.moveDir)
            {
                case MoveDirection.Left:
                    PrevState();
                    break;
                case MoveDirection.Right:
                    NextState();
                    break;
            }
        }

        public void Setup(List<string> options, int defValue = 0, Action<int> onChange = null)
        {
            _curIndex = defValue;
            _options = options;
            _onChange = onChange;
            textState.text = options[defValue];
        }

        private void PrevState()
        {
            _curIndex--;

            if (_curIndex == -1) _curIndex = _options.Count - 1;
            
            textState.text = _options[_curIndex];
            _onChange?.Invoke(_curIndex);
        }
        
        private void NextState()
        {
            _curIndex++;

            if (_curIndex == _options.Count) _curIndex = 0;
            
            textState.text = _options[_curIndex];
            _onChange?.Invoke(_curIndex);
        }
    }
}