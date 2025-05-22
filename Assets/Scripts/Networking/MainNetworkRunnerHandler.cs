using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Addons.Physics;
using Fusion.Sockets;
using Infrastructure;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Networking
{
    public class MainNetworkRunnerHandler : MonoBehaviour, INetworkRunnerCallbacks
    {
        [SerializeField] private NetworkRunner networkRunner;
        [SerializeField] private MatchBootstrapper matchBootstrapperPrefab;
        [SerializeField] private SceneRef gameScene;
        
        private readonly List<PlayerRef> _connectedPlayers = new();
        private const int MinPlayersToStartMatch = 2;
        private string _currentRoomName;
        
        [Networked] private MatchBootstrapper MatchBootstrapper { get; set; }

        private void PlayerJoined(PlayerRef player)
        {
            _connectedPlayers.Add(player);
            Debug.Log($"Player added: {player}");

            if (_connectedPlayers.Count == MinPlayersToStartMatch)
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
                if (networkRunner.IsServer)
                {
                    MatchBootstrapper = networkRunner.Spawn(matchBootstrapperPrefab).GetComponent<MatchBootstrapper>();
                }
            }
        }

        private void StartMatch()
        {
            var runnerSimulatePhysics3D = gameObject.AddComponent<RunnerSimulatePhysics3D>();
            runnerSimulatePhysics3D.ClientPhysicsSimulation = ClientPhysicsSimulation.SimulateAlways;
            
            if (networkRunner.IsSceneAuthority) 
            {
                networkRunner.LoadScene(gameScene);
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
        }

        public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
        {
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
    }
}