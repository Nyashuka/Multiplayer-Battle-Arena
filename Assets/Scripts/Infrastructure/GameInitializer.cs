using System.Collections.Generic;
using Core.MatchmakingComponents;
using Environment;
using Fusion;
using UnityEngine;
using UserInterface;

namespace Infrastructure
{
    public class GameInitializer : NetworkBehaviour
    {
        [Header("Network Prefabs")]
        [SerializeField] private NetworkPrefabRef mapPrefab;
        [SerializeField] private NetworkPrefabRef playerPrefab;
        [SerializeField] private NetworkPrefabRef matchManagerPrefab;
        
        [Header("Local Prefabs")]
        [SerializeField] private Canvas canvasPrefab;
        [SerializeField] private HUDManager hudPrefab;
        
        private Canvas _canvas;
        private HUDManager _hud;
        
        private MatchManager _matchManager;
        private MapLoader _mapLoader;
        private Map _currentMap;
        private PlayerSpawner _playerSpawner;
        private readonly Dictionary<PlayerRef, NetworkObject> _players = new Dictionary<PlayerRef, NetworkObject>();

        public override void Spawned()
        {
            SetupUI();
        }

        public void LoadGame()
        {
            if (HasStateAuthority)
            {
                Debug.Log("Loading game");
            
                _mapLoader = new MapLoader(Runner, mapPrefab);
                _currentMap = _mapLoader.LoadMap();
                var spawnPoints = _currentMap.SpawnPoints;
            
                _playerSpawner = new PlayerSpawner(Runner, playerPrefab, spawnPoints);
                SpawnPlayers(_playerSpawner);
            
                InstantiateMatchManager();

                Debug.Log("Game loaded");
            }
            
        }


        private void SpawnPlayers(PlayerSpawner playerSpawner)
        {
            foreach (var playerRef in Runner.ActivePlayers)
            {
                _players.TryAdd(playerRef, playerSpawner.SpawnPlayer(playerRef));
            }
        }

        private void InstantiateMatchManager()
        {
            Runner.Spawn(matchManagerPrefab);
        }
        
        private void SetupUI()
        {
            _canvas = Instantiate(canvasPrefab);
            _hud = Instantiate(hudPrefab, _canvas.transform);
        }
    }
}