using Fusion;
using Infrastructure;
using Networking;
using UnityEngine;

namespace ScriptableObjects
{
    
    [CreateAssetMenu(menuName = "Game/Match Start Config")]    
    public class MatchStartConfig : ScriptableObject
    {
        [SerializeField] private MatchBootstrapper matchBootstrapperPrefab;
        [SerializeField] private NetworkRunner networkRunnerPrefab;
        [SerializeField] private int playersInMatch;
        [SerializeField] private SceneRef gameScene;
        
        public NetworkRunner NetworkRunnerPrefab => networkRunnerPrefab;
        public int PlayersInMatch => playersInMatch;
        public SceneRef GameScene => gameScene;
        public MatchBootstrapper MatchBootstrapperPrefab => matchBootstrapperPrefab;
    }
}