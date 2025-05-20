using System.Collections.Generic;
using Core;
using Environment;
using Fusion;
using UnityEngine;

namespace Infrastructure
{
    public class PlayerSpawner
    {
        private NetworkPrefabRef _playerPrefab;
        private NetworkRunner _runner;
        private List<SpawnPoint> _spawnPoints;
        
        public PlayerSpawner(NetworkRunner runner, NetworkPrefabRef playerPrefab, List<SpawnPoint> spawnPoints)
        {
            _runner = runner;
            _playerPrefab = playerPrefab;
            _spawnPoints = spawnPoints;
        }

        public NetworkObject SpawnPlayer(PlayerRef playerRef)
        {
            var spawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Count)];
            
            var player = _runner.Spawn(_playerPrefab, spawnPoint.transform.position, Quaternion.identity, playerRef);

            return player;
        }
    }
}