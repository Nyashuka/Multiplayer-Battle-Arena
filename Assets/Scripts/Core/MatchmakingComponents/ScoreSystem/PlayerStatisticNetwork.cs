
using Fusion;

namespace Core.MatchmakingComponents.ScoreSystem
{
    public struct PlayerStatisticNetwork : INetworkStruct
    {
        public int Kills { get; set; }
        public int Deaths { get; set; }
        
        public PlayerStatisticNetwork(int kills, int deaths)
        {
            Kills = kills;
            Deaths = deaths;
        }
    }
}