using System.Collections.Generic;
using System.Collections.ObjectModel;
using Fusion;
using TMPro;
using UnityEngine;

namespace Core.MatchmakingComponents.ScoreSystem
{
    public class MatchStatistic
    {
        private readonly Dictionary<PlayerRef, PlayerStatistic> _playersStatistics = new();

        public void AddKill(PlayerRef playerRef)
        {
            if (_playersStatistics.ContainsKey(playerRef))
            {
                _playersStatistics[playerRef].AddKill();
                Debug.Log(_playersStatistics[playerRef].Kills);
            }
            else
            {
                Debug.Log("New statistic added");
                _playersStatistics[playerRef] = new PlayerStatistic(1, 0);
            }
        }

        public void AddDeath(PlayerRef playerRef)
        {
            if (_playersStatistics.TryGetValue(playerRef, out var statistic))
            {
                statistic.AddDeath();
            }
            else
            {
                _playersStatistics[playerRef] = new PlayerStatistic(0, 1);
            }
        }

        public PlayerStatistic GetPlayerStatistic(PlayerRef playerRef)
        {
            return _playersStatistics.GetValueOrDefault(playerRef, new PlayerStatistic(0, 0));
        }
        
        public PlayerStatisticNetwork GetNetworkPlayerStatistic(PlayerRef playerRef)
        {
            var statistic = GetPlayerStatistic(playerRef);
            return new PlayerStatisticNetwork(statistic.Kills, statistic.Deaths);
        }
        
        public ReadOnlyDictionary<PlayerRef, PlayerStatistic> GetAllPlayersStatistics()
        {
            return new ReadOnlyDictionary<PlayerRef, PlayerStatistic>(_playersStatistics);
        }
    }
}