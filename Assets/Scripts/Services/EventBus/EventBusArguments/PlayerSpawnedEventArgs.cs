using Core;

namespace Services.EventBus.EventBusArguments
{
    public class PlayerSpawnedEventArgs : IEventBusArgs
    {
        public Player Player { get; private set; }

        public PlayerSpawnedEventArgs(Player player)
        {
            Player = player;
        }
    }
}