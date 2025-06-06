using Fusion;

namespace Services.EventBus.EventBusArguments
{
    public class PlayerLeftMatchEventArgs : IEventBusArgs
    {
        public PlayerLeftMatchEventArgs(PlayerRef playerRef)
        {
            PlayerRef = playerRef;
        }

        public PlayerRef PlayerRef { get; private set; }
    }
}