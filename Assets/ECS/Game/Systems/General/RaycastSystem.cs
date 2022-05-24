using ECS.Core.Utils.SystemInterfaces;
using ECS.Game.Components.Flags;
using ECS.Game.Components.General;
using ECS.Game.Systems.General.Player;
using ECS.Views.General;
using Leopotam.Ecs;
using Runtime.Game.Utils.MonoBehUtils;
using Runtime.Services.VibrationService;
using Runtime.Signals;
using UnityEngine;
using Zenject;

namespace ECS.Game.Systems.General
{
    public class RaycastSystem : IEcsUpdateSystem
    {
        [Inject] private SignalBus _signalBus;
        [Inject] private ScreenVariables _screenVariables;
        [Inject] private IVibrationService _vibrationService;

#pragma warning disable
        private readonly EcsWorld _world;
        private readonly EcsFilter<CameraComponent, LinkComponent> _cameraF;
        private readonly EcsFilter<EventInputDownComponent> _eventDown;
        private readonly EcsFilter<EventInputHoldAndDragComponent> _eventHoldAndDrag;
        private readonly EcsFilter<EventInputUpComponent> _eventUp;
        private readonly EcsFilter<PickupedComponent, LinkComponent> _pickuped;
#pragma warning restore 649

        private CameraView _cameraView;
        private UnityEngine.Camera _camera;

        private int _releasableMask = LayerMask.GetMask("Releasable");
        private int _draggableMask = LayerMask.GetMask("Draggable");
        private int _surfaceMask = LayerMask.GetMask("Surface");
        private int _defaultMask = LayerMask.GetMask("Default");
        private RaycastHit _hit;
        private Collider[] _colliders = new Collider[8];
        private Vector3 _collisionOffset = new Vector3(0.0f, 1.5f, 0.0f);
        private Vector2 _targetPositionRaycastOffset = new Vector2(0, 100f);
        

        private SignalJoystickUpdate _signalJoystickUpdate =
            new SignalJoystickUpdate(false, Vector2.zero, Vector2.zero);

        public void Run()
        {
            foreach (var i in _cameraF)
            {
                _cameraView = _cameraF.Get2(i).Get<CameraView>();
                _camera = _cameraView.GetCamera();
                _targetPositionRaycastOffset = Vector2.zero;
            }

            foreach (var i in _eventDown)
            {
                if (!TryCameraRaycast(_eventDown.Get1(i).Down, ref _draggableMask))
                    break;
            }

            foreach (var i in _eventHoldAndDrag)
            {
                if (!TryCameraRaycast(_eventHoldAndDrag.Get1(i).Drag, ref _surfaceMask))
                    break;
                
            }

            foreach (var i in _eventUp)
            {
                
            }
        }

        private bool TryCameraRaycast(Vector2 point, ref int layerMask) =>
            Physics.Raycast(_camera.ScreenPointToRay(point), out _hit, 100f, layerMask);

        private void ClearColliders()
        {
            for (int i = 0; i < _colliders.Length; i++)
                if (_colliders[0] != null)
                {
                    _colliders[0] = null;
                }
        }
    }

    public struct PickupedComponent : IEcsIgnoreInFilter
    {
    }
}