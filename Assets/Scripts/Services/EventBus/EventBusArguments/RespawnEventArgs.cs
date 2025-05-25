using Fusion;

namespace Services.EventBus.EventBusArguments
{
    public class RespawnEventArgs : IEventBusArgs
    {
        public RespawnEventArgs(float respawnAt)
        {
            RespawnAt = respawnAt;
        }

        public float RespawnAt { get; private set; }
    }
}