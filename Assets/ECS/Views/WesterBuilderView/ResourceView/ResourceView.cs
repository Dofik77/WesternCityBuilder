using ECS.Game.Components.WesternBuilder_Component;
using ECS.Views.General;
using ECS.Views.WesterBuilderView.Interfaces;
using UnityEngine;

namespace ECS.Views.WesterBuilderView
{
    public class ResourceView : LinkableView, IHasStopDistance
    {
        [SerializeField] public RequiredResourceType ResourceType;
        [SerializeField] public float StopDistance;
        [SerializeField] public int MiningAnimationStage;
        public float GetStopDistance() => StopDistance;
    }
}