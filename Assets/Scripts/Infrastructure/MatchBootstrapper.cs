using System.Collections.Generic;
using Core.MatchmakingComponents;
using Core.PlayerComponents;
using Environment;
using Fusion;
using Infrastructure.Factories;
using Infrastructure.Factories.UI;
using ScriptableObjects;
using Services;
using Services.ServiceLocatorModule;
using Services.VFXs;
using UnityEngine;
using UserInterface.MatchUI;

namespace Infrastructure
{
    public class MatchBootstrapper : NetworkBehaviour
    {
        [SerializeField] private MatchBootstrapperConfig matchBootstrapperConfig;

        [Networked] private MatchManager MatchManager { get; set; }
        [Networked] private MatchTimer MatchTimer { get; set; }
        [Networked] private WeaponDealer WeaponDealer { get; set; }
        
        private Map _map;
        private Dictionary<PlayerRef, Player> Players { get; } = new();
        
        public override void Spawned()
        { 
            // all
            RegisterServices();
            
            // state authority
            InitializeMap();
            InitializePlayers();
            InitializeMatchTimer();
            InitializeMatchManager(); 
            
            // all clients
            InitializeUI();
        }

        private void RegisterServices()
        {
            ServiceLocator.Instance.Register(new WeaponDatabaseService(matchBootstrapperConfig.WeaponList));
            ServiceLocator.Instance.Register(new UtilityItemsDatabaseService(matchBootstrapperConfig.UtilityItemsList));
            ServiceLocator.Instance.Register(new VFXService());
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            UnregisterServices();
        }

        private void UnregisterServices()
        {
            ServiceLocator.Instance.UnRegister<WeaponDatabaseService>();
            ServiceLocator.Instance.UnRegister<UtilityItemsDatabaseService>();
        }

        private void InitializeMatchTimer()
        {
            if(!HasStateAuthority) return;
            
            var timerFactory = new MatchTimerFactory(Runner, matchBootstrapperConfig.MatchTimerPrefab);
            MatchTimer = timerFactory.Create();
        }

        private void InitializeMap()
        {
            if(!HasStateAuthority) return;
            
            var mapFactory = new MapFactory(Runner, matchBootstrapperConfig.mapPrefab);
            _map = mapFactory.Create();
        }

        private void InitializePlayers()
        {
            if(!HasStateAuthority) return;
            
            var playerFactory = new PlayerFactory(Runner, matchBootstrapperConfig.playerPrefab);
            foreach (var activePlayer in Runner.ActivePlayers)
            {
                var position = GetSpawnPosition(_map.SpawnPoints);
                var playerNetworkObject = playerFactory.Create(activePlayer, position, Quaternion.identity);

                var player = playerNetworkObject.GetComponent<Player>();
                Players.Add(activePlayer, player);
                
                SetupDefaultPlayerWeapon(activePlayer, player);
                SetupDefaultUtilityItem(player);
            }
        }

        private void SetupDefaultUtilityItem(Player player)
        {
            if(!HasStateAuthority) return;
            
            player.SetUtilityItem(matchBootstrapperConfig.DefaultUtilityItemConfig.Id);
        }

        private void SetupDefaultPlayerWeapon(PlayerRef playerRef, Player player)
        {
            if(!HasStateAuthority) return;
            
            var mainWeaponFactory = new MainWeaponFactory(Runner, matchBootstrapperConfig.DefaultWeaponConfig);
            var playerWeapon = mainWeaponFactory.Create(playerRef, player.MainWeaponTransform);
            player.SetWeapon(playerWeapon);
        }

        private void InitializeUI()
        {
            var hudFactory = new HUDFactory(matchBootstrapperConfig.GameHUDPrefab);
            var hud = hudFactory.Create();
            
            UIManager.Instance.SetHud(hud);
        }

        private void InitializeMatchManager()
        {
            if (HasStateAuthority)
            {
                var matchManagerFactory = new MatchManagerFactory(Runner, matchBootstrapperConfig.matchManagerPrefab);
                MatchManager = matchManagerFactory.Create();

                var weaponDealerFactory = new WeaponDealerFactory(Runner, matchBootstrapperConfig.WeaponDealerPrefab);
                WeaponDealer = weaponDealerFactory.Create();     
                
            }            
            
            MatchManager.Initialize(Players, MatchTimer, _map, WeaponDealer);
            ServiceLocator.Instance.Register<IPlayersListContext>(MatchManager);
        }

        private Vector3 GetSpawnPosition(List<SpawnPoint> spawnPoints)
        {
            return spawnPoints[Random.Range(0, spawnPoints.Count)].transform.position;
        }
    }
}