using Core.MatchmakingComponents.ScoreSystem;
using Fusion;

namespace Services.EventBus.EventBusArguments
{
    public class PlayerStatsChangedEventArgs : IEventBusArgs
    {
        public PlayerRef Owner { get; }
        public PlayerStatistic PlayerStatistic { get; }

        public PlayerStatsChangedEventArgs(PlayerRef owner, PlayerStatistic playerStatistic)
        {
            Owner = owner;
            PlayerStatistic = playerStatistic;
        }
    }
}