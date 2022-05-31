using ECS.Core.Utils.ReactiveSystem;
using ECS.Game.Components.General;
using ECS.Views.General;
using Leopotam.Ecs;

namespace ECS.Game.Systems.WesternBuilder_System
{
    public class SetAnimationSystem : ReactiveSystem<EventSetAnimationComponent>
    {
        protected override EcsFilter<EventSetAnimationComponent> ReactiveFilter { get; }
        private EcsFilter<EventSetAnimationComponent> _animationUnit;
        

        private UnitView _unitView;
        
        protected override void Execute(EcsEntity entity)
        {
            _unitView = entity.Get<LinkComponent>().View as UnitView;

            foreach (var i in _animationUnit)
            {
                var value = _animationUnit.Get1(i).Value;
                var name = _animationUnit.Get1(i).StageOfAnim;
                _unitView.SetAnimation(name,value);
            }
           
        }
    }

    public struct EventSetAnimationComponent
    {
        public string StageOfAnim;
        public int Value;
    }
}