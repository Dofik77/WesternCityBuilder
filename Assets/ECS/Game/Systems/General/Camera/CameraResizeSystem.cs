using ECS.Core.Utils.SystemInterfaces;
using ECS.Game.Components.Flags;
using ECS.Game.Components.General;
using ECS.Views.General;
using Leopotam.Ecs;
using UnityEngine;

namespace ECS.Game.Systems.General.Camera
{
    public class CameraResizeSystem : IEcsUpdateSystem
    {
        private readonly EcsFilter<CameraComponent, LinkComponent> _cameraF;

        private CameraView _cameraView;

        public void Run()
        {
            foreach (var i in _cameraF)
            {
                _cameraView = _cameraF.Get2(i).Get<CameraView>();

                if (_cameraView.GetCamera().orthographic)
                {
                    _cameraView.GetCamera().orthographicSize = Mathf.Lerp(
                        _cameraView.InitialSize * (_cameraView.GetTargetAspect() / _cameraView.GetCamera().aspect),
                        _cameraView.InitialSize, _cameraView.WidthOrHeight);
                }
                else
                {
                    _cameraView.GetCamera().fieldOfView =
                        Mathf.Lerp(
                            _cameraView.CalcVerticalFov(_cameraView.GetHorizontalFov(), _cameraView.GetCamera().aspect),
                            _cameraView.GetCamera().fieldOfView, _cameraView.WidthOrHeight);
                }
            }
        }
    }
}