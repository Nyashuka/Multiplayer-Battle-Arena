using Infrastructure.Factories;
using Networking;
using ScriptableObjects;
using UnityEngine;
using UserInterface.MatchUI;

namespace Infrastructure
{
    public class MainMenuBootstrapper : MonoBehaviour
    {
        [SerializeField] private MainMenuConfig mainMenuConfig;
        
        public void Awake()
        {
            if (!UIManager.Instance)
            {
                var uiManagerFactory = new UIManagerFactory(mainMenuConfig.UIManagerPrefab);
                uiManagerFactory.Create();
            }
            
            UIManager.Instance.OpenInitialPage();

            if (!MainNetworkRunnerHandler.Instance)
            {
                var networkRunnerFactory = new NetworkRunnerHandlerFactory(mainMenuConfig.networkRunnerHandlerPrefab);
                networkRunnerFactory.Create();
            }
        }
    }
}