using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using Fusion;
using Fusion.Addons.Physics;
using Fusion.Sockets;
using Infrastructure;
using ScriptableObjects;
using Services.EventBus;
using Services.EventBus.EventBusArguments;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Networking
{
    public class MainNetworkRunnerHandler : MonoBehaviour, INetworkRunnerCallbacks
    {
        public static MainNetworkRunnerHandler Instance { get; private set; }
        [SerializeField] private MatchStartConfig startConfig;
        
        private int _playersToStart;
        private string _currentRoomName;
        
        public int LobbySize => _playersToStart;

        public void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }        
        
        
        [Networked] private MatchBootstrapper MatchBootstrapper { get; set; }
        
        private FindMatchStarter _findMatchStarter;
        private MatchStateEnum _matchState;
        private NetworkRunner _networkRunner;
        
        public MatchStateEnum MatchState => _matchState;
        
        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            Reset();
            GameEventBus.Instance.Subscribe(GameEventDefinitions.StartMatchSearchRequested, OnStartSearchMatchRequested);
            GameEventBus.Instance.Subscribe(GameEventDefinitions.StopMatchSearchRequested, OnStopSearchMatchRequested);
        }

        private void Reset()
        {
            _matchState = MatchStateEnum.Lobby;
            _findMatchStarter = new FindMatchStarter();
        }

        private NetworkRunner InstantiateNetworkRunner()
        {
            var networkRunner = Instantiate(startConfig.NetworkRunnerPrefab);
            
            return networkRunner;
        }

        private void OnStartSearchMatchRequested(IEventBusArgs args)
        {
            if(_matchState == MatchStateEnum.Matching || _matchState == MatchStateEnum.Searching)
                return;
            
            if (args is StartMatchSearchEventArgs startMatchSearchArgs)
            {
                _playersToStart = startMatchSearchArgs.PlayersCount;
            }
            else
            {
                _playersToStart = startConfig.PlayersInMatch;
            }
            
            _matchState = MatchStateEnum.Searching;
            _networkRunner = InstantiateNetworkRunner();
            _networkRunner.AddCallbacks(this);
            _findMatchStarter.FindMatchAsync(_networkRunner, _playersToStart);
        }

        private void OnStopSearchMatchRequested(IEventBusArgs args)
        {
            if(_matchState == MatchStateEnum.Matching || _matchState == MatchStateEnum.Lobby)
                return;
            
            _networkRunner.Shutdown();
            _matchState = MatchStateEnum.Lobby;
        }

        private void PlayerJoined(PlayerRef player)
        {
            Debug.Log($"Player added: {player}");

            if (_networkRunner.ActivePlayers.Count() == _playersToStart)
            {
                Debug.Log($"Starting match");
                StartMatch();     
            }
        }

        private void SceneLoadDone()
        {
            var sceneName = SceneManager.GetActiveScene().name;
            Debug.Log("Scene loaded: " + sceneName);
            if (sceneName == "MatchScene")
            {
                if (_networkRunner.IsServer)
                {
                    MatchBootstrapper = _networkRunner.Spawn(startConfig.MatchBootstrapperPrefab).GetComponent<MatchBootstrapper>();
                }
            }
        }

        private void StartMatch()
        {
            _matchState = MatchStateEnum.Matching;

            if (_networkRunner.IsServer)
            {
                var runnerSimulatePhysics3D = _networkRunner.gameObject.AddComponent<RunnerSimulatePhysics3D>();
                runnerSimulatePhysics3D.ClientPhysicsSimulation = ClientPhysicsSimulation.SimulateAlways;
            }
            
            if (_networkRunner.IsSceneAuthority) 
            {
                _networkRunner.LoadScene(startConfig.GameScene);
            }
        } 
        
        public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
        {
        }

        public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
        {
        }

        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            PlayerJoined(player);
        }

        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
        }

        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
        {
            SceneManager.LoadScene("BootScene");
            Reset();
        }

        public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
        {
            _networkRunner.Shutdown();
        }

        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
        {
        }

        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
        {
        }

        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
        {
        }

        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
        {
        }

        public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
        {
        }

        public void OnInput(NetworkRunner runner, NetworkInput input)
        {
        }

        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
        {
        }

        public void OnConnectedToServer(NetworkRunner runner)
        {
        }

        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
        {
        }

        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
        {
        }

        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
        {
        }

        public void OnSceneLoadDone(NetworkRunner runner)
        {
            SceneLoadDone();
        }

        public void OnSceneLoadStart(NetworkRunner runner)
        {
        }

        public void LeftMatch()
        {
            _networkRunner.Shutdown();
        }
    }
}