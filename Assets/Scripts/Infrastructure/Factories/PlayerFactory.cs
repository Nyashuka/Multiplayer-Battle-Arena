using Fusion;
using UnityEngine;

namespace Infrastructure.Factories
{
    public class PlayerFactory
    {
        private readonly NetworkRunner _runner;
        private readonly NetworkPrefabRef _playerPrefab;
        
        public PlayerFactory(NetworkRunner runner, NetworkPrefabRef playerPrefab)
        {
            _runner = runner;
            _playerPrefab = playerPrefab;
        }

        public NetworkObject Create(PlayerRef playerRef, Vector3 position, Quaternion rotation)
        {
            return _runner.Spawn(_playerPrefab, position, rotation, playerRef);
        }
    }
}