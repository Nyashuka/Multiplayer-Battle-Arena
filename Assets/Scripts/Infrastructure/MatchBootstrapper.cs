using System.Collections.Generic;
using Core;
using Core.MatchmakingComponents;
using Environment;
using Fusion;
using Infrastructure.Factories;
using ScriptableObjects;
using UnityEngine;

namespace Infrastructure
{
    public class MatchBootstrapper : NetworkBehaviour
    {
        [SerializeField] private MatchConfig matchConfig;
        
        private MatchManager _matchManager;
        private Map _map;
        private readonly Dictionary<PlayerRef, NetworkObject> _players = new();
        
        public override void Spawned()
        { 
            InitializeMap();
            InitializePlayers();
            InitializeUI();
            InitializeMatchManager(); 
        }
        
        private void InitializeMap()
        {
            if(!HasStateAuthority) return;
            
            var mapFactory = new MapFactory(Runner, matchConfig.mapPrefab);
            _map = mapFactory.Create();
        }

        private void InitializePlayers()
        {
            if(!HasStateAuthority) return;
            
            var playerFactory = new PlayerFactory(Runner, matchConfig.playerPrefab);
            foreach (var activePlayer in Runner.ActivePlayers)
            {
                var position = GetSpawnPosition(_map.SpawnPoints);
                var playerNetworkObject = playerFactory.Create(activePlayer, position, Quaternion.identity);
                _players.Add(activePlayer, playerNetworkObject);

                var player = playerNetworkObject.GetComponent<Player>();
                SetupDefaultPlayerWeapon(activePlayer, player);
            }
        }

        private void SetupDefaultPlayerWeapon(PlayerRef playerRef, Player player)
        {
            var mainWeaponFactory = new MainWeaponFactory(Runner, matchConfig.DefaultWeaponPrefab);
            var playerWeapon = mainWeaponFactory.Create(playerRef, player.GetPrimaryWeaponTransform());
            player.SetWeapon(playerWeapon);
        }

        private void InitializeUI()
        {
            var canvasFactory = new CanvasFactory(matchConfig.canvasPrefab);
            var canvas = canvasFactory.Create();

            var hudFactory = new HUDFactory(canvas.transform, matchConfig.hudPrefab);
            var hud = hudFactory.Create();
        }

        private void InitializeMatchManager()
        {
            if(!HasStateAuthority) return;
            
            var matchManagerFactory = new MatchManagerFactory(Runner, matchConfig.matchManagerPrefab);
            _matchManager = matchManagerFactory.Create();
        }

        private Vector3 GetSpawnPosition(List<SpawnPoint> spawnPoints)
        {
            return spawnPoints[Random.Range(0, spawnPoints.Count)].transform.position;
        }
    }
}