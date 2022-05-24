using ECS.Core.Utils.SystemInterfaces;
using ECS.Game.Components.General;
using ECS.Game.Components.General.Events;
using Leopotam.Ecs;
using Runtime.Data.Base.Game;
using Runtime.Signals;
using UnityEngine;
using Zenject;

namespace ECS.Game.Systems.General.Level
{
    public class LevelTimerSystem : IEcsUpdateSystem, IEcsInitSystem
    {
        [Inject] private SignalBus _signalBus;
        
        private readonly EcsWorld _world;
        private readonly EcsFilter<GameStageComponent> _gameStage;
        private readonly EcsFilter<TimerComponent> _timerEntity;
        private readonly EcsFilter<LevelDataComponent> _levelData;
        private readonly EcsFilter<ChangeStageComponent> _changeStageEvent;

        private int _cacheSecond = int.MaxValue;
        private float _timer;

        public void Init()
        {
            foreach (var i in _timerEntity)
            {
                var value = _timerEntity.Get1(i).Value;
                _timer = _timerEntity.Get1(i).Value.ToFloat();
                _cacheSecond = value.ToInt();
            }
        }
        
        public void Run()
        {
            foreach (var g in _gameStage)
            {
                ref var stage = ref _gameStage.Get1(g).Value;
                if (stage != EGameStage.Play) return;
            }
            
            foreach (var t in _timerEntity)
            {
                ref var timerValue = ref _timerEntity.Get1(t).Value;
                _timer += Time.deltaTime / 2;
                timerValue.Increment(_timer);
                var toSeconds = timerValue.ToInt();
                if (_cacheSecond != toSeconds)  //Second tick
                {
                    _timerEntity.GetEntity(t).Get<TimerTickEventComponent>().Second = toSeconds;
                    _cacheSecond = toSeconds;
                    _signalBus.Fire(new SignalTimerUpdate(timerValue));
                }
                return;
            }
        }
        
        public struct TimerTickEventComponent
        {
            public int Second;
        }
    }
}