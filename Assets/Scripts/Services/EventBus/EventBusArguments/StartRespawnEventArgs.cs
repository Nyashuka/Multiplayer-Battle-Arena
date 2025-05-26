namespace Services.EventBus.EventBusArguments
{
    public class StartRespawnEventArgs : IEventBusArgs
    {
        public StartRespawnEventArgs(float respawnAt)
        {
            RespawnAt = respawnAt;
        }

        public float RespawnAt { get; private set; }
    }
}