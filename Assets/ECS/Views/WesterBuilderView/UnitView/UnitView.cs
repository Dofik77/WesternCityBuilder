using System;
using System.Collections.Generic;
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
        [SerializeField] private GameObject _resourcePosition;
        [SerializeField] private Animator _animator;

        
        public Transform GetTransformPoint() => _resourcePosition.transform;


        public void SetAnimation(string stage, int value)
        {
            _animator.SetInteger(stage, value);
        }

        public ref NavMeshAgent GetAgent()
        {
            return ref _agent;
        }
        
        
        
    }
}