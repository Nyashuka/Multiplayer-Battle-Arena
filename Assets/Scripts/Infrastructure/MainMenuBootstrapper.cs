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
        
        public void Start()
        {
            var canvasFactory = new CanvasFactory(mainMenuConfig.canvasPrefab);
            var canvas = canvasFactory.Create();

            var mainMenuFactory = new MainMenuFactory(canvas.transform, mainMenuConfig.mainMenuPrefab);
            mainMenuFactory.Create();
            
            var networkRunnerFactory = new NetworkRunnerHandlerFactory(mainMenuConfig.networkRunnerHandlerPrefab);
            networkRunnerFactory.Create();
        }
    }
}