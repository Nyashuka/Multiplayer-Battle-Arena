using System.Collections.Generic;
using System.Linq;
using Core.MatchmakingComponents.MatchStates;
using Core.MatchmakingComponents.ScoreSystem;
using Core.PlayerComponents;
using Data;
using Environment;
using Fusion;
using Infrastructure.MatchStates;
using ScriptableObjects;
using Services.EventBus;
using Services.EventBus.EventBusArguments;
using UnityEngine;
using MatchStateEnum = Core.MatchmakingComponents.MatchStates.MatchStateEnum;

namespace Core.MatchmakingComponents
{
    public class MatchManager : NetworkBehaviour, IMatchContext, IPlayersListContext
    {
        // config
        [SerializeField] private MatchConfig config;
        public MatchConfig MatchConfig => config;
        
        // match data
        public Dictionary<PlayerRef, Player> Players { get; private set; }
        public List<PlayerRef> AlivePlayers { get; private set; }
        
        // match components
        public MatchTimer MatchTimer { get; private set; }
        public Map Map { get; private set; }
        public MatchScore MatchScore { get; private set; }
        public MatchStatistic MatchStatistic { get; private set; }
        private IMatchState CurrentState { get; set; }

        // match utilities
        private WeaponDealer _weaponDealer;
        private PlayersRespawner _playersRespawner;

        public void Initialize(Dictionary<PlayerRef, Player> players, MatchTimer matchTimer, Map map,
            WeaponDealer weaponDealer)
        {
            Players = players;
            AlivePlayers = players.Keys.ToList();
            MatchTimer = matchTimer;
            Map = map;

            _weaponDealer = weaponDealer;
            _weaponDealer.Initialize(players);

            if (HasStateAuthority)
            {
                SetState(new MatchWarmupState(this));
            }
        }

        public override void Spawned()
        {
            MatchScore = new MatchScore();
            MatchStatistic = new MatchStatistic();
            _playersRespawner = new PlayersRespawner(this);
            
            if (HasStateAuthority)
            {
                GameEventBus.Instance.Subscribe(GameEventDefinitions.PlayerDeath, OnPlayerDeath);
                GameEventBus.Instance.Subscribe(GameEventDefinitions.PlayerLeft, OnPlayerLeft);
            }
        }
        
        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            GameEventBus.Instance.Unsubscribe(GameEventDefinitions.PlayerDeath, OnPlayerDeath);
            GameEventBus.Instance.Unsubscribe(GameEventDefinitions.PlayerLeft, OnPlayerLeft);
        }
        
        public override void FixedUpdateNetwork()
        {
            if (!HasStateAuthority) return;

            CurrentState?.Update();
            _playersRespawner?.Update(Runner);
        }

        private void OnPlayerLeft(IEventBusArgs e)
        {
            if(!HasStateAuthority) return;

            if (e is PlayerLeftMatchEventArgs playerLeftMatchEventArgs)
            {
                if(Players.TryGetValue(playerLeftMatchEventArgs.PlayerRef, out var player))
                {
                    Runner.Despawn(player.Object);
                    Players.Remove(playerLeftMatchEventArgs.PlayerRef);
                }
                
                AlivePlayers.Remove(playerLeftMatchEventArgs.PlayerRef);
            }
        }

        public void SetState(IMatchState newState)
        {
            if (!HasStateAuthority) return;

            CurrentState?.Exit();
            CurrentState = newState;
            CurrentState.Enter();

            Rpc_OnStateChanged(CurrentState.ToEnum());
        }

        public void RespawnPlayer(PlayerRef playerRef)
        {
            if (!HasStateAuthority) return;

            if (Players.TryGetValue(playerRef, out var player))
            {
                var spawnPoint = Map.SpawnPoints[Random.Range(0, Map.SpawnPoints.Count)];

                player.Respawn(spawnPoint.transform);
                Rpc_PlayerRespawned(playerRef);
            }
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void Rpc_PlayerRespawned(PlayerRef playerRef)
        {
            if (Runner.LocalPlayer == playerRef)
            {
                GameEventBus.Instance.RaiseEvent(GameEventDefinitions.PlayerRespawned, new EmptyEventArgs());
            }
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void Rpc_StatisticChanged(PlayerRef player, PlayerStatisticNetwork playerStatisticNetwork)
        {
            if (Runner.LocalPlayer == player)
            {
                GameEventBus.Instance.RaiseEvent(
                    GameEventDefinitions.PlayerMatchStatsChanged,
                    new PlayerStatsChangedEventArgs(player, playerStatisticNetwork)
                );
            }
        }
        
        private void HandlePlayerDeath(PlayerRef victim, PlayerRef killer)
        {
            if (!HasStateAuthority) return;

            MatchStatistic.AddDeath(victim);

            Rpc_StatisticChanged(victim, MatchStatistic.GetNetworkPlayerStatistic(victim));

            if (Players.TryGetValue(victim, out var player))
            {
                if (player.NetworkHealth.CurrentLives <= 0)
                {
                    Rpc_NotifyPlayerLost(victim);
                    AlivePlayers.Remove(victim);
                    return;
                }
            }
            
            _playersRespawner.AddPlayerToRespawn(victim, Runner);
            var respawnAt = _playersRespawner.GetRespawnAt(victim, Runner);
            Rpc_StartRespawn(victim, respawnAt);
        }

        private void HandleKill(PlayerRef killer, PlayerRef victim)
        {
            if(killer == victim)
               return;
            
            MatchStatistic.AddKill(killer);
            MatchScore.AddScore(killer, 1);
            
            Rpc_StatisticChanged(killer, MatchStatistic.GetNetworkPlayerStatistic(killer));
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void Rpc_StartRespawn(PlayerRef playerRef, float respawnAt)
        {
            if (Runner.LocalPlayer == playerRef)
            {
                GameEventBus.Instance.RaiseEvent(GameEventDefinitions.PlayerRespawnStarted, 
                    new StartRespawnEventArgs(respawnAt, Runner));
            }
        }

        private void OnPlayerDeath(IEventBusArgs args)
        {
            if (args is PlayerDeathEventArgs playerKilledEventArgs)
            {
                Debug.Log("On Player death match manager");
                CurrentState?.OnPlayerDeath(playerKilledEventArgs.DeathData.Victim, 
                    playerKilledEventArgs.DeathData.Killer);
            }
        }

        public void ProcessDeath(PlayerRef victim, PlayerRef killer)
        {
            if(!HasStateAuthority) return;

            if (victim == PlayerRef.None)
            {
                Debug.Log("Victim is not defined");
                return;
            }
            Debug.Log("Processing Death in Match Manager");
            HandleKill(killer, victim);
            HandlePlayerDeath(victim, killer);
        }
        

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void Rpc_OnStateChanged(MatchStateEnum matchStateEnum)
        {
            GameEventBus.Instance.RaiseEvent(GameEventDefinitions.MatchStateChanged,
                new MatchStateChangedEventArgs(matchStateEnum));
        }

        public void SendAllStatistic()
        {
            var statsGenerator = new NetworkStatsGenerator(Runner.ActivePlayers.ToList(), MatchStatistic, MatchScore);
            
            Rpc_SendAllStatistics(statsGenerator.GenerateStatistics());
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void Rpc_SendAllStatistics(NetworkStatsData[] statsData)
        {
            GameEventBus.Instance.RaiseEvent(GameEventDefinitions.LeaderboardDataAvailable, new ShowLeaderboardEventArgs(statsData));
            foreach (var stat in statsData) 
            {
                Debug.Log($"{stat.Owner} - Kills: {stat.Kills}, Deaths: {stat.Deaths}");
            }
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void Rpc_NotifyPlayerLost(PlayerRef playerRef)
        {
            if (Runner.LocalPlayer == playerRef)
            {
                GameEventBus.Instance.RaiseEvent(GameEventDefinitions.PlayerLost, new EmptyEventArgs());
            }
        }
    }
}