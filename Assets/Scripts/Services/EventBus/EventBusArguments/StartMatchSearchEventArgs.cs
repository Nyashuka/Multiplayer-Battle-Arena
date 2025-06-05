namespace Services.EventBus.EventBusArguments
{
    public class StartMatchSearchEventArgs : IEventBusArgs
    {
        public int PlayersCount { get; private set; }
        public string PlayerName { get; private set; }

        
        public StartMatchSearchEventArgs(int playersCount, string playerName)
        {
            PlayersCount = playersCount;
            PlayerName = playerName;
        }
    }
}