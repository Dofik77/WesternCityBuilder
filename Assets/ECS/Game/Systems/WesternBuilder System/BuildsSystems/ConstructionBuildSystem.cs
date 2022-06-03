using ECS.Core.Utils.ReactiveSystem;
using Leopotam.Ecs;

namespace ECS.Game.Systems.WesternBuilder_System.BuildsSystems
{
    public class ConstructionBuildSystem : ReactiveSystem<ConstructionBuild>
    {
        protected override EcsFilter<ConstructionBuild> ReactiveFilter { get; }
        protected override void Execute(EcsEntity entity)
        {
            //постройка = кол-во ресурсов необходимых для постройки ( уже принесенных ).секунд
            //по завершению удаляем BuildUnderConstruction, даём BuildComp, и обновляем приоритет
            //( обычно идут чилить / заполнять склады ) 
        }
    }


    public struct ConstructionBuild
    {
        
    }
}