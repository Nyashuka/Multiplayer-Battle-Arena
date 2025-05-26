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
            var uiManagerFactory = new UIManagerFactory(mainMenuConfig.UIManagerPrefab);
            uiManagerFactory.Create();
            
            var networkRunnerFactory = new NetworkRunnerHandlerFactory(mainMenuConfig.networkRunnerHandlerPrefab);
            networkRunnerFactory.Create();
        }
    }
}