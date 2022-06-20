using System.Diagnostics.CodeAnalysis;
using ECS.Core.Utils.ReactiveSystem;
using ECS.Core.Utils.ReactiveSystem.Components;
using ECS.Game.Components.Flags;
using ECS.Game.Components.General;
using ECS.Views.General;
using Leopotam.Ecs;
using Runtime.Game.Utils.MonoBehUtils;
using UnityEngine;
using Zenject;

namespace ECS.Game.Systems.WesternBuilder_System
{
    [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
    public class UnitInitSystem : ReactiveSystem<EventAddComponent<UnitComponent>>
    {
        [Inject] private ScreenVariables _screenVariables;
        private const string PLAYER_START = "PlayerStart";
        protected override EcsFilter<EventAddComponent<UnitComponent>> ReactiveFilter { get; }

        private EcsEntity _unitEntity;
        private UnitView _unitView;
        
        protected override bool DeleteEvent => true;
        protected override void Execute(EcsEntity entity)
        {
            entity.Get<UnitComponent>().AxeAvailable = false;
            entity.Get<UnitComponent>().PickAvailable = false;
            
            _unitView = entity.Get<LinkComponent>().View as UnitView;
            
            _unitView.Axe.SetActive(false);
            _unitView.Pick.SetActive(false);
            
            var randomSkin = Random.Range(0, 7);
            _unitView.gameObject.transform.GetChild(randomSkin).gameObject.SetActive(true);
            

            var offsetPos = Random.Range(5, 10);
            _unitView.transform.position = _screenVariables.GetTransformPoint(PLAYER_START).position + new Vector3(offsetPos,0,0);
            _unitView.transform.rotation = _screenVariables.GetTransformPoint(PLAYER_START).rotation;
            
            _unitView.GetAgent().enabled = true;
        }
    }
}