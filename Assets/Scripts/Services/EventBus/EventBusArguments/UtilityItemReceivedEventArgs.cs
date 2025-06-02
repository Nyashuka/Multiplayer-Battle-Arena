namespace Services.EventBus.EventBusArguments
{
    public class UtilityItemReceivedEventArgs : IEventBusArgs
    {
        public UtilityItemReceivedEventArgs(string id)
        {
            Id = id;
        }

        public string Id { get; } 
    }
}