using Core.MatchmakingComponents;
using Fusion;

namespace Services.EventBus.EventBusArguments
{
    public class MatchStartedEventArgs : IEventBusArgs
    {
        public MatchStartedEventArgs(NetworkRunner networkRunner, MatchTimer matchTimer)
        {
            NetworkRunner = networkRunner;
            MatchTimer = matchTimer;
        }

        public NetworkRunner NetworkRunner { get; private set; }
        public MatchTimer MatchTimer { get; private set; }
    }
}