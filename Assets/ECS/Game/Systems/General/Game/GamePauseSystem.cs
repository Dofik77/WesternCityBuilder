using System.Diagnostics.CodeAnalysis;
using ECS.Core.Utils.ReactiveSystem;
using ECS.Game.Components.General;
using ECS.Game.Components.General.Events;
using Leopotam.Ecs;
using Runtime.Data.Base.Game;
using Runtime.Services.PauseService;

namespace ECS.Game.Systems.General.Game
{
    [SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global")]
    public class GamePauseSystem : ReactiveSystem<ChangeStageComponent>
    {
        protected override EcsFilter<ChangeStageComponent> ReactiveFilter { get; }
        private readonly EcsFilter<LinkComponent> _links;
        protected override bool DeleteEvent => false;
        protected override void Execute(EcsEntity entity)
        {
            var pause = ReactiveFilter.Get1(0).Value == EGameStage.Pause;
            foreach (var i in _links)
            {
                if (!(_links.Get1(i).View is IPause iPause)) continue;
                if(pause)
                    iPause.Pause();
                else
                    iPause.UnPause();
            }
        }
    }
}