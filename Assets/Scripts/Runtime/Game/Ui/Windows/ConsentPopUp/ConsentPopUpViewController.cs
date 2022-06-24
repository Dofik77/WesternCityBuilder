using System;
using DG.Tweening;
using Runtime.Game.Ui.Extensions;
using Runtime.Signals;
using SimpleUi.Abstracts;
using SimpleUi.Interfaces;
using SimpleUi.Signals;
using UniRx;
using UnityEngine.EventSystems;
using Utils.UiExtensions;
using Zenject;

namespace Runtime.Game.Ui.Windows.ConsentPopUp
{
    public class ConsentPopUpViewController : UiController<ConsentPopUpTarget>, IInitializable, ICheckWindowBack
    {
        private readonly SignalBus _signalBus;
        private Action<bool> _onComplete;
        private bool _canBack;

        public ConsentPopUpViewController(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        public void Initialize()
        {
            _signalBus.GetStream<SignalQuestionChoice>().Subscribe(ONNext).AddTo(View);

            View.No.OnClickAsObservable().Subscribe(x => Confirm(false)).AddTo(View.No);
            View.Yes.OnClickAsObservable().Subscribe(x => Confirm(true)).AddTo(View.Yes);
        }

        private void ONNext(SignalQuestionChoice x)
        {
            _onComplete = x.Action;
            View.Title.text = x.Title;
        }

        public override void OnAfterShow()
        {
            EventSystem.current.SetSelectedGameObject(null);
            View.blockMouse.SetActive(true);

            DOTween.Sequence()
                .Append(View.canvasGroup.DOFade(1, 0.2f))
                .AppendInterval(.3f)
                .OnComplete(() =>
                {
                    View.blockMouse.SetActive(false);
                    View.No.Select();
                    View.No.OnSelect(null);
                });
        }

        private void Confirm(bool value)
        {
            EventSystem.current.SetSelectedGameObject(null);
            View.blockMouse.SetActive(true);
            
            DOTween.Sequence()
                .Append(View.canvasGroup.DOFade(0, 0.2f))
                .OnComplete(() =>
                {
                    _onComplete?.Invoke(value);
                    _canBack = true;
                    _signalBus.BackWindow();
                });
        }

        public bool HasBack() => _canBack;
    }
}