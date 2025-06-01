using System.Collections.Generic;
using Core.MatchmakingComponents.ScoreSystem;
using Data;
using Fusion;

namespace Core.MatchmakingComponents
{
    public class NetworkStatsGenerator
    {
        private readonly List<PlayerRef> _players;
        private readonly MatchStatistic _statistic;
        private readonly MatchScore _score;

        public NetworkStatsGenerator(List<PlayerRef> players, MatchStatistic statistic, MatchScore score)
        {
            _players = players;
            _statistic = statistic;
            _score = score;
        }

        public void FillStats(ref NetworkArray<NetworkStatsData> statisticsArray)
        {
            int i = 0;

            foreach (var player in _players)
            {
                var score = _score.GetScore(player);
                var stats = _statistic.GetPlayerStatistic(player);
                statisticsArray[i] = new NetworkStatsData(player, stats.Kills, stats.Deaths, (float)stats.Kills/stats.Deaths, score);

                i++;
            }
        }

        public NetworkStatsData[] GenerateStatistics()
        {
            int i = 0;
            NetworkStatsData[] statsArray = new NetworkStatsData[_players.Count];
        
            foreach (var player in _players)
            {
                var score = _score.GetScore(player);
                var stats = _statistic.GetPlayerStatistic(player);
                statsArray[i] = new NetworkStatsData(player, stats.Kills, stats.Deaths, (float)stats.Kills/stats.Deaths, score);
        
                i++;
            }

            return statsArray;
        }
    }
}