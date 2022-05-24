using System.Collections.Generic;
using ECS.Core.Utils.ReactiveSystem;
using ECS.DataSave;
using ECS.Game.Components.General;
using ECS.Game.Components.General.Events;
using Leopotam.Ecs;
using Runtime.Services.GameStateService;
using Zenject;

namespace ECS.Game.Systems.General.Game
{
    public class SaveGameSystem : ReactiveSystem<SaveGameEventComponent>
    {
        [Inject] private readonly IGameStateService<GameState> _gameState;
        private readonly EcsWorld _world;
        private readonly EcsFilter<UIdComponent> _entities;
        protected override EcsFilter<SaveGameEventComponent> ReactiveFilter { get; }
        protected override void Execute(EcsEntity entity)
        {
            var gameStateData =  _gameState.GetData();
            gameStateData.SaveState = new List<SaveState>();
            
            foreach (var i in _entities)
            {
                var components = new SaveState();
                components.WriteState(_entities.GetEntity(i));
                gameStateData.SaveState.Add(components);
            }
            _gameState.Save();
        }
    }
}