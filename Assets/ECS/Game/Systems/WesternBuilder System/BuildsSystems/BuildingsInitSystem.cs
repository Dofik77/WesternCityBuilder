using DG.Tweening;
using ECS.Core.Utils.ReactiveSystem;
using ECS.Core.Utils.ReactiveSystem.Components;
using ECS.Game.Components.Flags;
using ECS.Game.Components.General;
using ECS.Game.Components.WesternBuilder_Component;
using ECS.Utils.Extensions;
using ECS.Views;
using ECS.Views.General;
using Leopotam.Ecs;
using Runtime.Game.Utils.MonoBehUtils;
using UnityEngine;
using Zenject;

namespace ECS.Game.Systems.WesternBuilder_System
{
    public class BuildingsInitSystem : ReactiveSystem<EventAddComponent<BuildComponent>>
    {
        [Inject] private ScreenVariables _screenVariables;
        protected override EcsFilter<EventAddComponent<BuildComponent>> ReactiveFilter { get; }

        private readonly EcsFilter<UnitComponent, LinkComponent>.Exclude<UnitHasPriority> _units;

        private string _buildPos;
        protected override void Execute(EcsEntity entity)
        {
            _buildPos = entity.Get<BuildComponent>().Value;
            var _view = entity.Get<LinkComponent>().View as BuildsView;
            
            _view.Transform.position = _screenVariables.GetTransformPoint(_buildPos).position;
            _view.Transform.rotation = _screenVariables.GetTransformPoint(_buildPos).rotation;
            
            foreach (var i in _units)
                _units.GetEntity(i).Get<EventUpdatePriorityComponent>();
        }
    }
}