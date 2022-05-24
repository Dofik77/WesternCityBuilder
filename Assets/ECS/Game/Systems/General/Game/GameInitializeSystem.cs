using ECS.DataSave;
using ECS.Game.Components.Flags;
using ECS.Game.Components.General;
using ECS.Utils.Extensions;
using Leopotam.Ecs;
using Runtime.Data.Base.Game;
using Runtime.Services.GameStateService;
using Zenject;

namespace ECS.Game.Systems.General.Game
{
    public class GameInitializeSystem : IEcsInitSystem
    {
        [Inject] private readonly IGameStateService<GameState> _generalState;
        
        private readonly EcsFilter<CameraComponent, LinkComponent> _cameraF;
        
        private readonly EcsWorld _world;
        public void Init()
        {
            if (LoadGame()) return;
            CreateEcsEntities();
        }
        private bool LoadGame()
        {
            _world.NewEntity().Get<GameStageComponent>().Value = EGameStage.Start;
            var gState = _generalState.GetData();
            if (gState.SaveState.IsNullOrEmpty()) return false;
            foreach (var state in gState.SaveState)
            {
                var entity =_world.NewEntity();
                state.ReadState(entity);
            }
            return true;
        }
        
        private void CreateEcsEntities()
        {
            _world.CreateEcsEntities();
        }
    }
}