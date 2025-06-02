using System.Collections.Generic;
using System.Linq;
using Core.MatchmakingComponents.MatchStates;
using Core.MatchmakingComponents.ScoreSystem;
using Core.PlayerComponents;
using Data;
using Environment;
using Fusion;
using Infrastructure.MatchStates;
using Services.EventBus;
using Services.EventBus.EventBusArguments;
using UnityEngine;
using MatchStateEnum = Core.MatchmakingComponents.MatchStates.MatchStateEnum;

namespace Core.MatchmakingComponents
{
    public class MatchManager : NetworkBehaviour, IMatchContext
    {
        public Dictionary<PlayerRef, Player> Players { get; private set; }
        public List<PlayerRef> AlivePlayers { get; private set; }
        public MatchTimer MatchTimer { get; private set; }
        public Map Map { get; private set; }
        public MatchScore MatchScore { get; private set; }
        public MatchStatistic MatchStatistic { get; private set; }

        private WeaponDealer _weaponDealer;
        private PlayersRespawner _playersRespawner;

        private IMatchState CurrentState { get; set; }

        public override void Spawned()
        {
            MatchScore = new MatchScore();
            MatchStatistic = new MatchStatistic();
            _playersRespawner = new PlayersRespawner(this);
        }

        public override void FixedUpdateNetwork()
        {
            if (!HasStateAuthority) return;

            CurrentState?.Update();
            _playersRespawner?.Update(Runner);
        }

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

            GameEventBus.Instance.Subscribe(GameEventDefinitions.PlayerDeath, OnPlayerDeath);
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
        private void Rpc_StatisticChanged(PlayerRef player, PlayerStatistic playerStatistic)
        {
            if (Runner.LocalPlayer == player)
            {
                GameEventBus.Instance.RaiseEvent(
                    GameEventDefinitions.PlayerMatchStatsChanged,
                    new PlayerStatsChangedEventArgs(player, playerStatistic)
                );
            }
        }
        
        private void HandlePlayerDeath(PlayerRef victim)
        {
            if (!HasStateAuthority) return;

            MatchStatistic.AddDeath(victim);
            
            Rpc_StatisticChanged(victim, MatchStatistic.GetPlayerStatistic(victim));

            if (Players.TryGetValue(victim, out var player))
            {
                if (player.NetworkHealth.CurrentLives <= 0)
                {
                    AlivePlayers.Remove(victim);
                    return;
                }
            }
            
            _playersRespawner.AddPlayerToRespawn(victim, Runner);
            var respawnAt = _playersRespawner.GetRespawnAt(victim, Runner);
            Rpc_StartRespawn(victim, respawnAt);
        }

        private void HandleKill(PlayerRef killer)
        {
            MatchStatistic.AddKill(killer);
            MatchScore.AddScore(killer, 1);
            
            Rpc_StatisticChanged(killer, MatchStatistic.GetPlayerStatistic(killer));
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
                CurrentState?.OnPlayerDeath(playerKilledEventArgs.DeathData.Victim, playerKilledEventArgs.DeathData.Killer);
            }
        }

        public void ProcessDeath(PlayerRef victim, PlayerRef killer)
        {
            if(!HasStateAuthority) return;
            
            HandleKill(killer);
            HandlePlayerDeath(victim);
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
    }
}