using ECS.Core.Utils.ReactiveSystem;
using ECS.Core.Utils.ReactiveSystem.Components;
using ECS.Game.Components.General;
using ECS.Game.Systems.General;
using ECS.Game.Systems.General.NavMesh;
using ECS.Views.General;
using Leopotam.Ecs;
using PdUtils;
using Runtime.Game.Utils.MonoBehUtils;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace ECS.Game.Systems.WesternBuilder_System.StateMachine
{
    public class CampFireStateSystem : ReactiveSystem<EventStayNearCampFireComponent>
    {
        [Inject] private ScreenVariables _screenVariables;

        private readonly EcsFilter<EventStayNearCampFireComponent, LinkComponent> _unitFollowToCampFire;
        protected override EcsFilter<EventStayNearCampFireComponent> ReactiveFilter { get; }
        
        private const string CAMP_FIRE_START = "CampFire";
        private Vector3 _campfirePos;
        protected override void Execute(EcsEntity entity)
        {
            _campfirePos = _screenVariables.GetTransformPoint(CAMP_FIRE_START).transform.position;
            //entity.Get<EventSetDestinationComponent>().DestinationValue = _campfirePos;
        }
    }

    public struct EventStayNearCampFireComponent
    {
        
    }
}
