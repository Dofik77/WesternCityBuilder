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

        [SerializeField] public Canvas ProgressCanvasGroup;
        [SerializeField] public ProgressBar ResourceCountToConstructionProgressBar;
        [SerializeField] public ProgressBar CounctractProgressBar;

        [SerializeField] public GameObject ConstructedObject;
        [SerializeField] public GameObject BaseObject;
        
        private int _currentResource;
        private int _maxResourceForCounstract;
        

        private const float _barRepaintDuration = 0.3f;

        public void UpdateProgressBar(int updateValue)
        {
            _currentResource += updateValue;
            ResourceCountToConstructionProgressBar.Text.text = $"{_currentResource} / {_maxResourceForCounstract}";
            ResourceCountToConstructionProgressBar.Repaint(_currentResource / _maxResourceForCounstract, _barRepaintDuration);
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

        public int MaxResourceForCounstract
        {
            get => _maxResourceForCounstract;

            set
            {
                if(value < 0)
                    return;

                _maxResourceForCounstract = value;
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
        FoodStorage,
        OreStorage,
        
        CampFire,
        
        Recipe
    }
}