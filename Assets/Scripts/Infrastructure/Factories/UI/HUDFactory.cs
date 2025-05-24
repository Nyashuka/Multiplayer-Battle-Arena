using UnityEngine;
using UserInterface.MatchUI;

namespace Infrastructure.Factories.UI
{
    public class HUDFactory
    {
        private readonly Transform _canvasTransform;
        private readonly GameHUD _gameHUDPrefab;

        public HUDFactory(Transform canvasTransform, GameHUD gameHUDPrefab)
        {
            _canvasTransform = canvasTransform;
            _gameHUDPrefab = gameHUDPrefab;
        }

        public GameHUD Create()
        {
            return Object.Instantiate(_gameHUDPrefab, _canvasTransform);
        }
    }
}