using UnityEngine;
using UserInterface;

namespace Infrastructure.Factories
{
    public class MainMenuFactory
    {
        private readonly Transform _canvasTransform;
        private readonly MainMenu _mainMenuPrefab;

        public MainMenuFactory(Transform canvasTransform, MainMenu mainMenuPrefab)
        {
            _canvasTransform = canvasTransform;
            _mainMenuPrefab = mainMenuPrefab;
        }

        public MainMenu Create()
        {
            return Object.Instantiate(_mainMenuPrefab, _canvasTransform);
        }
    }
}