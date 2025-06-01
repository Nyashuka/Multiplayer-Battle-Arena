
using Fusion;

namespace Core.MatchmakingComponents.ScoreSystem
{
    public struct PlayerStatistic : INetworkStruct
    {
        public int Kills { get; set; }
        public int Deaths { get; set; }
        
        public PlayerStatistic(int kills, int deaths)
        {
            Kills = kills;
            Deaths = deaths;
        }

        public void AddKill()
        {
            Kills++;
        }
        
        public void AddDeath()
        {
            Deaths++;
        }
    }
}