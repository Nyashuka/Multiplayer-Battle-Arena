using Fusion;
using Networking;
using UnityEngine;
using UserInterface;
using UserInterface.MatchUI;

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
        [SerializeField] private UIManager uiManagerPrefab;
        
        public UIManager UIManagerPrefab => uiManagerPrefab;
    }
}