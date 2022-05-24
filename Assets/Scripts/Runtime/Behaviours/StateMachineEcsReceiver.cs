using ECS.Core.Utils.ReactiveSystem.Components;
using Leopotam.Ecs;
using UnityEngine;

namespace Runtime.Behaviours
{
    public class StateMachineEcsReceiver<T> : StateMachineBehaviour, IEcsBehaviourReceiver where T : struct
    {
        private EcsEntity _entity;
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);
            _entity.Get<StateMachineCallbackStart<T>>();
        }
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);
            _entity.Get<StateMachineCallbackEnd<T>>();
        }
        public void SetEntity(EcsEntity entity) => _entity = entity;
    }
}