using Data;

namespace Services.EventBus.EventBusArguments
{
    public class PlayerKilledEventArgs : IEventBusArgs
    {
        public DeathData DeathData { get; private set; }
        
        public PlayerKilledEventArgs(DeathData deathData)
        {
            DeathData = deathData;
        }
    }
}