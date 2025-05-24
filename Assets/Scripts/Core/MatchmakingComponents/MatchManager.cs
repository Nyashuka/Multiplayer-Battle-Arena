using System.Collections.Generic;
using System.Linq;
using Core.MatchmakingComponents.ScoreSystem;
using Core.PlayerComponents;
using Data;
using Environment;
using Fusion;
using Services.EventBus;
using Services.EventBus.EventBusArguments;
using UnityEngine;

namespace Core.MatchmakingComponents
{
    public class MatchManager : NetworkBehaviour
    {
        private Dictionary<PlayerRef, Player> Players { get; set; }
        
        private MatchScore _matchScore;
        private MatchStatistic _matchStatistic;
        private MatchTimer _matchTimer;
        private Map _map;

        private readonly float _respawnTime = 10f;
        private Dictionary<PlayerRef, TickTimer> _respawnTimers = new();

        public void Initialize(Dictionary<PlayerRef, Player> players, MatchTimer matchTimer, Map map)
        {
            if(!HasStateAuthority) return;
            
            Players = players;
            _matchTimer = matchTimer;
            _matchTimer.StartMatchTimer();
            _map = map;
        }
        
        public override void Spawned()
        {
            _matchScore = new MatchScore();
            _matchStatistic = new MatchStatistic();
            
            GameEventBus.Instance.Subscribe(GameEventDefinitions.PlayerDeath, OnPlayerDeath);
        }

        private void HandlePlayerDeath(PlayerRef playerRef)
        {
            if (!_respawnTimers.ContainsKey(playerRef))
            {
                _respawnTimers.Add(playerRef, TickTimer.CreateFromSeconds(Runner, _respawnTime));
            }

            GameEventBus.Instance.RaiseEvent(
                GameEventDefinitions.StatisticsChanged, 
                new StatisticsChangedEventArgs(Runner.LocalPlayer, _matchStatistic)
            );
            
            if(!HasStateAuthority) return;
            
            RespawnPlayer(playerRef);
        }

        private void RespawnPlayer(PlayerRef playerRef)
        {
            Player player = Players[playerRef];
            var spawnPoint = _map.SpawnPoints[Random.Range(0, _map.SpawnPoints.Count)];
            player.Respawn(spawnPoint.transform.position);
        }

        private void OnPlayerDeath(IEventBusArgs args)
        {
            if (args is PlayerDeathEventArgs playerKilledEventArgs)
            {
                _matchStatistic.AddKill(playerKilledEventArgs.DeathData.Killer);
                _matchStatistic.AddDeath(playerKilledEventArgs.DeathData.Victim);
                _matchScore.AddScore(playerKilledEventArgs.DeathData.Killer, 1);
                
                HandlePlayerDeath(playerKilledEventArgs.DeathData.Victim);
            }
        }
        
        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void Rpc_NotifyNewPlayerScore(PlayerRef player, int score)
        {
            if(HasStateAuthority) return;

            Debug.Log(player + " | score: " + score);
            _matchScore.SetScore(player, score);
        }
    }
}