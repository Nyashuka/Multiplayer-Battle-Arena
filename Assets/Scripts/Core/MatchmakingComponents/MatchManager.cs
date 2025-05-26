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
        public static MatchManager Instance { get; private set; }

        private Dictionary<PlayerRef, Player> Players { get; set; }

        private MatchScore _matchScore;
        private MatchStatistic _matchStatistic;
        public MatchTimer MatchTimer { get; private set; }
        
        private Map _map;

        private readonly float _respawnTime = 10f;
        private readonly Dictionary<PlayerRef, TickTimer> _respawnTimers = new();

        public void Initialize(Dictionary<PlayerRef, Player> players, MatchTimer matchTimer, Map map)
        {
            Players = players;
            MatchTimer = matchTimer;
            _map = map;
            
            if (HasStateAuthority)
            {
                MatchTimer.StartMatchTimer();
            }
            
            GameEventBus.Instance.Subscribe(GameEventDefinitions.PlayerDeath, OnPlayerDeath);
            GameEventBus.Instance.RaiseEvent(GameEventDefinitions.MatchStarted, new MatchStartedEventArgs(Runner, MatchTimer),true);
        }

        public override void Spawned()
        {
            if (Instance)
            {
                if (HasStateAuthority)
                    Runner.Despawn(Object);
            }
            else
            {
                Instance = this;
            }

            _matchScore = new MatchScore();
            _matchStatistic = new MatchStatistic();

        }

        public override void FixedUpdateNetwork()
        {
            var expiredPlayers = new List<PlayerRef>();

            foreach (var (player, timer) in _respawnTimers)
            {
                if (timer.Expired(Runner))
                {
                    expiredPlayers.Add(player);
                    if (HasStateAuthority)
                    {
                        Respawn(player);
                    }
                }
            }

            foreach (var player in expiredPlayers)
            {
                _respawnTimers.Remove(player);
            }
        }

        private void Respawn(PlayerRef playerRef)
        {
            var spawnPoint = _map.SpawnPoints[Random.Range(0, _map.SpawnPoints.Count)];
            
            Players[playerRef].Respawn(spawnPoint.transform);
        }

        private void HandlePlayerDeath(PlayerRef playerRef)
        {
            if (!_respawnTimers.ContainsKey(playerRef))
            {
                _respawnTimers.Add(playerRef, TickTimer.CreateFromSeconds(Runner, _respawnTime));
            }

            var respawnAt = Runner.SimulationTime + _respawnTime;
            
            GameEventBus.Instance.RaiseEvent(
                GameEventDefinitions.StatisticsChanged, 
                new StatisticsChangedEventArgs(Runner.LocalPlayer, _matchStatistic)
            );
            
            if (Runner.LocalPlayer == playerRef)
            {
                GameEventBus.Instance.RaiseEvent(GameEventDefinitions.StartRespawn, new StartRespawnEventArgs(respawnAt));
            }
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
    }
}