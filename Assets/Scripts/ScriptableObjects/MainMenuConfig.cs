using Fusion;
using Networking;
using UnityEngine;
using UserInterface;

namespace ScriptableObjects
{
    [CreateAssetMenu(menuName = "Game/MainMenuConfig")]
    public class MainMenuConfig : ScriptableObject
    {
        [Header("Network Prefabs")]
        public NetworkPrefabRef matchBootstrapperPrefab;
        public NetworkRunner networkRunnerPrefab;
        public MainNetworkRunnerHandler networkRunnerHandlerPrefab;

        [Header("UI Prefabs")]
        public Canvas canvasPrefab;
        public MainMenu mainMenuPrefab;
    }
}