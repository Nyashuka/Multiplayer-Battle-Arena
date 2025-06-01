using Data;

namespace Services.EventBus.EventBusArguments
{
    public class ShowLeaderboardEventArgs : IEventBusArgs
    {
        public ShowLeaderboardEventArgs(NetworkStatsData[] leaderboardData)
        {
            LeaderboardData = leaderboardData;
        }

        public NetworkStatsData[] LeaderboardData { get; }
    }
}