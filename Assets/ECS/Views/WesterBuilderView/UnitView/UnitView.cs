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
        
        [Header("DotWeen Fields")] 
        [SerializeField] private float _stackOffsetX = 1;
        [SerializeField] private float _stackOffsetY = 1;
        [SerializeField] private int _stackHeight = 10;

        [SerializeField] private float _interactionDuration = 0.4f;
        
        
        private Stack<EcsEntity> _resources;
        
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
            var z = _stackColumn * _stackOffsetX;
            var y = _stackRow * _stackOffsetY;
            _stackRow++;
            if (_resources.Count >= _stackHeight * _stackColumn)
            {
                _stackColumn++;
                _stackRow = 1;
            }
            // Attach to stack
            resource.Get<MoveTweenEventComponent>().EventType = ETweenEventType.ResourcePickUp;
            resource.Get<Vector3Component<MoveTweenEventComponent>>().Value = new Vector3(0, y, z);
        }
        
        
    }
}