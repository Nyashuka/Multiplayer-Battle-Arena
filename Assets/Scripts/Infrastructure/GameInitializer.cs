using System.Collections.Generic;
using Environment;
using Fusion;
using UnityEngine;

namespace Infrastructure
{
    
    public class GameInitializer : NetworkBehaviour
    {
        [SerializeField] private NetworkPrefabRef mapPrefab;
        [SerializeField] private NetworkPrefabRef playerPrefab;
        
        private MapLoader _mapLoader;
        private Map _currentMap;
        private PlayerSpawner _playerSpawner;
        private readonly Dictionary<PlayerRef, NetworkObject> _players = new Dictionary<PlayerRef, NetworkObject>();

        public void LoadGame()
        {
            if (!HasStateAuthority)
                return;
            
            Debug.Log("Loading game");
            
            _mapLoader = new MapLoader(Runner, mapPrefab);
            _currentMap = _mapLoader.LoadMap();
            var spawnPoints = _currentMap.SpawnPoints;
            
            _playerSpawner = new PlayerSpawner(Runner, playerPrefab, spawnPoints);
            SpawnPlayers(_playerSpawner);

            Debug.Log("Game loaded");
        }

        private void SpawnPlayers(PlayerSpawner playerSpawner)
        {
            foreach (var playerRef in Runner.ActivePlayers)
            {
                _players.TryAdd(playerRef, playerSpawner.SpawnPlayer(playerRef));
            }
        }
    }
}