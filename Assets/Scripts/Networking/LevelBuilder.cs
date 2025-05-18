using System.Collections.Generic;
using Core;
using Core.PlayerComponents;
using Environment;
using Fusion;
using UnityEngine;

namespace Networking
{
    public class LevelBuilder : NetworkBehaviour, IPlayerJoined
    {
        [SerializeField] private EnemyCanvas enemyCanvasPrefab;
        [SerializeField] private NetworkPrefabRef playerPrefab;
        private Dictionary<PlayerRef, NetworkObject> SpawnedCharacters { get; set; } = new Dictionary<PlayerRef, NetworkObject>();
        [SerializeField] private List<SpawnPoint> spawnPoints = new List<SpawnPoint>();
        
        public void SpawnPlayer(PlayerRef playerRef)
        {
            Vector3 spawnPosition =
                new Vector3((playerRef.RawEncoded % Runner.Config.Simulation.PlayerCount) * 3, 1, 0);
            
            NetworkObject networkPlayerObject =
                Runner.Spawn(playerPrefab, spawnPosition, Quaternion.identity, playerRef);
            
            SpawnedCharacters.Add(playerRef, networkPlayerObject);
        }

        public void PlayerJoined(PlayerRef player)
        {
            if (Runner.IsServer)
            {
                SpawnPlayer(player);
            }
        }
    }
}