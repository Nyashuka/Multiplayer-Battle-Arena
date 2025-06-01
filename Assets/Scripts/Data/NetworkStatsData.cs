using Fusion;

namespace Data
{
    public struct NetworkStatsData : INetworkStruct
    {
        public NetworkStatsData(PlayerRef owner, int kills, int deaths, float kd, int points)
        {
            Owner = owner;
            Kills = kills;
            Deaths = deaths;
            Points = points;
            Kd = kd;
        }

        public PlayerRef Owner { get; private set; }
        public int Kills { get; private set; }
        public int Deaths { get; private set; }
        public float Kd { get; private set; }
        public int Points { get; private set; }
    }
}