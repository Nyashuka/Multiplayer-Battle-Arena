using Core.MatchmakingComponents;
using Infrastructure.Factories;
using Infrastructure.Factories.UI;
using ScriptableObjects;
using UnityEngine;

namespace Infrastructure
{
    public class MainMenuBootstrapper : MonoBehaviour
    {
        [SerializeField] private MainMenuConfig mainMenuConfig;
        
        MatchSearcher _matchSearcher;
        
        public void Start()
        {
            var canvasFactory = new CanvasFactory(mainMenuConfig.canvasPrefab);
            var canvas = canvasFactory.Create();

            var mainMenuFactory = new MainMenuFactory(canvas.transform, mainMenuConfig.mainMenuPrefab);
            var mainMenu = mainMenuFactory.Create();
            
            var networkRunnerFactory = new NetworkRunnerFactory(mainMenuConfig.networkRunnerPrefab);
            var networkRunner = networkRunnerFactory.Create();
            
            _matchSearcher = new MatchSearcher(networkRunner);

            mainMenu.FindMatchRequested += FindMatch;
        }

        private async void FindMatch()
        {
            await _matchSearcher.FindMatchAsync();
        }
    }
}