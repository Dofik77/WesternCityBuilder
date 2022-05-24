using System;
using Runtime.Services.CommonPlayerData;
using Runtime.Services.CommonPlayerData.Data;
using Runtime.Services.DelayService;
using Runtime.Services.PauseService;
using Runtime.Services.SceneLoading;
using Runtime.Services.UiData;
using Runtime.Services.UiData.Data;
using Runtime.Services.UiSpawnService;
using SimpleUi.Abstracts;
using SimpleUi.Interfaces;
using Zenject;

namespace Runtime.Game.Ui.Impls
{
    public class UiControllerExtended<T> : UiController<T> where T : IUiView
    {
        [Inject] protected readonly ICommonPlayerDataService<CommonPlayerData> _commonPlayerData;
        [Inject] protected readonly IUiDataService<UiData> _uiData;
        [Inject] protected readonly ISceneLoadingManager _sceneLoadingManager;
        [Inject] protected readonly SignalBus _signalBus;
        [Inject] protected readonly IPauseService _pauseService;
        [Inject] protected readonly IUiSpawnService _uiSpawnService;
        [Inject] protected IDelayService _delayService;
        
        public BeforeActionEvent BeforeHideEvent;
        public BeforeActionEvent BeforeShowEvent;

        public override void Hide()
        {
            if (BeforeHideEvent == null)
                base.Hide();
            else
            {
                BeforeHideEvent.Action.Invoke();
                _delayService.Do(BeforeHideEvent.Delay, base.Hide);
            }
        }

        public override void Show()
        {
            if (BeforeShowEvent == null)
                base.Show();
            else
            {
                BeforeShowEvent.Action.Invoke();
                _delayService.Do(BeforeShowEvent.Delay, base.Show);
            }
        }
        
        public virtual void InitUiData()
        {}

        public virtual void EnableInput(bool value)
        {}
    }

    public class BeforeActionEvent
    {
        public Action Action;
        public float Delay;

        public BeforeActionEvent(Action action, float delay)
        {
            Action = action;
            Delay = delay;
        }
    }
}