using System.Collections.Generic;
using Core.MatchmakingComponents.ScoreSystem;
using Core.PlayerComponents;
using Environment;
using Fusion;
using Infrastructure.MatchStates;
using Services.EventBus;
using Services.EventBus.EventBusArguments;
using UnityEngine;

namespace Core.MatchmakingComponents
{
    public class MatchManager : NetworkBehaviour, IMatchContext
    {
        public Dictionary<PlayerRef, Player> Players { get; private set;  }
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
            if(!HasStateAuthority) return;
            
            CurrentState?.Update();
            _playersRespawner?.Update(Runner);
        }
        
        public void Initialize(Dictionary<PlayerRef, Player> players, MatchTimer matchTimer, Map map, WeaponDealer weaponDealer)
        {
            Players = players;
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
            CurrentState?.Exit();
            CurrentState = newState;
            CurrentState.Enter();
            
            Rpc_OnStateChanged(CurrentState.ToEnum());
        }
        
        public void RespawnPlayer(PlayerRef playerRef)
        {
            if(!HasStateAuthority) return;
            
            var spawnPoint = Map.SpawnPoints[Random.Range(0, Map.SpawnPoints.Count)];
            
            Players[playerRef].Respawn(spawnPoint.transform);
            Rpc_PlayerRespawned(playerRef);
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void Rpc_PlayerRespawned(PlayerRef playerRef)
        {
            if (Runner.LocalPlayer == playerRef)
            {
                GameEventBus.Instance.RaiseEvent(GameEventDefinitions.PlayerRespawned, new EmptyEventArgs());
            }
        }

        private void HandlePlayerDeath(PlayerRef playerRef)
        {
            if(!HasStateAuthority) return;
            
            MatchStatistic.AddDeath(playerRef);
            
            _playersRespawner.AddPlayerToRespawn(playerRef, Runner);
            var respawnAt = _playersRespawner.GetRespawnAt(playerRef, Runner);
            Rpc_StartRespawn(playerRef, respawnAt);
            
            GameEventBus.Instance.RaiseEvent(
                GameEventDefinitions.StatisticsChanged, 
                new StatisticsChangedEventArgs(Runner.LocalPlayer, MatchStatistic)
            );
        }
        
        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void Rpc_StartRespawn(PlayerRef playerRef, float respawnAt)
        {
            if (Runner.LocalPlayer == playerRef)
            {
                GameEventBus.Instance.RaiseEvent(GameEventDefinitions.StartRespawn, 
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
            
            MatchStatistic.AddKill(killer);
            MatchScore.AddScore(killer, 1);
                
            HandlePlayerDeath(victim);
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void Rpc_OnStateChanged(MatchStateEnum matchStateEnum)
        {
            GameEventBus.Instance.RaiseEvent(GameEventDefinitions.MatchStateChanged,
                new MatchStateChangedEventArgs(matchStateEnum));
        }
    }
}