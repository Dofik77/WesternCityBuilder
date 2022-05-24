using System.Collections.Generic;
using ECS.DataSave;
using PdUtils.Dao;
using Zenject;

namespace Runtime.Services.GameStateService.Impl
{
    public class GameStateService : IGameStateService<GameState>, IInitializable
    {
        private readonly IDao<GameState> _dao;
        private GameState _cachedData;
        public GameStateService(IDao<GameState> dao)
        {
            _dao = dao;
        }
        
        public void Initialize() => _cachedData = _dao.Load() ?? NullState();

        public GameState GetData()
        {
            return _cachedData;
        }

        public void Remove()
        {
            _cachedData = NullState();
            _dao.Remove();
        }

        public void Save() => _dao.Save(_cachedData);

        private static GameState NullState()
        {
            return new GameState
            {
                SaveState = new List<SaveState>()
            };
        }
    }
}