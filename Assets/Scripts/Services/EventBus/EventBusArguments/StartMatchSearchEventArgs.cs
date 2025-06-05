namespace Services.EventBus.EventBusArguments
{
    public class StartMatchSearchEventArgs : IEventBusArgs
    {
        public int PlayersCount { get; private set; }

        
        public StartMatchSearchEventArgs(int playersCount)
        {
            PlayersCount = playersCount;
        }
    }
}