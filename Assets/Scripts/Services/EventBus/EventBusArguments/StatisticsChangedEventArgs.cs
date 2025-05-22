using Core.MatchmakingComponents.ScoreSystem;
using Fusion;

namespace Services.EventBus.EventBusArguments
{
    public class StatisticsChangedEventArgs : IEventBusArgs
    {
        public PlayerRef Owner { get; }
        public MatchStatistic MatchStatistic { get; }

        public StatisticsChangedEventArgs(PlayerRef owner, MatchStatistic matchStatistic)
        {
            Owner = owner;
            MatchStatistic = matchStatistic;
        }
    }
}