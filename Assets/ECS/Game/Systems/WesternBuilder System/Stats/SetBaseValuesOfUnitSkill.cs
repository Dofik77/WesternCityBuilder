using ECS.Core.Utils.ReactiveSystem;
using ECS.Core.Utils.ReactiveSystem.Components;
using ECS.Game.Components.Flags;
using ECS.Game.Components.WesternBuilder_Component;
using Leopotam.Ecs;

namespace ECS.Game.Systems.WesternBuilder_System.Stats
{
    public class SetBaseValuesOfUnitSkill : ReactiveSystem<EventAddComponent<UnitsSkillScoreComponent>>
    {
        protected override EcsFilter<EventAddComponent<UnitsSkillScoreComponent>> ReactiveFilter { get; }
        protected override void Execute(EcsEntity entity)
        {
            entity.Get<UnitsSkillScoreComponent>().SkillsOfMine = new UnitSkillOfMine[]
            {
                new UnitSkillOfMine(RequiredResourceType.WoodResource, 5),
                new UnitSkillOfMine(RequiredResourceType.RockResource, 3),
            };

            entity.Get<UnitsSkillScoreComponent>().SkillOfPortability = new UnitSkillOfPortability[]
            {
                new UnitSkillOfPortability(RequiredResourceType.WoodResource, 3),
                new UnitSkillOfPortability(RequiredResourceType.RockResource, 2),
            };
        }
    }
}