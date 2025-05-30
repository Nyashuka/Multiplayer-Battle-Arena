using Core.MatchmakingComponents;
using Fusion;

namespace Services.EventBus.EventBusArguments
{
    public class MatchTimerChangedEventArgs : IEventBusArgs
    {
        public MatchTimerChangedEventArgs(NetworkRunner networkRunner, MatchTimer matchTimer)
        {
            NetworkRunner = networkRunner;
            MatchTimer = matchTimer;
        }

        public NetworkRunner NetworkRunner { get; private set; }
        public MatchTimer MatchTimer { get; private set; } 
    }
}