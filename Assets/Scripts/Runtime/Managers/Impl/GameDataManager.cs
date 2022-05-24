using System.Collections.Generic;
using ECS.DataSave;
using Runtime.Services.CommonPlayerData;
using Runtime.Services.CommonPlayerData.Data;
using Runtime.Services.GameStateService;
namespace Runtime.Managers.Impl
{
    public class GameDataManager : IGameDataManager
    {
        private readonly IGameStateService<GameState> _gameState;
        private readonly ICommonPlayerDataService<CommonPlayerData> _commonPlayerDataService;

        public GameDataManager(IGameStateService<GameState> gameState, ICommonPlayerDataService<CommonPlayerData> commonPlayerDataService)
        {
            _gameState = gameState;
            _commonPlayerDataService = commonPlayerDataService;
        }

        public bool NewGame => _gameState.GetData().SaveState.IsNullOrEmpty();

        public void RemoveAllData()
        {
            _gameState.Remove();
            _commonPlayerDataService.Remove();
        }

        public void RemoveCollectableData()
        {
            
        }

        public void RemoveNonCollectableData()
        {
            _commonPlayerDataService.Remove();
            _gameState.Remove();
        }

        public void RemoveLiveGameState()
        {
            _gameState.GetData().SaveState = new List<SaveState>();
            _gameState.Save();
        }
    }
}