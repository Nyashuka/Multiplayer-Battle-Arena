using System.Collections.Generic;
using Fusion;

namespace Core.MatchmakingComponents.ScoreSystem
{
    public class MatchScore
    {
        private readonly Dictionary<PlayerRef, int> _playersScore = new Dictionary<PlayerRef, int>();
        public IReadOnlyDictionary<PlayerRef, int> AllScores => _playersScore;

        public void AddScore(PlayerRef player, int score)
        {
            if (_playersScore.ContainsKey(player))
            {
                _playersScore[player] += score;
            }
            else
            {
                SetScore(player, score);
            }
        }

        public void SetScore(PlayerRef player, int score)
        {
            _playersScore[player] = score;
        }
        
        public void ResetScore(PlayerRef player)
        {
            if (_playersScore.ContainsKey(player))
                _playersScore[player] = 0;
        }
        
        public int GetScore(PlayerRef player)
        {
            return _playersScore.GetValueOrDefault(player, 0);
        }

        public void ResetAllScores()
        {
            _playersScore.Clear();
        }
    }
}