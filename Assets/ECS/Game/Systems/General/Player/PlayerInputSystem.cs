using ECS.Core.Utils.SystemInterfaces;
using ECS.Game.Components.General;
using ECS.Game.Components.General.Input;
using Leopotam.Ecs;
using Runtime.Data.Base.Game;
using Runtime.Game.Ui.Windows.TouchPad;
using Runtime.Signals;
using UnityEngine;

namespace ECS.Game.Systems.General.Player
{
    public class PlayerInputSystem : IEcsUpdateSystem
    {
#pragma warning disable 
        private readonly EcsWorld _world;
        private readonly EcsFilter<GameStageComponent> _gameStage;
        private readonly EcsFilter<PointerDownComponent> _pointerDown;
        private readonly EcsFilter<PointerUpComponent> _pointerUp;
        private readonly EcsFilter<PointerDragComponent> _pointerDrag;
        private readonly EcsFilter<InputComponent> _input;
#pragma warning restore 649

        private bool _pressed;
        private bool _released = true;
        private Vector2 _pointerDownValueScreen;
        private Vector2 _pointerDragValueScreen;
        private Vector2 _pointerUpValueScreen;

        private SignalJoystickUpdate _signalJoystickUpdate =
            new SignalJoystickUpdate(false, Vector2.zero, Vector2.zero);

        public void Run()
        {
            if (_gameStage.Get1(0).Value != EGameStage.Play) return;
            
            foreach (var i in _pointerDown)
            {
                _pressed = true;
                _released = false;
                _pointerDownValueScreen = _pointerDown.Get1(i).Position;
                _pointerDragValueScreen = _pointerDownValueScreen;
                HandlePress();
            }

            foreach (var i in _pointerUp)
            {
                _pressed = false;
                _pointerUpValueScreen = _pointerUp.Get1(i).Position;
            }

            if (!_pressed && !_released)
            {
                HandleRelease();
                _released = true;
            }

            if (!_pressed)
            {
                return;
            }
            
            foreach (var i in _pointerDrag)
            {
                _pointerDragValueScreen = _pointerDrag.Get1(i).Position;
            }

            HandleHoldAndDrag();
        }

        private void HandleHoldAndDrag()
        {
            foreach (var i in _input)
            {
                _input.GetEntity(i).Get<EventInputHoldAndDragComponent>().Drag = _pointerDragValueScreen;
                _input.GetEntity(i).Get<EventInputHoldAndDragComponent>().Down = _pointerDownValueScreen;
            }
        }

        private void HandleRelease()
        {
            foreach (var i in _input)
                _input.GetEntity(i).Get<EventInputUpComponent>().Up = _pointerUpValueScreen;
        }

        private void HandlePress()
        {
            foreach (var i in _input)
                _input.GetEntity(i).Get<EventInputDownComponent>().Down = _pointerDownValueScreen;
        }
    }

    public struct EventInputHoldAndDragComponent
    {
        public Vector2 Down;
        public Vector2 Drag;
    }
    
    public struct EventInputDownComponent
    {
        public Vector2 Down;
    }
    
    public struct EventInputUpComponent
    {
        public Vector2 Up;
    }
}