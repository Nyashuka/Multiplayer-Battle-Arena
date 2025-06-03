using Core.MatchmakingComponents.ScoreSystem;
using Fusion;

namespace Services.EventBus.EventBusArguments
{
    public class PlayerStatsChangedEventArgs : IEventBusArgs
    {
        public PlayerRef Owner { get; }
        public PlayerStatisticNetwork PlayerStatisticNetwork { get; }

        public PlayerStatsChangedEventArgs(PlayerRef owner, PlayerStatisticNetwork playerStatisticNetwork)
        {
            Owner = owner;
            PlayerStatisticNetwork = playerStatisticNetwork;
        }
    }
}