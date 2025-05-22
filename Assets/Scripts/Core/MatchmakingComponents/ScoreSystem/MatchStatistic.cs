using System.Collections.Generic;
using System.Collections.ObjectModel;
using Fusion;

namespace Core.MatchmakingComponents.ScoreSystem
{
    public class MatchStatistic
    {
        private readonly Dictionary<PlayerRef, PlayerStatistic> _playersStatistics = new();

        public void AddKill(PlayerRef playerRef)
        {
            if (_playersStatistics.TryGetValue(playerRef, out var statistic))
            {
                statistic.AddKill();
            }
            else
            {
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
        
        public ReadOnlyDictionary<PlayerRef, PlayerStatistic> GetAllPlayersStatistics()
        {
            return new ReadOnlyDictionary<PlayerRef, PlayerStatistic>(_playersStatistics);
        }
    }
}