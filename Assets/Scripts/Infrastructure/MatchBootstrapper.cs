using System.Collections.Generic;
using Core;
using Core.MatchmakingComponents;
using Data;
using Environment;
using Fusion;
using Infrastructure.Factories;
using Infrastructure.Factories.UI;
using ScriptableObjects;
using UnityEngine;
using UserInterface.MatchUI;

namespace Infrastructure
{
    public class MatchBootstrapper : NetworkBehaviour
    {
        [SerializeField] private MatchConfig matchConfig;
        
        private Map _map;
        private MatchManager MatchManager { get; set; }
        
        [Networked] private MatchTimer MatchTimer { get; set; }
        private Dictionary<PlayerRef, Player> Players { get; set; } = new();
        
        public override void Spawned()
        { 
            // state authority
            InitializeMap();
            InitializePlayers();
            InitializeMatchTimer();
            InitializeMatchManager(); 
            // all clients
            InitializeUI();
        }

        private void InitializeMatchTimer()
        {
            if(!HasStateAuthority) return;
            
            var timerFactory = new MatchTimerFactory(Runner, matchConfig.MatchTimerPrefab);
            MatchTimer = timerFactory.Create();
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

                var player = playerNetworkObject.GetComponent<Player>();
                Players.Add(activePlayer, player);
                
                SetupDefaultPlayerWeapon(activePlayer, player);
            }
        }

        private void SetupDefaultPlayerWeapon(PlayerRef playerRef, Player player)
        {
            if(!HasStateAuthority) return;
            
            var mainWeaponFactory = new MainWeaponFactory(Runner, matchConfig.DefaultWeaponPrefab);
            var playerWeapon = mainWeaponFactory.Create(playerRef, player.GetPrimaryWeaponTransform());
            player.SetWeapon(playerWeapon);
        }

        private void InitializeUI()
        {
            var canvasFactory = new CanvasFactory(matchConfig.canvasPrefab);
            var canvas = canvasFactory.Create();

            var matchUIControllerFactory = new MatchUIControllerFactory(matchConfig.MatchUIControllerPrefab);
            matchUIControllerFactory.Create(canvas.transform);
        }

        private void InitializeMatchManager()
        {
            if (!HasStateAuthority) return;
            
            var matchManagerFactory = new MatchManagerFactory(Runner, matchConfig.matchManagerPrefab);
            MatchManager = matchManagerFactory.Create();
            MatchManager.Initialize(Players, MatchTimer, _map);
        }

        private Vector3 GetSpawnPosition(List<SpawnPoint> spawnPoints)
        {
            return spawnPoints[Random.Range(0, spawnPoints.Count)].transform.position;
        }
    }
}