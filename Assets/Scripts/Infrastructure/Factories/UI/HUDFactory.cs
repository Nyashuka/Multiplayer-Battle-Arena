using UnityEngine;
using UserInterface.MatchUI;

namespace Infrastructure.Factories.UI
{
    public class HUDFactory
    {
        private readonly GameHUD _gameHUDPrefab;

        public HUDFactory(GameHUD gameHUDPrefab)
        {
            _gameHUDPrefab = gameHUDPrefab;
        }

        public GameHUD Create()
        {
            return Object.Instantiate(_gameHUDPrefab);
        }
    }
}