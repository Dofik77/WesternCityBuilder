using System.Diagnostics.CodeAnalysis;
using ECS.Core.Utils.ReactiveSystem;
using ECS.Core.Utils.ReactiveSystem.Components;
using ECS.Game.Components.Flags;
using ECS.Game.Components.General;
using ECS.Views.General;
using Leopotam.Ecs;
using Runtime.Game.Utils.MonoBehUtils;
using Runtime.Services.PlayerSettings;
using Zenject;

namespace ECS.Game.Systems.General.Camera
{
    [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
    public class CameraInitSystem : ReactiveSystem<EventAddComponent<CameraComponent>>
    {
        [Inject] private readonly IPlayerSettingsService _playerSettings;
        [Inject] private ScreenVariables _screenVariables;
        private const string CAMERA_PRE_START = "CameraPreStart";
        
        private readonly EcsFilter<CameraComponent, LinkComponent> _cameraF;
        protected override EcsFilter<EventAddComponent<CameraComponent>> ReactiveFilter { get; }
        protected override bool DeleteEvent => true;

        protected override void Execute(EcsEntity entity)
        {
            foreach (var i in _cameraF)
            {
                var cameraView = _cameraF.Get2(i).Get<CameraView>();
                
                cameraView.Transform.position = _screenVariables.GetTransformPoint(CAMERA_PRE_START).position;
                cameraView.Transform.rotation = _screenVariables.GetTransformPoint(CAMERA_PRE_START).rotation;

                cameraView.HapticReceiver.hapticsEnabled = _playerSettings.PlayerSettings.UseVibration;

                // var cameraData = cameraView.GetCamera().GetUniversalAdditionalCameraData();
                // foreach (var uIcamera in GameObject.FindGameObjectsWithTag("UiCamera"))
                //     if (uIcamera.transform.parent == null)
                //     {
                //         cameraData.cameraStack.Add(uIcamera.GetComponent<UnityEngine.Camera>());
                //         break;
                //     }
                
            }
        }
    }
}