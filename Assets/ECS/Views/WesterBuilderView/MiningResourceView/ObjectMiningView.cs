using ECS.Game.Components.WesternBuilder_Component;
using ECS.Views.General;
using ECS.Views.WesterBuilderView.Interfaces;
using UnityEngine;

namespace ECS.Views
{
    public class ObjectMiningView : LinkableView, IHasStopDistance
    {
        [SerializeField] private float StopDistance;
        [SerializeField] private int CurrentResourceValue;
        [SerializeField] public RequiredResourceType ResourceType;
        
        [SerializeField] public int MiningAnimationStage;

        public float GetStopDistance() => StopDistance;
        public int GetCurrentResourceValue
        {
            get => CurrentResourceValue;

            set
            {
                if (value < 0)
                    return;

                CurrentResourceValue = value;
            }
            
        }
        
    }
}