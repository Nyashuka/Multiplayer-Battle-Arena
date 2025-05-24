using Data;

namespace Services.EventBus.EventBusArguments
{
    public class PlayerDeathEventArgs : IEventBusArgs
    {
        public DeathData DeathData { get; private set; }
        
        public PlayerDeathEventArgs(DeathData deathData)
        {
            DeathData = deathData;
        }
    }
}