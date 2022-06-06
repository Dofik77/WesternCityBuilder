using ECS.Views.General;
using ECS.Views.WesterBuilderView.Interfaces;
using Runtime.Game.Ui.Objects;
using Runtime.Game.Ui.Objects.General;
using UnityEngine;


namespace ECS.Views
{
    public class BuildsView : LinkableView, IHasStopDistance
    {
        //указывать UI для здания и кол-во ресусров ( макс, если надо ) здесь 
        [SerializeField] public float StopDistance;
        [SerializeField] public RequiredObjectType ObjectType;
        [SerializeField] public ProgressBar ConstructionProgressBar;
        [SerializeField] public ProgressBar BuildProgressBar;
        [SerializeField] public GameObject ConstructedObject;
        [SerializeField] public GameObject BaseObject;
        
        private int _currentResource;

        private const float _barRepaintDuration = 0.3f;

        public void UpdateScore(int updateValue, int maxValue)
        {
            ConstructionProgressBar.Text.text = $"{updateValue} / {maxValue}";
            ConstructionProgressBar.Repaint(updateValue / maxValue, _barRepaintDuration);
        }
        
        public int CurrentResource
        {
            get => _currentResource;

            set
            {
                if(value < 0)
                    return;

                _currentResource = value;
            }
        }
        
        public float GetStopDistance()
        {
            return StopDistance;
        }
        
    }

    public enum RequiredObjectType
    {
        Default,
        
        WoodStorage,
        RockStorage,
        OreStorage,
        
        CampFire,
        
        Recipe
    }
}