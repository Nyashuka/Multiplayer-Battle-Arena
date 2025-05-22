namespace Services.EventBus
{
    public class GameEventBus : EventBusBase
    {
        public static GameEventBus Instance { get; } = new();
        private GameEventBus() { }
    }
}