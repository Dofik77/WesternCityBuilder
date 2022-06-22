using System;
using System.Collections.Generic;
using ECS.Game.Components.WesternBuilder_Component;
using ECS.Game.Components.WesternBuilder_Component.dotween_components;
using ECS.Game.Systems.General;
using ECS.Game.Systems.General.NavMesh;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.AI;

namespace ECS.Views.General
{
    public class UnitView : LinkableView, IHasNavMeshAgent
    {
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private GameObject _resourceStack;
        [SerializeField] private Animator _animator;

        [Header("Tool Fields")] 
        [SerializeField] public GameObject Axe;
        [SerializeField] public GameObject Pick;
        
        [Header("DotWeen Fields")] 
        [SerializeField] private float _stackOffsetX = 0.1f;
        [SerializeField] private float _stackOffsetY = 0.1f;
        [SerializeField] private int _stackHeight = 10;

        private float _interactionDuration = 0.4f;
        
        Stack<EcsEntity> _resources = new Stack<EcsEntity>();
        
        private int _stackColumn;
        private int _stackRow;

        
        public Transform GetResourceStack() => _resourceStack.transform;

        public float GetInteractionDuration() => _interactionDuration;


        public void SetAnimation(string stage, int value)
        {
            _animator.SetInteger(stage, value);
        }

        public ref NavMeshAgent GetAgent()
        {
            return ref _agent;
        }

        public void AddResource(ref EcsEntity resource)
        {
            _resources.Push(resource);
            var x = _stackColumn * _stackOffsetX;
            var y = _stackRow * _stackOffsetY;
            _stackRow++;
            
            if (_resources.Count >= _stackHeight * _stackColumn)
            {
                _stackColumn++;
                _stackRow = 1;
            }
            
            resource.Get<MoveTweenEventComponent>().EventType = ETweenEventType.ResourcePickUp;
            resource.Get<Vector3Component<MoveTweenEventComponent>>().Value = new Vector3(x, 0, 0);
        }
    }
}